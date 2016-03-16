using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Alpha(uTools)")]
	public class uTweenAlpha : uTween<float> {

		public GameObject target;
		public bool includeChildren = false;

		Transform mTransform;
		Graphic[] mGraphics;

		float mAlpha = 0f;

		protected override void Start ()
		{
			mTransform = GetComponent<Transform>();
			if (target == null) {
				target = gameObject;
			}
			mGraphics = includeChildren?target.GetComponentsInChildren<Graphic>() : target.GetComponents<Graphic>();			
			base.Start ();
		}

		public float alpha {
			get {
				return mAlpha;
			}
			set {
				SetAlpha(mTransform, value);
				mAlpha = value;
			}
		}

		protected override void OnUpdate (float value, bool isFinished)
		{
			alpha = value;
		}

		void SetAlpha(Transform _transform, float _alpha) {
			foreach (var item in mGraphics) {
				Color color = item.color;
				color.a = _alpha;
				item.color = color;
			}
		}

        public static uTweenAlpha Begin(GameObject go, float from, float to, float duration = 1f, float delay = 0f)
        {
            uTweenAlpha comp = Begin<uTweenAlpha>(go, duration);
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