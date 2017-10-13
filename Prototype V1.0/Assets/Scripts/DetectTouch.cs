using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTouch : MonoBehaviour {


    public delegate void OnTouch(Vector2 position);
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
    private Vector2 touchPosition;
    public Vector2 TouchPosition
    {
        get { return touchPosition; }
        set
        {
            if (touchPosition == value) return;
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
            TouchPosition = Camera.main.ScreenToWorldPoint(new Vector2(touch.X, touch.Y));
                                  
        }

    }
}
