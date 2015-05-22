using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConcurrentQueue<T>
{
    private readonly Queue<T> _queue = new Queue<T>();

    private readonly object _syncObject = new object();

    public void Enqueue(T item)
    {
        lock (_syncObject)
        {
            _queue.Enqueue(item);
        }
    }

    public bool TryDequeue(out T item)
    {
        lock (_syncObject)
        {
            if (_queue.Count > 0)
            {
                item = _queue.Dequeue();
                return true;
            }
            item = default(T);
            return false;
        }
    }

    public int Count
    {
        get
        {
            lock (_syncObject)
            {
                return _queue.Count;
            }
        }
    }
}