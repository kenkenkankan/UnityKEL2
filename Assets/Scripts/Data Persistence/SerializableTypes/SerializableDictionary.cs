using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Tooltip("Menambahkan data hanya bisa di editor saja, buat transaksi data save/load harus lewat build game")]
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [Serializable]
    private struct SerializableKeyValuePair
    {
        public TKey key;
        public TValue value;
        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [SerializeField] private List<SerializableKeyValuePair> serializedList = new();

    // Save dictionary to list
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        List<SerializableKeyValuePair> tempDict = new(serializedList);

        foreach (var pair in this.Select((val, index) => new { val, index }))
        {
            tempDict[pair.index] = new(pair.val.Key, pair.val.Value);
        }

        serializedList.Clear();
        serializedList = new(tempDict);
#else
        serializedList.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            serializedList.Add(new (pair.Key, pair.Value));
        }
#endif
    }

    // Load the dictionary from list
    public void OnAfterDeserialize()
    {
        this.Clear();
        for (int i = 0; i < serializedList.Count; i++)
        {
            if (serializedList[i].key != null && !this.ContainsKey(serializedList[i].key))
            {
                this.Add(serializedList[i].key, serializedList[i].value);
            }
        }
    }
}

[Tooltip("Menambahkan data hanya bisa di editor saja, buat transaksi data save/load harus lewat build game")]
[Serializable]
public class SerializableDictionaryValueList<TKey, TValue> : Dictionary<TKey, List<TValue>>, ISerializationCallbackReceiver
{
    public SerializableDictionaryValueList() : base() { }

    [Serializable]
    private struct SerializableKeyValuePair
    {
        public TKey key;
        public List<TValue> value;
        public SerializableKeyValuePair(TKey key, List<TValue> value)
        {
            this.key = key;
            this.value = value;
        }
    }
    
    [SerializeField] private List<SerializableKeyValuePair> serializedList = new();

    public void OnBeforeSerialize()
    {
        #if UNITY_EDITOR
        List<SerializableKeyValuePair> tempDict = new(serializedList);

        foreach (var pair in this.Select((val, index) => new { val, index }))
        {
            tempDict[pair.index] = new (pair.val.Key, pair.val.Value);
        }

        serializedList.Clear();
        serializedList = new(tempDict);  
        # else
        serializedList.Clear();
        foreach (KeyValuePair<TKey, List<TValue>> pair in this)
        {
            serializedList.Add(new (pair.Key, pair.Value));
        }
        #endif        
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        for (int i = 0; i < serializedList.Count; i++)
        {
            if (serializedList[i].key != null && !this.ContainsKey(serializedList[i].key))
            {
                this.Add(serializedList[i].key, serializedList[i].value);
            }
        }
    }
}