using System;

public class ObserData<T>
{
    public T value
    {
        get =>  _value;
        set
        {
            if (!Equals(_value, value))
            {
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

    public Action onValueChange;

    public (T,T) Watched(Action watched)
    {
        onValueChange += watched;
        onValueChange?.Invoke();
        return (oldValue,newValue);
    }
}
