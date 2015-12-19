using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Color(uTools)")]
	public class uTweenColor : uTweener {
		
		public GameObject target;
		public Color from = Color.white;
		public Color to = Color.white;
		public bool includeChildren = false;

		Graphic[] mGraphics;

		Color mColor = Color.white;

		protected override void Start ()
		{
			if (target == null) {
				target = gameObject;
			}
			mGraphics = includeChildren?target.GetComponentsInChildren<Graphic>() : target.GetComponents<Graphic>();
			base.Start ();
		}

		public Color colorValue {
			get {
				return mColor;
			}
			set {
				SetColor(transform, value);
				mColor = value;
			}
		}

		protected override void OnUpdate (float factor, bool isFinished)
		{
			colorValue = Color.Lerp(from, to, factor);
		}

		public static uTweenColor Begin(GameObject go, float duration, float delay, Color from, Color to) {
			uTweenColor comp = uTweener.Begin<uTweenColor>(go, duration);
			comp.from = from;
			comp.to = to;
			comp.delay = delay;
			
			if (duration <=0) {
				comp.Sample(1, true);
				comp.enabled = false;
			}
			return comp;
		}

		void SetColor(Transform _transform, Color _color) {
			foreach (var item in mGraphics) {
				item.color = _color;
			}			
		}


	}
}
