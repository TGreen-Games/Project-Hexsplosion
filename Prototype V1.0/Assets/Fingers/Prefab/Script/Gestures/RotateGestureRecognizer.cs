//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

using System;

namespace DigitalRubyShared
{
    /// <summary>
    /// A rotate gesture uses two touches to call back rotation angles as the two touches twist around a central point
    /// </summary>
    public class RotateGestureRecognizer : GestureRecognizer
    {
        private float startAngle = float.MinValue;
        private float previousAngle;

        private float DifferenceBetweenAngles(float angle1, float angle2)
        {
            float angle = angle1 - angle2;
            //return ((angle + (float)Math.PI) % ((float)Math.PI * 2.0f)) - (float)Math.PI;
            return (float)Math.Atan2(Math.Sin(angle), Math.Cos(angle));
        }

        private void UpdateAngle()
        {
            float currentAngle = CurrentAngle();
            RotationRadians = DifferenceBetweenAngles(currentAngle, startAngle);
            RotationRadiansDelta = DifferenceBetweenAngles(currentAngle, previousAngle);
            previousAngle = currentAngle;
            CalculateFocus(CurrentTrackedTouches);
            SetState(GestureRecognizerState.Executing);
        }

        private void CheckForStart()
        {
            CalculateFocus(CurrentTrackedTouches);

            if (Distance(DistanceX, DistanceY) < ThresholdUnits)
            {
                return;
            }

            float angle = CurrentAngle();
            if (startAngle == float.MinValue)
            {
                startAngle = previousAngle = angle;
            }
            else
            {
                float angleDiff = Math.Abs(DifferenceBetweenAngles(angle, startAngle));
                if (angleDiff >= AngleThreshold)
                {
                    startAngle = previousAngle = angle;
                    SetState(GestureRecognizerState.Began);
                }
            }
        }

        protected override void StateChanged()
        {
            base.StateChanged();

            if (State == GestureRecognizerState.Ended || State == GestureRecognizerState.Failed)
            {
                startAngle = float.MinValue;
                RotationRadians = 0.0f;
            }
        }

        protected override void TouchesBegan(System.Collections.Generic.IEnumerable<GestureTouch> touches)
        {
            if (State == GestureRecognizerState.Possible && TrackTouches(touches) > 0)
            {
                CalculateFocus(CurrentTrackedTouches);
            }
        }

        protected override void TouchesMoved()
        {
            if (CurrentTrackedTouches.Count == MaximumNumberOfTouchesToTrack)
            {
                // we have the right number of touches to do the gesture, check if it's to start or execute
                if (State == GestureRecognizerState.Possible)
                {
                    CheckForStart();
                }
                else
                {
                    UpdateAngle();
                }
            }
        }

        protected override void TouchesEnded()
        {
            if (State == GestureRecognizerState.Possible)
            {
                // didn't move far enough to rotate, fail the gesture
                SetState(GestureRecognizerState.Failed);
            }
            else
            {
                CalculateFocus(CurrentTrackedTouches);
                SetState(GestureRecognizerState.Ended);
            }
        }

        /// <summary>
        /// Get the current angle
        /// </summary>
        /// <returns>Current angle</returns>
        protected virtual float CurrentAngle()
        {
            return (float)Math.Atan2(CurrentTrackedTouches[0].Y - CurrentTrackedTouches[1].Y, CurrentTrackedTouches[0].X - CurrentTrackedTouches[1].X);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RotateGestureRecognizer()
        {
            MaximumNumberOfTouchesToTrack = 2;
            AngleThreshold = 0.05f;
        }

        /// <summary>
        /// Angle threshold in radians that must be met before rotation starts - this is the amount of rotation that must happen to start the gesture.
        /// </summary>
        /// <value>The angle threshold.</value>
        public float AngleThreshold { get; set; }

        /// <summary>
        /// The gesture focus must change distance by this number of units from the start focus in order to start. Default is 0.0.
        /// </summary>
        /// <value>The threshold in units.</value>
        public float ThresholdUnits { get; set; }

        /// <summary>
        /// The current rotation angle in radians.
        /// </summary>
        /// <value>The rotation angle in radians.</value>
        public float RotationRadians { get; set; }

        /// <summary>
        /// The change in rotation radians.
        /// </summary>
        /// <value>The rotation radians delta.</value>
        public float RotationRadiansDelta { get; set; }

        /// <summary>
        /// The current rotation angle in degrees.
        /// </summary>
        /// <value>The rotation angle in degrees.</value>
        public float RotationDegrees { get { return RotationRadians * (180.0f / (float)Math.PI); } }

        /// <summary>
        /// The change in rotation degrees.
        /// </summary>
        /// <value>The rotation degrees delta.</value>
        public float RotationDegreesDelta { get { return RotationRadiansDelta * (180.0f / (float)Math.PI); } }
    }
}

