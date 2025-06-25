using System;
using System.Collections;
using System.Collections.Generic;
public class MyDictionary<TKey, TValue> : IEnumerable
{
    private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
    private Action _onDictionaryChange;

    // 索引器
    public TValue this[TKey key]
    {
        get => dictionary[key];
        internal set
        {
            dictionary[key] = value;
            _onDictionaryChange?.Invoke();
        }
    }

    // 键集合
    public ICollection<TKey> Keys => dictionary.Keys;

    // 值集合
    public ICollection<TValue> Values => dictionary.Values;
    
    public int Count => dictionary.Count;
    
    public bool IsReadOnly => false;
    
    internal void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
        _onDictionaryChange?.Invoke();
    }
    
    internal void Add(KeyValuePair<TKey, TValue> item)
    {
        dictionary.Add(item.Key, item.Value);
        _onDictionaryChange?.Invoke();
    }
    
    internal void Clear()
    {
        dictionary.Clear();
        _onDictionaryChange?.Invoke();
    }
    
    public bool Contains(KeyValuePair<TKey, TValue> item) =>
        dictionary.TryGetValue(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);
    
    public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
    
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        foreach (var kvp in dictionary)
        {
            array[arrayIndex++] = kvp;
        }
    }

    // 获取枚举器
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();

    // 非泛型枚举器
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    internal bool Remove(TKey key)
    {
        if (dictionary.Remove(key))
        {
            _onDictionaryChange?.Invoke();
            return true;
        }
        return false;
    }
    
    internal bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (Contains(item))
        {
            dictionary.Remove(item.Key);
            _onDictionaryChange?.Invoke();
            return true;
        }
        return false;
    }
    
    public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);

    // 注册变化事件
    public void Watched(Action onDictionaryChange)
    {
        _onDictionaryChange += onDictionaryChange;
    }
}
