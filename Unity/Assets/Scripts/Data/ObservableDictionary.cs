using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ObservableDictionary<TKey, TValue> : IEnumerable
{
    private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
    private Action _onDictionaryChange;

    // 索引器
    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set
        {
            _dictionary[key] = value;
            _onDictionaryChange?.Invoke();
        }
    }

    // 键集合
    public ICollection<TKey> Keys => _dictionary.Keys;

    // 值集合
    public ICollection<TValue> Values => _dictionary.Values;
    
    public int Count => _dictionary.Count;
    
    public bool IsReadOnly => false;
    
    public void Add(TKey key, TValue value)
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return;
        }
        _dictionary.Add(key, value);
        _onDictionaryChange?.Invoke();
    }
    
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return;
        }
        _dictionary.Add(item.Key, item.Value);
        _onDictionaryChange?.Invoke();
    }
    
    public void Clear()
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return;
        }
        _dictionary.Clear();
        _onDictionaryChange?.Invoke();
    }
    
    public bool Contains(KeyValuePair<TKey, TValue> item) =>
        _dictionary.TryGetValue(item.Key, out var value) && EqualityComparer<TValue>.Default.Equals(value, item.Value);
    
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
    
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        foreach (var kvp in _dictionary)
        {
            array[arrayIndex++] = kvp;
        }
    }

    // 获取枚举器
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    // 非泛型枚举器
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public bool Remove(TKey key)
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return false;
        }
        if (_dictionary.Remove(key))
        {
            _onDictionaryChange?.Invoke();
            return true;
        }
        return false;
    }
    
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return false;
        }
        if (Contains(item))
        {
            _dictionary.Remove(item.Key);
            _onDictionaryChange?.Invoke();
            return true;
        }
        return false;
    }
    
    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

    // 注册变化事件
    public void Watched(Action onDictionaryChange)
    {
        _onDictionaryChange += onDictionaryChange;
    }
}
