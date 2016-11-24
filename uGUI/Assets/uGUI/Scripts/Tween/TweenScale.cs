using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace uTools {
		
	public class TweenScale : Tween<Vector3> {

        private Vector3 mValue;

        Transform mTransform;

        Transform cachedTransform
        {
            get
            {
                if (mTransform == null)
                {
                    mTransform = transform;
                }
                return mTransform;
            }
        }

        public override Vector3 value
        {
            get { return mValue; }
            set
            {
                mValue = value;
                cachedTransform.localScale = value;
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = from + factor * (to - from);
        }

        public static TweenScale Begin(GameObject go, Vector3 from, Vector3 to, float duration = 1f, float delay = 0f)
        {
            TweenScale comp = Tweener.Begin<TweenScale>(go, duration);
            comp.value = from;
            comp.from = from;
            comp.to = to;
            comp.duration = duration;
            comp.delay = delay;
            if (duration <= 0)
            {
                comp.Sample(1, true);
                comp.enabled = false;
            }
            return comp;
        }
    }
}
