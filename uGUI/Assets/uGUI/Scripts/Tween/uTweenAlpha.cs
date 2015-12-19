using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Alpha(uTools)")]
	public class uTweenAlpha : uTweenValue {

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

		protected override void ValueUpdate (float value, bool isFinished)
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

	}

}