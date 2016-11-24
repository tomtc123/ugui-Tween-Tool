using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	
	public class TweenRect : Tween<Vector2> {

		private RectTransform mRectTransform;
		public RectTransform cacheRectTransform {
			get {
				if (mRectTransform == null) {
					mRectTransform = GetComponent<RectTransform>();
				}
				return mRectTransform;
			}
		}

		protected override void OnUpdate (float factor, bool isFinished) {
			value = from + factor * (to - from);
			cacheRectTransform.sizeDelta = value;
		}

		public static TweenRect Begin(RectTransform go, Vector2 from, Vector2 to, float duration, float delay) {
			TweenRect comp = Begin<TweenRect>(go.gameObject, duration);
            comp.value = from;
			comp.from = from;
			comp.to = to;
			comp.delay = delay;
			
			if (duration <=0) {
				comp.Sample(1, true);
				comp.enabled = false;
			}
			return comp;
		}
	}
}
