using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uGUI {
	public class uTweenColor : uTweener {

		public Color from = Color.white;
		public Color to = Color.white;

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

		public Color colorValue {
			get {
				if (!mCached) Cache();
				if (mText != null) return mText.color;
				if (mLight != null) return mLight.color;
				if (mImage != null) return mImage.color;
				if (mSpriteRender != null) return mSpriteRender.color;
				if (mMat != null) return mMat.color;
				return Color.white;
			}
			set {
				if (!mCached) Cache();
				if (mText != null) mText.color = value;
				if (mLight != null) mLight.color = value;
				if (mImage != null) mImage.color = value;
				if (mSpriteRender != null) mSpriteRender.color = value;
				if (mMat != null) mMat.color = value;
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
	}
}
