using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ObservableList<T> : IList<T>, IReadOnlyList<T>
{
    private readonly List<T> list = new();
    private Action _onCollectionChanged;

    // ========== 公共只读接口 ==========
    public T this[int index] => list[index]; // 只读索引器
    public int Count => list.Count;
    public bool IsReadOnly => false;
    public bool Contains(T item) => list.Contains(item);
    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
    public int IndexOf(T item) => list.IndexOf(item);
    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // ========== 显式接口实现（对外隐藏修改方法） ==========
    void IList<T>.Insert(int index, T item) => InsertInternal(index, item);
    void ICollection<T>.Add(T item) => AddInternal(item);
    bool ICollection<T>.Remove(T item) => RemoveInternal(item);
    void IList<T>.RemoveAt(int index) => RemoveAtInternal(index);
    void ICollection<T>.Clear() => ClearInternal();
    T IList<T>.this[int index]
    {
        get => list[index];
        set => SetItemInternal(index, value);
    }

    // ========== 内部修改方法（保持原名） ==========
    internal void Add(T item) => AddInternal(item);
    internal void Insert(int index, T item) => InsertInternal(index, item);
    internal bool Remove(T item) => RemoveInternal(item);
    internal void RemoveAt(int index) => RemoveAtInternal(index);
    internal void Clear() => ClearInternal();
    internal T SetAt(int index, T value) => SetItemInternal(index, value);
    internal void AddRange(IEnumerable<T> items) => list.AddRange(items);
    internal void Sort(Comparison<T> comparison) => list.Sort(comparison);

    // ========== 实际实现 ==========
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddInternal(T item)
    {
        list.Add(item);
        _onCollectionChanged?.Invoke();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void InsertInternal(int index, T item)
    {
        list.Insert(index, item);
        _onCollectionChanged?.Invoke();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool RemoveInternal(T item)
    {
        if (list.Remove(item))
        {
            _onCollectionChanged?.Invoke();
            return true;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void RemoveAtInternal(int index)
    {
        list.RemoveAt(index);
        _onCollectionChanged?.Invoke();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearInternal()
    {
        list.Clear();
        _onCollectionChanged?.Invoke();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private T SetItemInternal(int index, T value)
    {
        list[index] = value;
        _onCollectionChanged?.Invoke();
        return value;
    }

    // ========== 观察者模式 ==========
    public void Watched(Action onChange) => _onCollectionChanged += onChange;
}

