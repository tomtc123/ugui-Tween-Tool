using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Internal/Play Tween(uTools)")]
	public class uPlayTween : MonoBehaviour, uIPointHandler {
		public GameObject tweenTarget;
        public Direction playDirection = Direction.Forward;
		public Trigger trigger = Trigger.OnPointerClick;
		public int tweenGroup = 0;
		public bool inCludeChildren = false;

		private uTweener[] mTweeners;

		// Use this for initialization
		void Start () {
			if (tweenTarget == null) {
				tweenTarget = gameObject;
			}		
			mTweeners = inCludeChildren? tweenTarget.GetComponentsInChildren<uTweener>() : tweenTarget.GetComponents<uTweener>();
		}

		public void OnPointerEnter (PointerEventData eventData) {
			TriggerPlay (Trigger.OnPointerEnter);
		}

		public void OnPointerDown (PointerEventData eventData) {
			TriggerPlay (Trigger.OnPointerDown);
		}

		public void OnPointerClick (PointerEventData eventData) {
			TriggerPlay (Trigger.OnPointerClick);
		}

		public void OnPointerUp (PointerEventData eventData) {
			TriggerPlay (Trigger.OnPointerUp);
		}

		public void OnPointerExit (PointerEventData eventData) {
			TriggerPlay (Trigger.OnPointerExit);
		}

		private void TriggerPlay(Trigger _trigger) {
			if (_trigger == trigger) {
				Play();
			}
		}

		/// <summary>
		/// Play this instance.
		/// </summary>
		private void Play() {
            if (playDirection == Direction.Toggle)
            {
				foreach (var item in mTweeners) {
					if (item.tweenGroup == tweenGroup) {
						item.Toggle();
					}
				}
			}
			else {
				foreach (var item in mTweeners) {
					if (item.tweenGroup == tweenGroup) {
						item.Play(playDirection == Direction.Forward);
					}
				}
			}
		}

	}
}