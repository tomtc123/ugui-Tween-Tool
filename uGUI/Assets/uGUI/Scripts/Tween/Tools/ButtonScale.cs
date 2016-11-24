using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace uTools {
	
	public class ButtonScale : MonoBehaviour, IPointHandler {

		public RectTransform tweenTarget;
		public Vector3 enter = new Vector3(1.1f, 1.1f, 1.1f);
		public Vector3 down = new Vector3(1.05f, 1.05f, 1.05f);
		public float duration = .2f;

		Vector3 mScale;
        TweenScale mTween;
        RectTransform mCacheTarget
        {
            get
            {
                if (tweenTarget == null)
                {
                    tweenTarget = GetComponent<RectTransform>();
                    mScale = tweenTarget.localScale;
                }
                return tweenTarget;
            }
        }

        // Use this for initialization
        void Start () {
			if (tweenTarget == null) {
				tweenTarget = GetComponent<RectTransform>();
			}
			mScale = tweenTarget.localScale;
		}

        void OnDisable()
        {
            mCacheTarget.localScale = mScale;
            if (mTween)
                mTween.enabled = false;
        }

        public void OnPointerEnter (PointerEventData eventData) {
			Scale(enter);
		}

		public void OnPointerExit(PointerEventData eventData) {
			Scale(mScale);		
		}

		public void OnPointerDown (PointerEventData eventData) {
			Scale(down);
		}

		public void OnPointerUp (PointerEventData eventData) {
			Scale(mScale);		
		}

		public void OnPointerClick (PointerEventData eventData) {
		}

		void Scale(Vector3 to) {
            mTween = TweenScale.Begin(tweenTarget.gameObject, tweenTarget.localScale, to, duration);
		}
	}
}
