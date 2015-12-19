using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Width(uTools)")]	
	
	public class uTweenRect : uTweener {

		public Vector2 from;
		public Vector2 to;

		Vector2 mValue;
		public Vector2 value {
			get { return mValue;}
			set { 
				mValue = value;
			}
		}

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

		public static uTweenRect Begin(RectTransform go, float duration, float delay, Vector2 from, Vector2 to) {
			uTweenRect comp = uTweener.Begin<uTweenRect>(go.gameObject, duration);
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
