using System;

public class ObservableData<T>
{
    public T Value
    {
        get =>  value;
        internal set
        {
            if (!Equals(this.value, value))
            {
                oldValue = this.value;
                newValue = value;
                this.value = value;
                onValueChange?.Invoke();
            }
        }
    }

    private T value;
    private T oldValue;
    private T newValue;
    
    public ObservableData()
    {
        value = default;
        Value = value;
        oldValue = value;
        newValue = value;
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
