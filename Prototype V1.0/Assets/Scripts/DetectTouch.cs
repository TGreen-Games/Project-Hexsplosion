using System.Collections.Generic;
using UnityEngine;

public class DetectTouch : MonoBehaviour
{


	public delegate void OnTouch(RaycastHit2D position);
	public event OnTouch onTouch;
	private RaycastHit2D hit;
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
		Debug.Log(string.Format(text, format));
	}
	private RaycastHit2D touchPosition;
	public RaycastHit2D TouchPosition
	{
		get { return touchPosition; }
		set
		{
				touchPosition = value;
				if (onTouch != null)
					onTouch(touchPosition);
		}

		}

	void Start()
	{
		CreateTapGesture();
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
			hit = Physics2D.GetRayIntersection(ray, 10);
			TouchPosition = hit;

		}

	}
}