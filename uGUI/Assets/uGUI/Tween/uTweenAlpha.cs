using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uGUI {
	public class uTweenAlpha : uTweenValue {

		private Text mText;
		private Light mLight;
		private Material mMat;
		private Image mImage;
		private SpriteRenderer mSpriteRender;

		bool mCached = false;

		void Cache() {
			mCached = true;
			mText = GetComponent<Text> ();
			if (mText != null){ return;}
			mLight = light;
			if (mLight != null){ return;}
			mImage = GetComponent<Image> ();
			if (mImage != null) { return;}
			mSpriteRender = GetComponent<SpriteRenderer> ();
			if (mSpriteRender != null) { return;}
			mMat = renderer.material;
			if (mMat != null) { return;}
		}

		public float alpha {
			get {
				if (!mCached) Cache();
				if (mText != null) return mText.color.a;
				if (mLight != null) return mLight.color.a;
				if (mImage != null) return mImage.color.a;
				if (mSpriteRender != null) return mSpriteRender.color.a;
				if (mMat != null) return mMat.color.a;
				return Color.white.a;
			}
			set {
				if (!mCached) Cache();
				Color c = Color.white;
				if (mText != null) {
					c = mText.color;
					c.a = value;
					mText.color = c;
				}
				if (mLight != null) {
					c = mLight.color;
					mLight.color = c;
				}
				if (mImage != null) {
					c = mImage.color;
					c.a = value;
					mImage.color = c;
				}
				if (mSpriteRender != null) {
					c = mSpriteRender.color;
					c.a = value;
					mSpriteRender.color = c;
				}
				if (mMat != null) {
					c = mMat.color;
					c.a = value;
					mMat.color = c;
				}
			}
		}

		protected override void ValueUpdate (float value, bool isFinished)
		{
			alpha = value;
		}

	}

}