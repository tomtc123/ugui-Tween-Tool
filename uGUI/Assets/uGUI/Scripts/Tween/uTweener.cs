using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Base of tween.
/// </summary>

namespace uTools
{
    public abstract class uTweener : MonoBehaviour
    {
        static public uTweener current;

        public enum Style
        {
            Once,
            Loop,
            PingPong,
        }

        public EaseType method = EaseType.linear;
        public Style style = Style.Once;
        public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
        public bool ignoreTimeScale = true;
        public float delay = 0f;
        public float duration = 1f;

        [HideInInspector]
        public bool steeperCurves = false;

        public int tweenGroup = 0;
        public UnityEvent onFinished = null;
        public UnityAction onValueChange = null;

        [HideInInspector]
        public GameObject eventReceiver;
		[HideInInspector]
        public string callWhenFinished;

        bool mStarted = false;
        float mStartTime = 0f;
        float mDuration = 0f;
        float mAmountPerDelta = 1000f;
        float mFactor = 0f;

        /// <summary>
        /// Amount advanced per delta time.
        /// </summary>

        public float amountPerDelta
        {
            get
            {
                if (mDuration != duration)
                {
                    mDuration = duration;
                    mAmountPerDelta = Mathf.Abs((duration > 0f) ? 1f / duration : 1000f) * Mathf.Sign(mAmountPerDelta);
                }
                return mAmountPerDelta;
            }
        }

        public float tweenFactor { get { return mFactor; } set { mFactor = Mathf.Clamp01(value); } }

        public Direction direction { get { return amountPerDelta < 0f ? Direction.Reverse : Direction.Forward; } }

        void Reset()
        {
            if (!mStarted)
            {
                SetStartToCurrentValue();
                SetEndToCurrentValue();
            }
        }

        protected virtual void Start() { Update(); }

        void Update()
        {
            float delta = ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            float time = ignoreTimeScale ? Time.unscaledTime : Time.time;

            if (!mStarted)
            {
                mStarted = true;
                mStartTime = time + delay;
            }

            if (time < mStartTime) return;

            // Advance the sampling factor
            mFactor += amountPerDelta * delta;

            // Loop style simply resets the play factor after it exceeds 1.
            if (style == Style.Loop)
            {
                if (mFactor > 1f)
                {
                    mFactor -= Mathf.Floor(mFactor);
                }
            }
            else if (style == Style.PingPong)
            {
                // Ping-pong style reverses the direction
                if (mFactor > 1f)
                {
                    mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
                    mAmountPerDelta = -mAmountPerDelta;
                }
                else if (mFactor < 0f)
                {
                    mFactor = -mFactor;
                    mFactor -= Mathf.Floor(mFactor);
                    mAmountPerDelta = -mAmountPerDelta;
                }
            }

            // If the factor goes out of range and this is a one-time tweening operation, disable the script
            if ((style == Style.Once) && (duration == 0f || mFactor > 1f || mFactor < 0f))
            {
                mFactor = Mathf.Clamp01(mFactor);
                Sample(mFactor, true);
                enabled = false;

                if (current != this)
                {
                    uTweener before = current;
                    current = this;

                    if (onFinished != null)
                    {
                        onFinished.Invoke();
                    }

                    // Deprecated legacy functionality support
                    if (eventReceiver != null && !string.IsNullOrEmpty(callWhenFinished))
                        eventReceiver.SendMessage(callWhenFinished, this, SendMessageOptions.DontRequireReceiver);

                    current = before;
                }
            }
            else Sample(mFactor, false);
        }

        public void SetOnFinished(UnityEvent finishedCallBack) { onFinished = finishedCallBack; }

        public void AddOnFinished(UnityEvent finishedCallBack) {  }

        public void RemoveOnFinished(UnityEvent finishedCallBack)
        {
           
        }

        void OnDisable() { mStarted = false; }

        /// <summary>
        /// Sample the tween at the specified factor.
        /// </summary>

        public void Sample(float factor, bool isFinished)
        {
            // Calculate the sampling value
            float val = Mathf.Clamp01(factor);

            val = (method == EaseType.none) ? animationCurve.Evaluate(val) : EaseManager.EasingFromType(0, 1, val, method);

            // Call the virtual update
            OnUpdate((method == EaseType.none) ? animationCurve.Evaluate(val) : val, isFinished);
            if (onValueChange != null)
            {
                onValueChange.Invoke();
            }
        }

        /// <summary>
        /// Play the tween forward.
        /// </summary>

        public void PlayForward() { Play(true); }

        /// <summary>
        /// Play the tween in reverse.
        /// </summary>

        public void PlayReverse() { Play(false); }

        /// <summary>
        /// Manually activate the tweening process, reversing it if necessary.
        /// </summary>

        public void Play(bool forward)
        {
            mAmountPerDelta = Mathf.Abs(amountPerDelta);
            if (!forward) mAmountPerDelta = -mAmountPerDelta;
            enabled = true;
            Update();
        }

        /// <summary>
        /// Manually reset the tweener's state to the beginning.
        /// If the tween is playing forward, this means the tween's start.
        /// If the tween is playing in reverse, this means the tween's end.
        /// </summary>

        public void ResetToBeginning()
        {
            mStarted = false;
            mFactor = (amountPerDelta < 0f) ? 1f : 0f;
            Sample(mFactor, false);
        }

        /// <summary>
        /// Manually start the tweening process, reversing its direction.
        /// </summary>

        public void Toggle()
        {
            if (mFactor > 0f)
            {
                mAmountPerDelta = -amountPerDelta;
            }
            else
            {
                mAmountPerDelta = Mathf.Abs(amountPerDelta);
            }
            enabled = true;
        }

        /// <summary>
        /// Actual tweening logic should go here.
        /// </summary>

        abstract protected void OnUpdate(float factor, bool isFinished);

        /// <summary>
        /// Starts the tweening operation.
        /// </summary>

        static public T Begin<T>(GameObject go, float duration) where T : uTweener
        {
            T comp = go.GetComponent<T>();
#if UNITY_FLASH
		if ((object)comp == null) comp = (T)go.AddComponent<T>();
#else
            // Find the tween with an unset group ID (group ID of 0).
            if (comp != null && comp.tweenGroup != 0)
            {
                comp = null;
                T[] comps = go.GetComponents<T>();
                for (int i = 0, imax = comps.Length; i < imax; ++i)
                {
                    comp = comps[i];
                    if (comp != null && comp.tweenGroup == 0) break;
                    comp = null;
                }
            }

            if (comp == null)
            {
                comp = go.AddComponent<T>();

                if (comp == null)
                {
                    Debug.LogError("Unable to add " + typeof(T) + " to " + go);
                    return null;
                }
            }
#endif
            comp.mStarted = false;
            comp.duration = duration;
            comp.mFactor = 0f;
            comp.mAmountPerDelta = Mathf.Abs(comp.amountPerDelta);
            comp.style = Style.Once;
            comp.animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
            comp.eventReceiver = null;
            comp.callWhenFinished = null;
            comp.enabled = true;
            return comp;
        }
        
        public virtual void SetStartToCurrentValue() { }
        public virtual void SetEndToCurrentValue() { }
        public virtual void SetCurrentValueToStart() { }
        public virtual void SetCurrentValueToEnd() { }
    }

}