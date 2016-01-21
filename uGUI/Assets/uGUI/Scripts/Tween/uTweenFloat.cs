using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {	
	public class uTweenFloat : uTween<float> {

		protected override void OnUpdate(float factor, bool isFinished)
		{
            value = from + factor * (to - from);
		}

        public static uTweenFloat Begin(float from, float to, float duration, float delay)
        {
            GameObject go = new GameObject("___uTweenFloat");
            uTweenFloat comp = Begin<uTweenFloat>(go, duration);
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
