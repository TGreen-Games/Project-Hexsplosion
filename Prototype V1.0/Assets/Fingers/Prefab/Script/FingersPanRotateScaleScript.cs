using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DigitalRubyShared
{
    /// <summary>
    /// Allows two finger pan, scale and rotate on a 2d game object
    /// </summary>
    public class FingersPanRotateScaleScript : MonoBehaviour
    {
        [Tooltip("The camera to use to convert screen coordinates to world coordinates. Defaults to Camera.main.")]
        public Camera Camera;

        [Tooltip("Whether to bring the object to the front when a gesture executes on it")]
        public bool BringToFront = true;

        [Tooltip("Minimum touch count to start panning. Rotating and scaling always requires two fingers. This should be 1 or 2.")]
        public int PanMinimumTouchCount = 2;

        private Rigidbody2D rigidBody;
        private SpriteRenderer spriteRenderer;
        private int startSortOrder;
        private Vector2 panStart;
        private PanGestureRecognizer panGesture;
        private ScaleGestureRecognizer scaleGesture;
        private RotateGestureRecognizer rotateGesture;

        private static readonly List<RaycastResult> captureRaycastResults = new List<RaycastResult>();

        public static void StartOrResetGesture(GestureRecognizer r, bool bringToFront, Camera camera, GameObject obj, SpriteRenderer spriteRenderer)
        {
            if (r.State == GestureRecognizerState.Began)
            {
                if (GestureIntersectsObject(r, camera, obj))
                {
                    if (bringToFront)
                    {
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.sortingOrder = 1000;
                        }
                    }
                }
                else
                {
                    r.Reset();
                }
            }
        }

        private static int RaycastResultCompare(RaycastResult r1, RaycastResult r2)
        {
            SpriteRenderer rend1 = r1.gameObject.GetComponent<SpriteRenderer>();
            if (rend1 != null)
            {
                SpriteRenderer rend2 = r2.gameObject.GetComponent<SpriteRenderer>();
                if (rend2 != null)
                {
                    int comp = rend2.sortingLayerID.CompareTo(rend1.sortingLayerID);
                    if (comp == 0)
                    {
                        comp = rend2.sortingOrder.CompareTo(rend1.sortingOrder);
                    }
                    return comp;
                }
            }
            return 0;
        }

        private static bool GestureIntersectsObject(GestureRecognizer r, Camera camera, GameObject obj)
        {
            captureRaycastResults.Clear();
            PointerEventData p = new PointerEventData(EventSystem.current);
            p.position = new Vector2(r.FocusX, r.FocusY);
            p.clickCount = 1;
            p.dragging = false;
            EventSystem.current.RaycastAll(p, captureRaycastResults);
            captureRaycastResults.Sort(RaycastResultCompare);

            foreach (RaycastResult result in captureRaycastResults)
            {
                if (result.gameObject == obj)
                {
                    return true;
                }
                else if (result.gameObject.GetComponent<Collider>() != null ||
                    result.gameObject.GetComponent<Collider2D>() != null ||
                    result.gameObject.GetComponent<FingersPanRotateScaleScript>() != null)
                {
                    // blocked by a collider or gesture, bail
                    break;
                }
            }
            return false;
        }

        private void PanGestureUpdated(GestureRecognizer r, ICollection<GestureTouch> touches)
        {
            StartOrResetGesture(r, BringToFront, Camera, gameObject, spriteRenderer);
            if (r.State == GestureRecognizerState.Began)
            {
                panStart = (rigidBody == null ? (Vector2)gameObject.transform.position : rigidBody.position);
            }
            else if (r.State == GestureRecognizerState.Executing)
            {
                Vector2 screenMovement = new Vector2(panGesture.DistanceX, panGesture.DistanceY);
                Vector2 worldMovement = Camera.ScreenToWorldPoint(screenMovement) - Camera.ScreenToWorldPoint(Vector2.zero);
                //Debug.LogFormat("Screen movement: {0}, World movement: {1}", screenMovement, worldMovement);
                if (rigidBody == null)
                {
                    transform.position = panStart + worldMovement;
                }
                else
                {
                    rigidBody.MovePosition(panStart + worldMovement);
                }
            }
            else if (r.State == GestureRecognizerState.Ended)
            {
                if (spriteRenderer != null && BringToFront)
                {
                    spriteRenderer.sortingOrder = startSortOrder;
                }
            }
        }

        private void ScaleGestureUpdated(GestureRecognizer r, ICollection<GestureTouch> touches)
        {
            StartOrResetGesture(r, BringToFront, Camera, gameObject, spriteRenderer);
            if (r.State == GestureRecognizerState.Executing)
            {
                transform.localScale *= scaleGesture.ScaleMultiplier;
            }
        }

        private void RotateGestureUpdated(GestureRecognizer r, ICollection<GestureTouch> touches)
        {
            StartOrResetGesture(r, BringToFront, Camera, gameObject, spriteRenderer);
            if (r.State == GestureRecognizerState.Executing)
            {
                if (rigidBody == null)
                {
                    transform.Rotate(Vector3.forward, rotateGesture.RotationDegreesDelta, Space.Self);
                }
                else
                {
                    rigidBody.MoveRotation(rigidBody.rotation + rotateGesture.RotationDegreesDelta);
                }
            }
        }

        private void Start()
        {
            if (FingersScript.Instance == null)
            {
                Debug.LogError("Fingers script prefab needs to be added to the first scene");
                return;
            }

            this.Camera = (this.Camera == null ? Camera.main : this.Camera);
            panGesture = new PanGestureRecognizer();
            panGesture.MinimumNumberOfTouchesToTrack = PanMinimumTouchCount;
            panGesture.Updated += PanGestureUpdated;
            scaleGesture = new ScaleGestureRecognizer();
            scaleGesture.Updated += ScaleGestureUpdated;
            rotateGesture = new RotateGestureRecognizer();
            rotateGesture.Updated += RotateGestureUpdated;
            rigidBody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                startSortOrder = spriteRenderer.sortingOrder;
            }
            panGesture.AllowSimultaneousExecution(scaleGesture);
            panGesture.AllowSimultaneousExecution(rotateGesture);
            scaleGesture.AllowSimultaneousExecution(rotateGesture);
            FingersScript.Instance.AddGesture(panGesture);
            FingersScript.Instance.AddGesture(scaleGesture);
            FingersScript.Instance.AddGesture(rotateGesture);
        }

        private void Update()
        {
        }
    }
}
