using UnityEngine;
using System.Collections;
using System;

namespace uTools
{
    public abstract class uTween<T> : uTweener
    {
        public T from;
        public T to;
        public virtual T value { get; set; }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue() { from = value; }

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue() { to = value; }

        [ContextMenu("Assume value of 'From'")]
        public override void SetCurrentValueToStart() { value = from; }

        [ContextMenu("Assume value of 'To'")]
        public override void SetCurrentValueToEnd() { value = to; }
    }
}
