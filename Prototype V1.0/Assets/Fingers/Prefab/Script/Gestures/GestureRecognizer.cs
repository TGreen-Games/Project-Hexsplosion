//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

using System;
using System.Collections.Generic;

namespace DigitalRubyShared
{
    /// <summary>
    /// Gesture recognizer states
    /// </summary>
    public enum GestureRecognizerState
    {
        /// <summary>
        /// Gesture is possible
        /// </summary>
        Possible,

        /// <summary>
        /// Gesture has started
        /// </summary>
        Began,

        /// <summary>
        /// Gesture is executing
        /// </summary>
        Executing,

        /// <summary>
        /// Gesture has ended
        /// </summary>
        Ended,

        /// <summary>
        /// End is pending, if the dependant gesture fails
        /// </summary>
        EndPending,

        /// <summary>
        /// Gesture has failed
        /// </summary>
        Failed
    }

    /// <summary>
    /// Contains a touch event
    /// </summary>
    public struct GestureTouch : IDisposable, IComparable<GestureTouch>
    {
        /// <summary>
        /// Invalid patform specific id
        /// </summary>
        public const int PlatformSpecificIdInvalid = -1;

        private int id;
        private float x;
        private float y;
        private float previousX;
        private float previousY;
        private float pressure;
        private float screenX;
        private float screenY;
        private object platformSpecificTouch;

        /// <summary>
        /// Constructor
        /// </summary>
        public GestureTouch(int platformSpecificId, float x, float y, float previousX, float previousY, float pressure)
        {
            this.id = platformSpecificId;
            this.x = x;
            this.y = y;
            this.previousX = previousX;
            this.previousY = previousY;
            this.pressure = pressure;
            screenX = float.NaN;
            screenY = float.NaN;
            platformSpecificTouch = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GestureTouch(int platformSpecificId, float x, float y, float previousX, float previousY, float pressure, float screenX, float screenY)
        {
            this.id = platformSpecificId;
            this.x = x;
            this.y = y;
            this.previousX = previousX;
            this.previousY = previousY;
            this.pressure = pressure;
            this.screenX = screenX;
            this.screenY = screenY;
            platformSpecificTouch = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GestureTouch(int platformSpecificId, float x, float y, float previousX, float previousY, float pressure, float screenX, float screenY, object platformSpecificTouch)
        {
            this.id = platformSpecificId;
            this.x = x;
            this.y = y;
            this.previousX = previousX;
            this.previousY = previousY;
            this.pressure = pressure;
            this.screenX = screenX;
            this.screenY = screenY;
            this.platformSpecificTouch = platformSpecificTouch;
        }

        /// <summary>
        /// Sets the Id to GestureTouch.PlatformSpecificIdInvalid;
        /// </summary>
        public void Invalidate()
        {
            this.id = GestureTouch.PlatformSpecificIdInvalid;
        }

        /// <summary>
        /// Determines whether this instance is valid
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid()
        {
            return (Id != PlatformSpecificIdInvalid);
        }

        public int CompareTo(GestureTouch other)
        {
            return this.id.CompareTo(other.id);
        }

        /// <summary>
        /// Returns a hash code for this GestureTouch
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>
        /// Checks if this GestureTouch equals another GestureTouch
        /// </summary>
        /// <param name="obj">The object to compare against</param>
        /// <returns>True if equal to obj, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is GestureTouch)
            {
                return ((GestureTouch)obj).Id == Id;
            }
            return false;
        }

        /// <summary>
        /// Invalidates this gesture touch object
        /// </summary>
        public void Dispose()
        {
            Invalidate();
        }

        /// <summary>
        /// Unique id for the touch
        /// </summary>
        /// <value>The platform specific identifier</value>
        public int Id { get { return id; } }

        /// <summary>
        /// X value
        /// </summary>
        /// <value>The x value</value>
        public float X { get { return x; } }

        /// <summary>
        /// Y value
        /// </summary>
        /// <value>The y value</value>
        public float Y { get { return y; } }

        /// <summary>
        /// Previous x value
        /// </summary>
        /// <value>The previous x value</value>
        public float PreviousX { get { return previousX; } }

        /// <summary>
        /// Previous Y value
        /// </summary>
        /// <value>The previous y value</value>
        public float PreviousY { get { return previousY; } }

        /// <summary>
        /// Screen x coordinate, NAN if unknown
        /// </summary>
        /// <value>The screen x coordinate.</value>
        public float ScreenX { get { return screenX; } }

        /// <summary>
        /// Screen y coordinate, NAN if unknown
        /// </summary>
        /// <value>The screen y coordinate.</value>
        public float ScreenY { get { return screenY; } }

        /// <summary>
        /// Pressure, 0 if unknown
        /// </summary>
        /// <value>The pressure of the touch</value>
        public float Pressure { get { return pressure; } }

        /// <summary>
        /// Change in x value
        /// </summary>
        /// <value>The delta x</value>
        public float DeltaX { get { return x - previousX; } }

        /// <summary>
        /// Change in y value
        /// </summary>
        /// <value>The delta y</value>
        public float DeltaY { get { return y - previousY; } }

        /// <summary>
        /// Platform specific touch information (null if none)
        /// </summary>
        public object PlatformSpecificTouch { get { return platformSpecificTouch; } }
    }

    /// <summary>
    /// Gesture recognizer updated
    /// </summary>
    public delegate void GestureRecognizerUpdated(GestureRecognizer gesture, ICollection<GestureTouch> touches);

    /// <summary>
    /// Tracks and calculates velocity for gestures
    /// </summary>
    public class GestureVelocityTracker
    {
        private struct VelocityHistory
        {
            public float VelocityX;
            public float VelocityY;
            public float Seconds;
        }

        private const int maxHistory = 8;

        private readonly System.Collections.Generic.Queue<VelocityHistory> history = new System.Collections.Generic.Queue<VelocityHistory>();
        private readonly System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
        private float previousX;
        private float previousY;

        private void AddItem(float velocityX, float velocityY, float elapsed)
        {
            VelocityHistory item = new VelocityHistory
            {
                VelocityX = velocityX,
                VelocityY = velocityY,
                Seconds = elapsed
            };
            history.Enqueue(item);
            if (history.Count > maxHistory)
            {
                history.Dequeue();
            }
            float totalSeconds = 0.0f;
            VelocityX = VelocityY = 0.0f;
            foreach (VelocityHistory h in history)
            {
                totalSeconds += h.Seconds;
            }
            foreach (VelocityHistory h in history)
            {
                float weight = h.Seconds / totalSeconds;
                VelocityX += (h.VelocityX * weight);
                VelocityY += (h.VelocityY * weight);
            }
            timer.Reset();
            timer.Start();
        }

        public void Reset()
        {
            timer.Reset();
            VelocityX = VelocityY = 0.0f;
            history.Clear();
        }

        public void Restart()
        {
            Restart(float.MinValue, float.MinValue);
        }

        public void Restart(float previousX, float previousY)
        {
            this.previousX = previousX;
            this.previousY = previousY;
            Reset();
            timer.Start();
        }

        public void Update(float x, float y)
        {
            float elapsed = ElapsedSeconds;
            if (previousX != float.MinValue)
            {
                float px = previousX;
                float py = previousY;
                float velocityX = (x - px) / elapsed;
                float velocityY = (y - py) / elapsed;
                AddItem(velocityX, velocityY, elapsed);
            }
            previousX = x;
            previousY = y;
        }

        public float ElapsedSeconds { get { return (float)timer.Elapsed.TotalSeconds; } }
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float Speed { get { return (float)Math.Sqrt(VelocityX * VelocityX + VelocityY * VelocityY); } }
    }

    /// <summary>
    /// A gesture recognizer allows handling gestures as well as ensuring that different gestures
    /// do not execute at the same time. Platform specific code is required to create GestureTouch
    /// sets and pass them to the appropriate gesture recognizer(s). Creating extension methods
    /// on the GestureRecognizer class is a good way.
    /// </summary>
    public class GestureRecognizer : IDisposable
    {
        private static readonly GestureRecognizer allGesturesReference = new GestureRecognizer();
        private GestureRecognizerState state = GestureRecognizerState.Possible;
        private readonly List<GestureTouch> currentTrackedTouches = new List<GestureTouch>();
        private readonly System.Collections.ObjectModel.ReadOnlyCollection<GestureTouch> currentTrackedTouchesReadOnly;
        private GestureRecognizer requireGestureRecognizerToFail;
        private readonly HashSet<GestureRecognizer> failGestures = new HashSet<GestureRecognizer>();
        private readonly List<GestureRecognizer> simultaneousGestures = new List<GestureRecognizer>();
        private readonly GestureVelocityTracker velocityTracker = new GestureVelocityTracker();
        private bool touchesJustEnded;

        private int minimumNumberOfTouchesToTrack = 1;
        private int maximumNumberOfTouchesToTrack = 1;
        protected float prevFocusX { get; private set; }
        protected float prevFocusY { get; private set; }

        public static readonly HashSet<GestureRecognizer> ActiveGestures = new HashSet<GestureRecognizer>();

        private void FailGestureNow()
        {
            state = GestureRecognizerState.Failed;
            ActiveGestures.Remove(this);
            StateChanged();
            foreach (GestureRecognizer r in failGestures)
            {
                if (r.state == GestureRecognizerState.EndPending)
                {
                    r.SetState(GestureRecognizerState.Ended);
                }
            }
            Reset();
        }

        private bool TouchesIntersect(IEnumerable<GestureTouch> collection, List<GestureTouch> list)
        {
            foreach (GestureTouch t in collection)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Id == t.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void UpdateTrackedTouches(IEnumerable<GestureTouch> touches)
        {
            int count = 0;
            foreach (GestureTouch touch in touches)
            {
                for (int i = 0; i < currentTrackedTouches.Count; i++)
                {
                    if (currentTrackedTouches[i].Id == touch.Id)
                    {
                        currentTrackedTouches[i] = touch;
                        count++;
                        break;
                    }
                }
            }
            if (count != 0)
            {
                currentTrackedTouches.Sort();
            }
        }

        private int TrackTouchesInternal(IEnumerable<GestureTouch> touches)
        {
            int count = 0;
            foreach (GestureTouch touch in touches)
            {
                if (currentTrackedTouches.Count < MaximumNumberOfTouchesToTrack && !currentTrackedTouches.Contains(touch))
                {
                    currentTrackedTouches.Add(touch);
                    count++;
                }
            }
            if (count != 0)
            {
                currentTrackedTouches.Sort();
            }
            return count;
        }

        private

#if PCL || PORTABLE || HAS_TASKS

        async

#endif

        static void RunActionAfterDelayInternal(float seconds, Action action)
        {
            if (action == null)
            {
                return;
            }

#if PCL || PORTABLE || HAS_TASKS

            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(seconds));

            action();

#else

            MainThreadCallback(seconds, action);

#endif

        }

        protected System.Collections.ObjectModel.ReadOnlyCollection<GestureTouch> CurrentTrackedTouches { get { return currentTrackedTouchesReadOnly; } }

        /// <summary>
        /// Calculate the focus of the gesture
        /// </summary>
        /// <param name="touches">Touches</param>
        /// <returns>True if this was the first focus calculation, false otherwise</returns>
        protected bool CalculateFocus(ICollection<GestureTouch> touches)
        {
            return CalculateFocus(touches, false);
        }

        /// <summary>
        /// Calculate the focus of the gesture
        /// </summary>
        /// <param name="touches">Touches</param>
        /// <param name="resetFocus">True to force reset of the start focus, false otherwise</param>
        /// <returns>True if this was the first focus calculation, false otherwise</returns>
        protected bool CalculateFocus(ICollection<GestureTouch> touches, bool resetFocus)
        {
            bool first = resetFocus || (StartFocusX == float.MinValue || StartFocusY == float.MinValue);

            prevFocusX = FocusX;
            prevFocusY = FocusY;

            FocusX = 0.0f;
            FocusY = 0.0f;

            foreach (GestureTouch t in touches)
            {
                FocusX += t.X;
                FocusY += t.Y;
            }

            FocusX /= (float)touches.Count;
            FocusY /= (float)touches.Count;

            if (first)
            {
                StartFocusX = FocusX;
                StartFocusY = FocusY;
                DeltaX = 0.0f;
                DeltaY = 0.0f;
                velocityTracker.Restart();
            }
            else
            {
                DeltaX = FocusX - prevFocusX;
                DeltaY = FocusY - prevFocusY;
            }

            velocityTracker.Update(FocusX, FocusY);

            DistanceX = FocusX - StartFocusX;
            DistanceY = FocusY - StartFocusY;

            return first;
        }

        /// <summary>
        /// Called when state changes
        /// </summary>
        protected virtual void StateChanged()
        {
            if (Updated != null)
            {
                Updated(this, currentTrackedTouches);
            }

            if (failGestures.Count != 0 && (state == GestureRecognizerState.Began || state == GestureRecognizerState.Executing ||
                state == GestureRecognizerState.Ended))
            {
                foreach (GestureRecognizer r in failGestures)
                {
                    r.FailGestureNow();
                }
            }
        }

        /// <summary>
        /// Sets the state of the gesture. Continous gestures should set the executing state every time they change.
        /// </summary>
        /// <param name="value">True if success, false if the gesture was forced to fail or the state is pending a require gesture recognizer to fail state change</param>
        protected bool SetState(GestureRecognizerState value)
        {
            if (value == GestureRecognizerState.Failed)
            {
                FailGestureNow();
                return true;
            }
            // if we are trying to execute from a non-executing state and there are gestures already executing,
            // we need to make sure we are allowed to execute simultaneously
            else if (!AllowSimultaneousExecutionWithAllGestures && ActiveGestures.Count != 0 &&
            (
                value == GestureRecognizerState.Began ||
                value == GestureRecognizerState.Executing ||
                value == GestureRecognizerState.Ended
            ) && state != GestureRecognizerState.Began && state != GestureRecognizerState.Executing)
            {
                // check all the active gestures and if any are not allowed to simultaneously
                // execute with this gesture, fail this gesture immediately
                foreach (GestureRecognizer r in ActiveGestures)
                {
                    if (r != this &&
                        !simultaneousGestures.Contains(r) &&
                        !r.simultaneousGestures.Contains(this) &&
                        !simultaneousGestures.Contains(allGesturesReference) &&
                        !r.simultaneousGestures.Contains(allGesturesReference))
                    {
                        FailGestureNow();
                        return false;
                    }
                }
            }

            if (requireGestureRecognizerToFail != null && value == GestureRecognizerState.Ended &&
                requireGestureRecognizerToFail.state != GestureRecognizerState.Failed &&
                (requireGestureRecognizerToFail.currentTrackedTouches.Count != 0 || requireGestureRecognizerToFail.touchesJustEnded))
            {
                // the other gesture will end the state when it fails, or fail this gesture if it executes
                state = GestureRecognizerState.EndPending;
                return false;
            }
            else
            {
                state = value;
                if (state == GestureRecognizerState.Began || state == GestureRecognizerState.Executing)
                {
                    ActiveGestures.Add(this);
                }
                else if (state == GestureRecognizerState.Ended)
                {
                    // a tap gesture for example needs to be active for one loop so that other tap gestures do not fire at the same time
                    ActiveGestures.Add(this);
                    if (ResetOnEnd)
                    {
                        RunActionAfterDelay(0.01f, Reset);
                    }
                    else
                    {
                        StateChanged();
                        StopTrackingAllTouches();
                        SetState(GestureRecognizerState.Possible);
                    }
                }
                StateChanged();
            }

            return true;
        }

        /// <summary>
        /// Call with the touches that began, child class should override
        /// </summary>
        /// <param name="touches">Touches that began</param>
        protected virtual void TouchesBegan(IEnumerable<GestureTouch> touches)
        {

        }

        /// <summary>
        /// Call with the touches that moved, child class should override
        /// </summary>
        /// <param name="touches">Touches that moved</param>
        protected virtual void TouchesMoved()
        {

        }

        /// <summary>
        /// Call with the touches that ended, child class should override
        /// </summary>
        /// <param name="touches">Touches that ended</param>
        protected virtual void TouchesEnded()
        {

        }

        /// <summary>
        /// Begin tracking the specified touch ids
        /// </summary>
        /// <param name="touchIds">Touch ids to track</param>
        /// <returns>The number of tracked touches</returns>
        protected int TrackTouches(IEnumerable<GestureTouch> touches)
        {
            return TrackTouchesInternal(touches);
        }

        /// <summary>
        /// Stops tracking all currently tracked touches
        /// </summary>
        /// <returns>The number of touches that stopped tracking</returns>
        protected int StopTrackingAllTouches()
        {
            int count = currentTrackedTouches.Count;
            currentTrackedTouches.Clear();
            return count;
        }

        /// <summary>
        /// Stops tracking the specified touch ids
        /// </summary>
        /// <param name="touches">Touches to stop tracking</param>
        /// <returns>The number of touches that stopped tracking</returns>
        protected int StopTrackingTouches(ICollection<GestureTouch> touches)
        {
            int[] touchIds = new int[touches.Count];
            int index = 0;
            foreach (GestureTouch t in touches)
            {
                touchIds[index++] = t.Id;
            }
            return StopTrackingTouches(touchIds);
        }

        /// <summary>
        /// Stops tracking the specified touch ids
        /// </summary>
        /// <param name="touchIds">Touch ids to stop tracking</param>
        /// <returns>The number of touches that stopped tracking</returns>
        protected int StopTrackingTouches(params int[] touchIds)
        {
            int count = 0;
            foreach (int t in touchIds)
            {
                for (int i = 0; i < currentTrackedTouches.Count; i++)
                {
                    if (currentTrackedTouches[i].Id == t)
                    {
                        currentTrackedTouches.RemoveAt(i);
                        count++;
                        break;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GestureRecognizer()
        {
            state = GestureRecognizerState.Possible;
            PlatformSpecificViewScale = 1.0f;
            StartFocusX = StartFocusY = float.MinValue;
            currentTrackedTouchesReadOnly = new System.Collections.ObjectModel.ReadOnlyCollection<GestureTouch>(currentTrackedTouches);
        }

        /// <summary>
        /// Simulate a gesture
        /// </summary>
        /// <param name="xy">List of xy coordinates, repeating</param>
        /// <returns>True if success, false if xy array invalid</returns>
        public bool Simulate(params float[] xy)
        {
            if (xy == null || xy.Length < 2 || xy.Length % 2 != 0)
            {
                return false;
            }
            else if (xy.Length > 3)
            {
                ProcessTouchesBegan(new GestureTouch[] { new GestureTouch(0, xy[2], xy[3], xy[0], xy[1], 1.0f) });
            }
            else
            {
                ProcessTouchesBegan(new GestureTouch[] { new GestureTouch(0, xy[0], xy[1], xy[0], xy[1], 1.0f) });                
            }

            for (int i = 2; i < xy.Length - 2; i += 2)
            {
                ProcessTouchesMoved(new GestureTouch[] { new GestureTouch(0, xy[i - 2], xy[i - 1], xy[i], xy[i + 1], 1.0f) });
            }

            if (xy.Length > 3)
            {
                ProcessTouchesEnded(new GestureTouch[] { new GestureTouch(0, xy[xy.Length - 2], xy[xy.Length - 1], xy[xy.Length - 4], xy[xy.Length - 3], 1.0f) });
            }
            else
            {
                ProcessTouchesEnded(new GestureTouch[] { new GestureTouch(0, xy[xy.Length - 2], xy[xy.Length - 1], xy[xy.Length - 2], xy[xy.Length - 1], 1.0f) });
            }

            return true;
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~GestureRecognizer()
        {
            Dispose();
        }

        /// <summary>
        /// Reset all internal state  for the gesture recognizer
        /// </summary>
        public virtual void Reset()
        {
            currentTrackedTouches.Clear();
            StartFocusX = prevFocusX = StartFocusY = prevFocusY = float.MinValue;
            FocusX = FocusY = DeltaX = DeltaY = DistanceX = DistanceY = 0.0f;
            velocityTracker.Reset();
            ActiveGestures.Remove(this);
            SetState(GestureRecognizerState.Possible);
        }

        /// <summary>
        /// Call with the touches that began
        /// </summary>
        /// <param name="touches">Touches that began</param>
        public void ProcessTouchesBegan(ICollection<GestureTouch> touches)
        {
            touchesJustEnded = false;
            if (touches == null || touches.Count == 0)
            {
                return;
            }
            UpdateTrackedTouches(touches);
            TouchesBegan(touches);
        }

        /// <summary>
        /// Call with the touches that moved
        /// </summary>
        /// <param name="touches">Touches that moved</param>
        public void ProcessTouchesMoved(ICollection<GestureTouch> touches)
        {
            if (touches == null || touches.Count == 0 || !TouchesIntersect(touches, currentTrackedTouches))
            {
                return;
            }
            UpdateTrackedTouches(touches);
            TouchesMoved();
        }

        /// <summary>
        /// Call with the touches that ended
        /// </summary>
        /// <param name="touches">Touches that ended</param>
        public void ProcessTouchesEnded(ICollection<GestureTouch> touches)
        {
            if (touches == null || touches.Count == 0 || !TouchesIntersect(touches, currentTrackedTouches))
            {
                return;
            }
            UpdateTrackedTouches(touches);
            TouchesEnded();
            touchesJustEnded = true;
        }

        public void ProcessTouchesCancelled(ICollection<GestureTouch> touches)
        {
            if (touches != null && touches.Count != 0)
            {
                foreach (GestureTouch t in touches)
                {
                    if (currentTrackedTouches.Contains(t))
                    {
                        SetState(GestureRecognizerState.Failed);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether two points are within a specified distance
        /// </summary>
        /// <returns>True if within distance false otherwise</returns>
        /// <param name="x1">The first x value in pixels.</param>
        /// <param name="y1">The first y value in pixels.</param>
        /// <param name="x2">The second x value in pixels.</param>
        /// <param name="y2">The second y value in pixels.</param>
        /// <param name="d">Distance in units</param>
        public bool PointsAreWithinDistance(float x1, float y1, float x2, float y2, float d)
        {
            return (DistanceBetweenPoints(x1, y1, x2, y2) <= d);
        }

        /// <summary>
        /// Gets the distance between two points, in units
        /// </summary>
        /// <returns>The distance between the two points in units.</returns>
        /// <param name="x1">The first x value in pixels.</param>
        /// <param name="y1">The first y value in pixels.</param>
        /// <param name="x2">The second x value in pixels.</param>
        /// <param name="y2">The second y value in pixels.</param>
        public float DistanceBetweenPoints(float x1, float y1, float x2, float y2)
        {
            float a = (float)(x2 - x1);
            float b = (float)(y2 - y1);

            return ((float)Math.Sqrt(a * a + b * b) * PlatformSpecificViewScale) / DeviceInfo.UnitMultiplier;
        }

        /// <summary>
        /// Gets the distance of a vector, in units
        /// </summary>
        /// <param name="xVector">X vector</param>
        /// <param name="yVector">Y vector</param>
        /// <returns>The distance of the vector in units.</returns>
        public float Distance(float xVector, float yVector)
        {
            return ((float)Math.Sqrt(xVector * xVector + yVector * yVector) * PlatformSpecificViewScale) / DeviceInfo.UnitMultiplier;
        }

        /// <summary>
        /// Dispose of the gesture and ensure it is removed from the global list of active gestures
        /// </summary>
        public virtual void Dispose()
        {
            ActiveGestures.Remove(this);
            foreach (GestureRecognizer r in simultaneousGestures.ToArray())
            {
                DisallowSimultaneousExecution(r);
            }
            foreach (GestureRecognizer r in failGestures)
            {
                if (r.requireGestureRecognizerToFail == this)
                {
                    r.requireGestureRecognizerToFail = null;
                }
            }
        }

        /// <summary>
        /// Allows the simultaneous execution with other gesture. This links both gestures so this method
        /// only needs to be called once on one of the gestures.
        /// Pass null to allow simultaneous execution with all gestures.
        /// </summary>
        /// <param name="other">Gesture to execute simultaneously with</param>
        public void AllowSimultaneousExecution(GestureRecognizer other)
        {
            other = (other ?? allGesturesReference);
            simultaneousGestures.Add(other);
            if (other != allGesturesReference)
            {
                other.simultaneousGestures.Add(this);
            }
        }

        /// <summary>
        /// Disallows the simultaneous execution with other gesture. This unlinks both gestures so this method
        /// only needs to be called once on one of the gestures.
        /// By default, gesures are not allowed to execute simultaneously, so you only need to call this method
        /// if you previously allowed the gestures to execute simultaneously.
        /// Pass null to disallow simulatneous execution with all gestures (i.e. you previously called
        /// AllowSimultaneousExecution with a null value.
        /// </summary>
        /// <param name="other">Gesture to no longer execute simultaneously with</param>
        public void DisallowSimultaneousExecution(GestureRecognizer other)
        {
            other = (other ?? allGesturesReference);
            simultaneousGestures.Remove(other);
            if (other != allGesturesReference)
            {
                other.simultaneousGestures.Remove(this);
            }
        }

        /// <summary>
        /// Run an action on the main thread after a delay
        /// </summary>
        /// <param name="seconds">Delay in seconds</param>
        /// <param name="action">Action to run</param>
        public static void RunActionAfterDelay(float seconds, Action action)
        {
            RunActionAfterDelayInternal(seconds, action);
        }

        /// <summary>
        /// The global total number of gestures in progress
        /// </summary>
        /// <returns>Number of gestures in progress</returns>
        public static int NumberOfGesturesInProgress()
        {
            return ActiveGestures.Count;
        }

        /// <summary>
        /// Get the current gesture recognizer state
        /// </summary>
        /// <value>Gesture recognizer state</value>
        public GestureRecognizerState State { get { return state; } }

        /// <summary>
        /// Executes when the gesture changes
        /// </summary>
        public event GestureRecognizerUpdated Updated;

        /// <summary>
        /// Change in x in pixels
        /// </summary>
        /// <value>The change in x</value>
        public float DeltaX { get; private set; }

        /// <summary>
        /// Change in y in pixels
        /// </summary>
        /// <value>The change in y</value>
        public float DeltaY { get; private set; }

        /// <summary>
        /// Focus x value in pixels (average of all touches)
        /// </summary>
        /// <value>The focus x.</value>
        public float FocusX { get; private set; }

        /// <summary>
        /// Focus y value in pixels (average of all touches)
        /// </summary>
        /// <value>The focus y.</value>
        public float FocusY { get; private set; }

        /// <summary>
        /// Start focus x value in pixels (average of all touches)
        /// </summary>
        /// <value>The focus x.</value>
        public float StartFocusX { get; private set; }

        /// <summary>
        /// Start focus y value in pixels (average of all touches)
        /// </summary>
        /// <value>The focus y.</value>
        public float StartFocusY { get; private set; }

        /// <summary>
        /// The distance (in pixels) the gesture has moved from where it started along the x axis
        /// </summary>
        public float DistanceX { get; private set; }

        /// <summary>
        /// The distance (in pixels) the gesture has moved from where it started along the y axis
        /// </summary>
        public float DistanceY { get; private set; }

        /// <summary>
        /// Velocity x in pixels
        /// </summary>
        /// <value>The velocity x value in pixels</value>
        public float VelocityX { get { return velocityTracker.VelocityX; } }

        /// <summary>
        /// Velocity y in pixels
        /// </summary>
        /// <value>The velocity y value in pixels</value>
        public float VelocityY { get { return velocityTracker.VelocityY; } }

        /// <summary>
        /// The speed of the gesture in pixels
        /// </summary>
        /// <value>The speed of the gesture</value>
        public float Speed { get { return velocityTracker.Speed; } }

        /// <summary>
        /// A platform specific view object that this gesture can execute in, null if none
        /// </summary>
        /// <value>The platform specific view this gesture can execute in</value>
        public object PlatformSpecificView { get; set; }

        /// <summary>
        /// The platform specific view scale (default is 1.0). Change this if the view this gesture is attached to is being scaled.
        /// </summary>
        /// <value>The platform specific view scale</value>
        public float PlatformSpecificViewScale { get; set; }

        /// <summary>
        /// If this gesture reaches the EndPending state and the specified gesture fails,
        /// this gesture will end. If the specified gesture begins, executes or ends,
        /// then this gesture will immediately fail.
        /// </summary>
        public GestureRecognizer RequireGestureRecognizerToFail
        {
            get { return requireGestureRecognizerToFail; }
            set
            {
                if (value != requireGestureRecognizerToFail)
                {
                    if (requireGestureRecognizerToFail != null)
                    {
                        requireGestureRecognizerToFail.failGestures.Remove(this);
                    }
                    requireGestureRecognizerToFail = value;
                    if (requireGestureRecognizerToFail != null)
                    {
                        requireGestureRecognizerToFail.failGestures.Add(this);
                    }
                }
            }
        }

        /// <summary>
        /// The minimum number of touches to track. This gesture will not start unless this many touches are tracked. Default is usually 1 or 2.
        /// Not all gestures will honor values higher than 1.
        /// </summary>
        public int MinimumNumberOfTouchesToTrack
        {
            get { return minimumNumberOfTouchesToTrack; }
            set
            {
                minimumNumberOfTouchesToTrack = value;
                if (minimumNumberOfTouchesToTrack > maximumNumberOfTouchesToTrack)
                {
                    maximumNumberOfTouchesToTrack = minimumNumberOfTouchesToTrack;
                }
            }
        }

        /// <summary>
        /// The maximum number of touches to track. This gesture will never track more touches than this. Default is usually 1 or 2.
        /// Not all gestures will honor values higher than 1.
        /// </summary>
        public int MaximumNumberOfTouchesToTrack
        {
            get { return maximumNumberOfTouchesToTrack; }
            set
            {
                maximumNumberOfTouchesToTrack = value;
                if (maximumNumberOfTouchesToTrack < minimumNumberOfTouchesToTrack)
                {
                    minimumNumberOfTouchesToTrack = maximumNumberOfTouchesToTrack;
                }
            }
        }

        /// <summary>
        /// Whether to allow simultaneous execution with all gestures - default is false
        /// </summary>
        public bool AllowSimultaneousExecutionWithAllGestures { get; set; }

        /// <summary>
        /// Whether the gesture should reset when it ends
        /// </summary>
        public virtual bool ResetOnEnd { get { return true; } }

#if !PCL && !HAS_TASKS

        public delegate void CallbackMainThreadDelegate(float delay, Action callback);

        public static CallbackMainThreadDelegate MainThreadCallback;

#endif

    }
}

