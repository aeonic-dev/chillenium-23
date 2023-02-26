using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A serializable Dictionary wrapper.
/// </summary>
[Serializable]
public class SerializedMap<K, V> : ISerializationCallbackReceiver {
    private Dictionary<K, V> valueMap = new Dictionary<K, V>();

    [SerializeField]
    private K[] keys;
    [SerializeField]
    private V[] values;

    public void Set(K key, V value) {
        valueMap[key] = value;
    }

    public V Get(K key) {
        return valueMap[key];
    }

    public V GetOrDefault(K key, V fallback) {
        return valueMap.ContainsKey(key) ? valueMap[key] : fallback;
    }

    public V GetOrCreate(K key, Func<V> create) {
        if (!valueMap.ContainsKey(key)) {
            valueMap[key] = create();
        }
        return valueMap[key];
    }

    public void OnBeforeSerialize() {
        keys = new K[valueMap.Keys.Count];
        values = new V[valueMap.Values.Count];
        valueMap.Keys.CopyTo(keys, 0);
        valueMap.Values.CopyTo(values, 0);
    }

    public void OnAfterDeserialize() {
        valueMap = new Dictionary<K, V>();
        for (int i = 0; i < keys.Length; i++) {
            valueMap.Add(keys[i], values[i]);
        }
    }
}