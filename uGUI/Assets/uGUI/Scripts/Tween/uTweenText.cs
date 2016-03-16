using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Text(uTools)")]	
	
	public class uTweenText : uTween<float> {

		private Text mText;
		public Text cacheText {
			get {
				if (mText == null) {
					mText = GetComponent<Text>();
				}
				return mText;
			}
		}

		/// <summary>
		/// number after the digit point
		/// </summary>
		public int digits;

		protected override void OnUpdate(float value, bool isFinished)
		{
			cacheText.text = (System.Math.Round(value, digits)).ToString();
		}

		public static uTweenText Begin(Text label, float from, float to, float duration, float delay) {
			uTweenText comp = Begin<uTweenText>(label.gameObject, duration);
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
