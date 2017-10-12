using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalRubyShared
{
    public class FingersJoystickScript : MonoBehaviour
    {
        [Tooltip("The image to move around like a joystick")]
        public Image JoystickImage;

        [Tooltip("Reduces the amount the joystick moves the closer it is to the center. As the joystick moves to it's max extents, the movement amount approaches 1. " +
            "For example, a power of 1 would be a linear equation, 2 would be squared, 3 cubed, etc.")]
        [Range(0.01f, 10.0f)]
        public float JoystickPower = 2.0f;

        [Tooltip("The max exten the joystick can move as a percentage of Screen.width + Screen.height")]
        [Range(0.001f, 0.2f)]
        public float MaxExtentPercent = 0.02f;

        private Vector2 startCenter;

        private void Start()
        {
            PanGesture = new PanGestureRecognizer
            {
                AllowSimultaneousExecutionWithAllGestures = true,
                PlatformSpecificView = JoystickImage.gameObject,
                ThresholdUnits = 0.0f
            };
            PanGesture.Updated += PanGestureUpdated;
            FingersScript.Instance.AddGesture(PanGesture);

#if UNITY_EDITOR

            if (JoystickImage == null || JoystickImage.canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                Debug.LogError("Fingers joystick script requires that JoystickImage be set and that the Canvas be in ScreenSpaceOverlay mode.");
            }

#endif

        }

        private void SetImagePosition(Vector2 pos)
        {
            JoystickImage.rectTransform.anchoredPosition = pos;
        }

        private void ExecuteCallback(Vector2 amount)
        {
            if (JoystickExecuted != null)
            {
                JoystickExecuted(this, amount);
            }
        }

        private void PanGestureUpdated(GestureRecognizer gesture, ICollection<GestureTouch> touches)
        {
            if (gesture.State == GestureRecognizerState.Executing)
            {
                // clamp joystick movement to max values
                float maxOffset = (Screen.width + Screen.height) * MaxExtentPercent;
                Vector2 offset = new Vector2(gesture.FocusX - gesture.StartFocusX, gesture.FocusY - gesture.StartFocusY);

                // check distance from center, clamp to distance
                offset = Vector2.ClampMagnitude(offset, maxOffset);

                // don't bother if no motion
                if (offset == Vector2.zero)
                {
                    return;
                }

                // move image
                SetImagePosition(startCenter + offset);

                // callback with movement weight
                if (JoystickPower >= 1.0f)
                {
                    // power is reducing offset, apply as is
                    offset.x = Mathf.Sign(offset.x) * Mathf.Pow(Mathf.Abs(offset.x) / maxOffset, JoystickPower);
                    offset.y = Mathf.Sign(offset.y) * Mathf.Pow(Mathf.Abs(offset.y) / maxOffset, JoystickPower);
                }
                else
                {
                    // power is increasing offset, we need to make sure we maintain the aspect ratio of offset
                    Vector2 absOffset = new Vector2(Mathf.Abs(offset.x), Mathf.Abs(offset.y));
                    float offsetTotal = absOffset.x + absOffset.y;
                    float xWeight = absOffset.x / offsetTotal;
                    float yWeight = absOffset.y / offsetTotal;
                    offset.x = xWeight * Mathf.Sign(offset.x) * Mathf.Pow(absOffset.x / maxOffset, JoystickPower);
                    offset.y = yWeight * Mathf.Sign(offset.y) * Mathf.Pow(absOffset.y / maxOffset, JoystickPower);
                    offset = Vector2.ClampMagnitude(offset, maxOffset);
                }
                ExecuteCallback(offset);
            }
            else if (gesture.State == GestureRecognizerState.Began)
            {
                startCenter = JoystickImage.rectTransform.anchoredPosition;
            }
            else if (gesture.State == GestureRecognizerState.Ended)
            {
                // return to center
                SetImagePosition(startCenter);

                // final callback
                ExecuteCallback(Vector2.zero);
            }
        }

        private void Update()
        {

        }

        public PanGestureRecognizer PanGesture { get; private set; }
        public System.Action<FingersJoystickScript, Vector2> JoystickExecuted;
    }
}
