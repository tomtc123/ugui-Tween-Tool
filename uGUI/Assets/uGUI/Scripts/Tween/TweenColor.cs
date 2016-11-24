using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	
	public class TweenColor : Tween<Color> {

        public bool includeChildren = false;

        Graphic[] mGraphics;
        Color mColor = Color.white;
        public override Color value
        {
            get
            {
                return mColor;
            }
            set
            {
                SetColor(transform, value);
                mColor = value;
            }
        }

        protected override void Start()
        {
            mGraphics = includeChildren ? gameObject.GetComponentsInChildren<Graphic>() : gameObject.GetComponents<Graphic>();
            base.Start();
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = Color.Lerp(from, to, factor);
        }

        void SetColor(Transform _transform, Color _color)
        {
            foreach (var item in mGraphics)
            {
                item.color = _color;
            }
        }

        public static TweenColor Begin(GameObject go, Color from, Color to, float duration, float delay)
        {
            TweenColor comp = Tweener.Begin<TweenColor>(go, duration);
            comp.value = from;
            comp.from = from;
            comp.to = to;
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
