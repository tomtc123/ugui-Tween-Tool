using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace uTools {
	
	public class DragObject : MonoBehaviour, IDragHandler {

		public RectTransform target;

		RectTransform cacheTarget {
			get {
				if (target == null) {
					target = GetComponent<RectTransform>();
				}
				return target;
			}
		}

		public void OnDrag (PointerEventData eventData) {
			cacheTarget.localPosition += new Vector3 (eventData.delta.x, eventData.delta.y, 0);
		}
	}
}