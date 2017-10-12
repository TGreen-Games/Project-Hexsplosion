using System;

namespace DigitalRubyShared
{
    public class OneTouchScaleGestureRecognizer : GestureRecognizer
    {
        public OneTouchScaleGestureRecognizer()
        {
            ScaleMultiplier = 1.0f;
            ThresholdUnits = 0.15f;

#if UNITY_5 || UNITY_6 || UNITY_5_3_OR_NEWER

            ZoomSpeed = -0.2f;

#else

            ZoomSpeed = 0.2f;

#endif

        }

        protected override void TouchesBegan(System.Collections.Generic.IEnumerable<GestureTouch> touches)
        {
            if (State == GestureRecognizerState.Possible && TrackTouches(touches) == 1)
            {
                CalculateFocus(CurrentTrackedTouches);
                SetState(GestureRecognizerState.Possible);
            }
        }

        protected override void TouchesMoved()
        {
            CalculateFocus(CurrentTrackedTouches);

            if (State == GestureRecognizerState.Possible)
            {
                // see if we have moved far enough to start scaling
                if (Distance(DistanceX, DistanceY) < ThresholdUnits)
                {
                    return;
                }
                ScaleMultiplier = 1.0f;
                SetState(GestureRecognizerState.Began);
            }
            else if (DeltaX != 0.0f || DeltaY != 0.0f)
            {
                float distance = Distance(DeltaX, DeltaY) * Math.Sign(DeltaY) * ZoomSpeed;
                ScaleMultiplier = 1.0f + distance;
                SetState(GestureRecognizerState.Executing);
            }
        }

        protected override void TouchesEnded()
        {
            if (State == GestureRecognizerState.Possible)
            {
                // didn't move far enough to start scaling, fail the gesture
                SetState(GestureRecognizerState.Failed);
            }
            else
            {
                CalculateFocus(CurrentTrackedTouches);
                SetState(GestureRecognizerState.Ended);
            }
        }

        /// <summary>
        /// The current scale multiplier. Multiply your current scale value by this to scale.
        /// </summary>
        /// <value>The scale multiplier.</value>
        public float ScaleMultiplier { get; set; }

        /// <summary>
        /// Additional multiplier for ScaleMultiplier. This will making scaling happen slower or faster.
        /// For this one finger scale gesture, this value is generally a small fraction, like 0.2 (the default).
        /// For a UI that starts 0,0 in the bottom left (like Unity), this value should be negative.
        /// For most other UI (0,0 in top left), this should be positive.
        /// </summary>
        /// <value>The zoom speed.</value>
        public float ZoomSpeed { get; set; }

        /// <summary>
        /// The threshold in units that the touch must move to start the gesture
        /// </summary>
        /// <value>The threshold units.</value>
        public float ThresholdUnits { get; set; }
    }
}