using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Util {
    /// <summary>
    /// Holds a single nullable instance of an object, clearing when necessary and calling a given mehtod
    /// upon set/get. Just useful because I do this a lot anyway with member fields, this is easier.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Singleton<T> {
        public T Value {
            get => value;
            set {
                if (this.value != null) _onUnset?.Invoke(this.value);
                this.value = value;
                if (this.value != null) _onSet?.Invoke(this.value);
            }
        }
        
        [SerializeField] private T value;
        private readonly T _defaultValue;
        private Action<T> _onSet;
        private Action<T> _onUnset;

        /// <param name="initial">An initial value</param>
        /// <param name="onSet">The function to call on a given value when it's set</param>
        /// <param name="onUnset">The function to call on a given value when it becomes no longer the selected instance</param>
        public Singleton(T initial, Action<T> onSet = null, Action<T> onUnset = null) {
            value = initial;
            _defaultValue = initial;
            _onSet = onSet;
            _onUnset = onUnset;
        }

        [CanBeNull]
        public R Map<R>(Func<T, R> function) {
            return IsPresent() ? function.Invoke(value) : default;
        }

        public void IfPresent(Action<T> action) {
            if (IsPresent()) action.Invoke(value);
        }

        public bool IsPresent() {
            return value != null;
        }

        public T Get() {
            return value;
        }

        public void Clear() {
            Set(_defaultValue);
        }

        public void Set(T value) {
            Value = value;
        }
    }
}