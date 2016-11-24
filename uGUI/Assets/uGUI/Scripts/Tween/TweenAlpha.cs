using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	
	public class TweenAlpha : Tween<float> {

        public bool includeChildren = false;
        private bool isCanvasGroup = false;
        float mAlpha = 0f;

        Transform mTransform;
        Transform CachedTranform
        {
            get
            {
                if (mTransform == null)
                {
                    mTransform = GetComponent<Transform>();
                }
                return mTransform;
            }
        }

        Graphic[] mGraphics;
        Graphic[] CachedGraphics
        {
            get
            {
                if (mGraphics == null)
                {
                    mGraphics = includeChildren ? gameObject.GetComponentsInChildren<Graphic>() : gameObject.GetComponents<Graphic>();
                }
                return mGraphics;
            }
        }

        CanvasGroup mCanvasGroup;
        CanvasGroup CacheCanvasGroup
        {
            get
            {
                if (mCanvasGroup == null)
                {
                    mCanvasGroup = gameObject.GetComponent<CanvasGroup>();
                }
                return mCanvasGroup;
            }
        }


        protected override void Start()
        {
            base.Start();
            if (CacheCanvasGroup != null)
            {
                isCanvasGroup = true;
            }
        }

        public override float value
        {
            get
            {
                return mAlpha;
            }
            set
            {
                mAlpha = value;
                SetAlpha(CachedTranform, value);
            }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = from + factor * (to - from);
        }

        void SetAlpha(Transform _transform, float _alpha)
        {
            if (isCanvasGroup)
            {
                CacheCanvasGroup.alpha = _alpha;
            }
            else
            {
                foreach (var item in CachedGraphics)
                {
                    Color color = item.color;
                    color.a = _alpha;
                    item.color = color;
                }
            }
        }

        public static TweenAlpha Begin(GameObject go, float from, float to, float duration = 1f, float delay = 0f)
        {
            TweenAlpha comp = Begin<TweenAlpha>(go, duration);
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