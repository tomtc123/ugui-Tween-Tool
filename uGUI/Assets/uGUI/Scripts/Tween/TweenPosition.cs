using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	
	public class TweenPosition : Tween<Vector3> {

        RectTransform mRectTransform = null;
        Transform mTransform = null;

        bool mIs3D = true;
        private bool is3D
        {
            get
            {
                if (mTransform == null)
                {
                    mTransform = transform;
                    RectTransform rect = cachedTransform as RectTransform;
                    mIs3D = (rect != null) ? false : true;
                }
                return mIs3D;
            }
            set
            {
                mIs3D = value;
            }
        }

        Transform cachedTransform
        {
            get
            {
                if (mTransform == null)
                {
                    mTransform = transform;
                    RectTransform rect = cachedTransform as RectTransform;
                    is3D = (rect != null) ? false : true;
                }
                return mTransform;
            }
        }


        RectTransform cachedRectTransform
        {
            get
            {
                if (mRectTransform == null)
                {
                    mRectTransform = cachedTransform as RectTransform;
                    is3D = (mRectTransform != null) ? false : true;
                }
                return mRectTransform;
            }
        }

        public override Vector3 value
        {
            get
            {
                if (is3D)
                {
                    return cachedTransform.localPosition;
                }
                else
                {
                    return cachedRectTransform.anchoredPosition;
                }
            }
            set
            {
                if (is3D)
                {
                    cachedTransform.localPosition = value;
                }
                else
                {
                    cachedRectTransform.anchoredPosition = value;
                }
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = from + factor * (to - from);
        }

        public static TweenPosition Begin(GameObject go, Vector3 from, Vector3 to, float duration = 1f, float delay = 0f)
        {
            TweenPosition comp = Tweener.Begin<TweenPosition>(go, duration);
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
