//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

#if PCL || PORTABLE || HAS_TASKS || NETFX_CORE

#define USE_TASKS

using System.Threading.Tasks;

#endif

using System;
using System.Diagnostics;

namespace DigitalRubyShared
{
    /// <summary>
    /// A long press gesture detects a tap and hold and then calls back for movement until
    /// the touch is released
    /// </summary>
    public class LongPressGestureRecognizer : GestureRecognizer
    {
        private int tag;

        private void PrepareFailGestureAfterDelay()
        {
            CalculateFocus(CurrentTrackedTouches);
            SetState(GestureRecognizerState.Possible);
        }

        private void VerifyFailGestureAfterDelay(int tag)
        {
            // if the gesture is still possible and we have the same tag, start the gesture, otherwise fail it
            if (this.tag == tag && State == GestureRecognizerState.Possible)
            {
                CalculateFocus(CurrentTrackedTouches);
                SetState(GestureRecognizerState.Began);
            }
            else
            {
                SetState(GestureRecognizerState.Failed);
            }
        }

#if USE_TASKS

        private async void AttemptToStartAfterDelay()
        {
            int tempTag = ++tag;
            PrepareFailGestureAfterDelay();

            await Task.Delay(TimeSpan.FromSeconds(MinimumDurationSeconds));

            VerifyFailGestureAfterDelay(tempTag);
        }

#else

        private void AttemptToStartAfterDelay()
        {
            int tempTag = ++tag;
            PrepareFailGestureAfterDelay();

            MainThreadCallback(MinimumDurationSeconds, () =>
            {
                VerifyFailGestureAfterDelay(tempTag);
            });
        }

#endif

        protected override void StateChanged()
        {
            base.StateChanged();

            if (State == GestureRecognizerState.Ended || State == GestureRecognizerState.Failed)
            {
                tag++;
            }
        }

        protected override void TouchesBegan(System.Collections.Generic.IEnumerable<GestureTouch> touches)
        {
            if (State == GestureRecognizerState.Possible && TrackTouches(touches) == 1)
            {
                AttemptToStartAfterDelay();
            }
        }

        protected override void TouchesMoved()
        {
            CalculateFocus(CurrentTrackedTouches);
            if (State == GestureRecognizerState.Began || State == GestureRecognizerState.Executing)
            {
                SetState(GestureRecognizerState.Executing);
            }
            else if (State == GestureRecognizerState.Possible)
            {
                // if the touch moved too far to count as a long tap, fail the gesture
                float distance = Distance(DistanceX, DistanceY);
                if (distance > ThresholdUnits)
                {
                    SetState(GestureRecognizerState.Failed);
                }
                else
                {
                    SetState(GestureRecognizerState.Possible);
                }
            }
        }

        protected override void TouchesEnded()
        {
            if (State == GestureRecognizerState.Began || State == GestureRecognizerState.Executing)
            {
                CalculateFocus(CurrentTrackedTouches);
                SetState(GestureRecognizerState.Ended);
            }
            else
            {
                // touch came up too soon, fail the gesture
                SetState(GestureRecognizerState.Failed);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LongPressGestureRecognizer()
        {
            MinimumDurationSeconds = 0.6f;
            ThresholdUnits = 0.35f;
        }

        /// <summary>
        /// The number of seconds that the touch must stay down to begin executing
        /// </summary>
        /// <value>The minimum long press duration in seconds</value>
        public float MinimumDurationSeconds { get; set; }

        /// <summary>
        /// How many units away the long press can move before failing. After the long press begins,
        /// it is allowed to move any distance and stay executing.
        /// </summary>
        /// <value>The threshold in units</value>
        public float ThresholdUnits { get; set; }
    }
}
