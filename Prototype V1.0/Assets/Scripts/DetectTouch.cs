using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTouch : MonoBehaviour {

    private RaycastHit2D hit;
    public delegate void OnTouch(RaycastHit2D position);
    public event OnTouch onTouch;
    private DigitalRubyShared.TapGestureRecognizer tapGesture;
    private DigitalRubyShared.GestureTouch FirstTouch(ICollection<DigitalRubyShared.GestureTouch> touches)
    {
        foreach (DigitalRubyShared.GestureTouch touch in touches)
        {
            return touch;
        }
        return new DigitalRubyShared.GestureTouch();
    }
    private void DebugText(string text, params object[] format)
    {
        //bottomLabel.text = string.Format(text, format);
        Debug.Log(string.Format(text, format));
    }
    private RaycastHit2D touchPosition;
    public RaycastHit2D TouchPosition
    {
        get { return touchPosition; }
        set
        {

            //if (touchPosition == value)
            //    return;
            //else{
            touchPosition = value;
            onTouch(touchPosition);
         }
           
        }


    // Use this for initialization
    void Start () {
        CreateTapGesture();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void CreateTapGesture()
    {
        tapGesture = new DigitalRubyShared.TapGestureRecognizer();
        tapGesture.Updated += TapGestureCallback;
        DigitalRubyShared.FingersScript.Instance.AddGesture(tapGesture);
    }

    private void TapGestureCallback(DigitalRubyShared.GestureRecognizer gesture, ICollection<DigitalRubyShared.GestureTouch> touches)
    {
        if (gesture.State == DigitalRubyShared.GestureRecognizerState.Ended)
        {
            DigitalRubyShared.GestureTouch touch = FirstTouch(touches);
            DebugText("Tapped at {0}, {1}", touch.X, touch.Y);
            var touchPoint = new Vector3(touch.X, touch.Y);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touchPoint);
            worldPoint.z = Camera.main.transform.position.z;

            Ray ray = new Ray(worldPoint, new Vector3(0, 0, 5));
            Debug.DrawRay(worldPoint, new Vector3(0, 0, 5),Color.green,100);
            hit = Physics2D.GetRayIntersection(ray,10);
            TouchPosition = hit;
           


            //RaycastHit hit;
            //Physics.Raycast(touchPoint, Vector3.forward, out hit);
            //Debug.DrawRay(touchPoint,Vector3.forward,Color.green);

            //Vector2 touchPoint = Camera.main.ScreenToWorldPoint(target);
            //Debug.Log(target);
            //Debug.Log(touchPoint); //ray starts here
            //RaycastHit2D hit = Physics2D.Raycast(touchPoint,Vector2.zero);
            //TouchPosition = hit;
            //Debug.Log(touchPosition + " and  " + TouchPosition);

            //TouchPosition = Camera.main.ScreenPointToRay(new Vector2(touch.X, touch.Y));

        }

    }
}