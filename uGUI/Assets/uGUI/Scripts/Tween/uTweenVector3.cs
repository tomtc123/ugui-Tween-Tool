using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
    public class uTweenVector3 : uTween<Vector3>
    {
        protected override void OnUpdate(float factor, bool isFinished)
		{
            this.value = from + factor * (to - from);
		}

        public static uTweenVector3 Begin(GameObject go, Vector3 from, Vector3 to, float duration, float delay)
        {
            uTweenVector3 comp = Begin<uTweenVector3>(go, duration);
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
