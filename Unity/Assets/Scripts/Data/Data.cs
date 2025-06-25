using System;

public class Data<T>
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
                tOnValueChange?.Invoke(oldValue,newValue);
            }
        }
    }

    private T value;
    private T oldValue;
    private T newValue;
    
    public Data()
    {
        value = default;
        Value = value;
        oldValue = value;
        newValue = value;
    }

    private Action onValueChange;

    private Action<T, T> tOnValueChange;

    
    public void Watched(Action watched)
    {
        if (watched == null) return;
        onValueChange += watched;
        onValueChange?.Invoke();
    }
    public void Watched(Action<T, T> watched)
    {
        if (watched == null) return;
        tOnValueChange += watched; 
        tOnValueChange?.Invoke(oldValue,newValue);
    }
    
    public void UnWatched(Action watched)
    {
        if (watched == null) return;
        onValueChange -= watched;
    }
    
    public void UnWatched(Action<T, T> watched)
    {
        if (watched == null || tOnValueChange == null) return;
        tOnValueChange -= watched;
    }

}
