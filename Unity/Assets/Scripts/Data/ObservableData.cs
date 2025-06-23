using System;
using System.Reflection;

public class ObservableData<T>
{
    public T value
    {
        get =>  _value;
        set
        {
            if (!Equals(_value, value))
            {
                if(!DataUtility.EnsureCallerIsAllowed()) return;
                oldValue = _value;
                newValue = value;
                _value = value;
                onValueChange?.Invoke();
            }
        }
    }

    private T _value;
    private T oldValue;
    private T newValue;
    
    public ObservableData()
    {
        _value = default;
        value = _value;
        oldValue = _value;
        newValue = _value;
    }

    private Action onValueChange;

    public void Watched(Action<T,T> watched)
    {
        onValueChange += () =>
        {
            T currentOldValue = oldValue;
            T currentNewValue = newValue;
            watched?.Invoke(currentOldValue, currentNewValue);
        };
        onValueChange?.Invoke();
    }
}
