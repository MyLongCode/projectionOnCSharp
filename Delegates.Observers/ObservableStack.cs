using System.Text;

namespace Delegates.Observers;

public class StackOperationsLogger
{
    public StringBuilder Logger = new StringBuilder();

    public string GetLog() => Logger.ToString();

    public void SubscribeOn<T>(ObservableStack<T> stack)
    {
        stack.OnStackChanged += (sender, e) =>
        {
            Logger.Append(e);
        };
    }
}

public class ObservableStack<T>
{
    public event EventHandler<StackEventData<T>> OnStackChanged;
    List<T> stackData = new List<T>();

    protected void Notify(StackEventData<T> eventData)
    {
        OnStackChanged?.Invoke(this, eventData);
    }

    public T Pop()
    {
        if (stackData.Count == 0)
            throw new InvalidOperationException();
        var last_element = stackData[stackData.Count - 1];
        Notify(new StackEventData<T> { IsPushed = false, Value = last_element });
        return last_element;
    }

    public void Push(T element)
    {
        stackData.Add(element);
        Notify(new StackEventData<T> { IsPushed = true, Value = element });
    }
}