using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace uTools {
		
	public class TweenText : Tween<float> {

        private float mValue;

        private Text mText;
        Text cacheText
        {
            get
            {
                if (mText == null)
                {
                    mText = GetComponent<Text>();
                }
                return mText;
            }
        }

        public string format = "{0}";

        public override float value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                if (isTime)
                {
                    cacheText.text = string.Format(format, GetTime());
                }
                else
                {
                    cacheText.text = (System.Math.Round(value, digits)).ToString();
                }
            }
        }

        /// <summary>
        /// number after the digit point
        /// </summary>
        public int digits = 0;
        public bool isTime = false;

        protected string GetTime()
        {
            TimeSpan dt = new TimeSpan(0, 0, (int)value);
            string strTime = "";
            if (dt.Hours > 0)
            {
                strTime = string.Format("{0:00}:{1:00}:{2:00}", dt.Hours, dt.Minutes, dt.Seconds);
            }
            else
            {
                strTime = string.Format("{0:00}:{1:00}", dt.Minutes, dt.Seconds);
            }
            return strTime;
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = from + factor * (to - from);
        }

        public static TweenText Begin(Text label, float from, float to, float duration, float delay) {
			TweenText comp = Begin<TweenText>(label.gameObject, duration);
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
