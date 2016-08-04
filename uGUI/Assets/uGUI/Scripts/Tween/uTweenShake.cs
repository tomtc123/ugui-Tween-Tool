using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace uTools
{
    public class uTweenShake : uTweener
    {
        private Transform mTarget;
        public Transform target
        {
            get
            {
                if (mTarget == null)
                {
                    mTarget = transform;
                    CacheTargetInfo();
                }
                return mTarget;
            }
        }

        private Vector3 localPosition = Vector3.zero;
        private Vector3 position = Vector3.zero;
        private Vector3 localScale = Vector3.zero;
        private Vector3 localEulerAngles = Vector3.zero;
        private Vector3 eulerAngles = Vector3.zero;

        public Vector3 limit;
        public Space space = Space.Self;
        public eShake shakeType = eShake.Position;

        private Vector3 mValue;
        public Vector3 value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
                Shake();
            }
        }

        private void CacheTargetInfo()
        {
            localPosition = target.localPosition;
            position = target.position;
            localScale = target.localScale;
            localEulerAngles = target.localEulerAngles;
            eulerAngles = target.eulerAngles;
        }

        protected override void Start()
        {
            base.Start();
            CacheTargetInfo();
        }


        protected override void OnUpdate(float factor, bool isFinished)
        {
            float x = limit.x * factor;
            float y = limit.y * factor;
            float z = limit.z * factor;
            mValue.x = UnityEngine.Random.Range(x * -1, x);
            mValue.y = UnityEngine.Random.Range(y * -1, y);
            mValue.z = UnityEngine.Random.Range(z * -1, z);
            value = mValue;
        }

        void Shake()
        {
            if (shakeType == eShake.Position)
            {
                if (space == Space.Self)
                {
                    target.localPosition = value + localPosition;
                }
                else
                {
                    target.position = value + position;
                }
            }
            else if (shakeType == eShake.Scale)
            {
                target.localScale = value + localScale;
            }
            else
            {
                if (space == Space.Self)
                {
                    target.localEulerAngles = value + localEulerAngles;
                }
                else
                {
                    target.eulerAngles = value + eulerAngles;
                }
            }
        }

        public static uTweenShake Begin(GameObject go, Vector3 from, float duration = 1f, float delay = 0f)
        {
            uTweenShake comp = uTweener.Begin<uTweenShake>(go, duration);
            comp.limit = from;
            comp.duration = duration;
            comp.delay = delay;
            if (duration <= 0)
            {
                comp.Sample(1, true);
                comp.enabled = false;
            }
            return comp;
        }        
    }

}