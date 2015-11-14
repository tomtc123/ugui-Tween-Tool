using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Alpha(uTools)")]
	public class uTweenAlpha : uTweenValue {

		[HideInInspector]
		Transform mTransform;

		public bool includeChilds = false;

		private Text mText;
		private Light mLight;
		private Material mMat;
		private Image mImage;
		private SpriteRenderer mSpriteRender;

		float mAlpha = 0f;

		protected override void Start ()
		{
			mTransform = transform;
			mText = mTransform.GetComponent<Text> ();
			mLight = mTransform.GetComponent<Light>();
			mImage = mTransform.GetComponent<Image> ();
			mSpriteRender = mTransform.GetComponent<SpriteRenderer> ();
			
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
			Color c = Color.white;
			if (mText != null){
				c = mText.color;
				c.a = _alpha;
				mText.color = c;
			}
			if (mLight != null){ 
				c = mLight.color;
				c.a = _alpha;
				mLight.color = c;
			}
			if (mImage != null) {
				c = mImage.color;
				c.a = _alpha;
				mImage.color = c;
			}
			if (mSpriteRender != null) {
				c = mSpriteRender.color;
				c.a = _alpha;
				mSpriteRender.color = c;
			}
			if (_transform.GetComponent<Renderer>() != null) {
				mMat = _transform.GetComponent<Renderer>().material;
				if (mMat != null) {
					c = mMat.color;
					c.a = _alpha;
					mMat.color = c;
				}
			}
			if (includeChilds) {
				for (int i = 0; i < _transform.childCount; ++i) {
					Transform child = _transform.GetChild(i);
					SetAlpha(child, _alpha);
				}
			}

		}

	}

}