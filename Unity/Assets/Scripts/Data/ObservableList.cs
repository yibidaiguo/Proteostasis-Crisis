using System;
using System.Collections.Generic;

public class ObservableList<T> : IList<T>
{
    private readonly List<T> _list = new List<T>();
    private Action _onElementChange;

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value; // 不触发事件，因为这只是替换元素
    }

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return;
        }
        _list.Add(item);
        _onElementChange?.Invoke();
    }

    public void Clear()
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return;
        }
        _onElementChange?.Invoke();
        _list.Clear();
    }

    public bool Contains(T item) => _list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    public int IndexOf(T item) => _list.IndexOf(item);

    public void Insert(int index, T item)
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return;
        }
        _list.Insert(index, item);
        _onElementChange?.Invoke();
    }

    public bool Remove(T item)
    {
        if (_list.Remove(item))
        {
            if (!DataUtility.EnsureCallerIsAllowed())
            {
                return false;
            }
            _onElementChange?.Invoke();
            return true;
        }
        return false;
    }

    public void RemoveAt(int index)
    {
        if (!DataUtility.EnsureCallerIsAllowed())
        {
            return;
        }
        var item = _list[index];
        _list.RemoveAt(index);
        _onElementChange?.Invoke();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    public void Watched(Action onElementChange)
    {
        _onElementChange += onElementChange;
    }
}

