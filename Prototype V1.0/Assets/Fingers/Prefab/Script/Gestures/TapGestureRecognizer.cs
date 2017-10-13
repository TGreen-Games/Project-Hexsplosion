//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

using System;
using System.Diagnostics;

#if PCL || PORTABLE || HAS_TASKS

using System.Threading.Tasks;

#endif

namespace DigitalRubyShared
{
    /// <summary>
    /// A tap gesture detects one or more consecutive taps. The ended state denotes a successful tap.
    /// </summary>
    public class TapGestureRecognizer : GestureRecognizer
    {
        private int tapCount;
        private readonly Stopwatch timer = new Stopwatch();

        private void VerifyFailGestureAfterDelay()
        {
            float elapsed = (float)timer.Elapsed.TotalSeconds;
            if (State == GestureRecognizerState.Possible && elapsed > ThresholdSeconds)
            {
                SetState(GestureRecognizerState.Failed);
            }
        }

        private void FailGestureAfterDelayIfNoTap()
        {
            RunActionAfterDelay(ThresholdSeconds, VerifyFailGestureAfterDelay);
        }

        protected override void StateChanged()
        {
            base.StateChanged();

            if (State == GestureRecognizerState.Failed || State == GestureRecognizerState.Ended)
            {
                tapCount = 0;
                timer.Reset();
            }
        }

        protected override void TouchesBegan(System.Collections.Generic.IEnumerable<GestureTouch> touches)
        {
            if (State == GestureRecognizerState.Possible && TrackTouches(touches) != 0)
            {
                CalculateFocus(CurrentTrackedTouches);
                if (tapCount == 0)
                {
                    TapX = FocusX;
                    TapY = FocusY;
                }
                timer.Reset();
                timer.Start();
                if (SendBeginState)
                {
                    SetState(GestureRecognizerState.Began);
                }
            }
        }

        protected override void TouchesMoved()
        {
            CalculateFocus(CurrentTrackedTouches);
        }

        protected override void TouchesEnded()
        {
            if ((float)timer.Elapsed.TotalSeconds <= ThresholdSeconds)
            {
                CalculateFocus(CurrentTrackedTouches);
                GestureTouch t = CurrentTrackedTouches[0];
                if (PointsAreWithinDistance(TapX, TapY, t.X, t.Y, ThresholdUnits))
                {
                    if (++tapCount == NumberOfTapsRequired)
                    {
                        SetState(GestureRecognizerState.Ended);
                    }
                    else
                    {
                        StopTrackingAllTouches();
                        timer.Reset();
                        timer.Start();
                        FailGestureAfterDelayIfNoTap();
                    }
                }
                else
                {
                    SetState(GestureRecognizerState.Failed);
                }
            }
            else
            {
                // too much time elapsed, fail the gesture
                SetState(GestureRecognizerState.Failed);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TapGestureRecognizer()
        {
            NumberOfTapsRequired = 1;
            ThresholdSeconds = 0.35f;
            ThresholdUnits = 0.3f;
        }

        /// <summary>
        /// How many taps must execute in order to end the gesture
        /// </summary>
        /// <value>The number of taps required to execute the gesture</value>
        public int NumberOfTapsRequired { get; set; }

        /// <summary>
        /// How many seconds can expire before the tap is released to still count as a tap
        /// </summary>
        /// <value>The threshold in seconds</value>
        public float ThresholdSeconds { get; set; }

        /// <summary>
        /// How many units away the tap down and up and subsequent taps can be to still be considered - must be greater than 0. Default is 0.3.
        /// </summary>
        /// <value>The threshold in units</value>
        public float ThresholdUnits { get; set; }

        /// <summary>
        /// Tap start location x value
        /// </summary>
        public float TapX { get; private set; }

        /// <summary>
        /// Tap start location y value
        /// </summary>
        public float TapY { get; private set; }

        /// <summary>
        /// Whether the tap gesture will immediately send a begin state when a touch is first down. Default is false.
        /// </summary>
        public bool SendBeginState { get; set; }
    }
}

