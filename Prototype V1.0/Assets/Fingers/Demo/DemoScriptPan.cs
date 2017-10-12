using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalRubyShared
{
    public class DemoScriptPan : MonoBehaviour
    {
        public FingersScript FingersScript;

        private void Start()
        {
            PanGestureRecognizer pan = new PanGestureRecognizer();
            pan.Updated += Pan_Updated;
            FingersScript.AddGesture(pan);
        }

        private void Pan_Updated(GestureRecognizer gesture, ICollection<GestureTouch> touches)
        {
            Debug.LogFormat("Pan gesture, state: {0}, position: {1},{2}", gesture.State, gesture.FocusX, gesture.FocusY);
        }

        private void Update()
        {

        }
    }
}