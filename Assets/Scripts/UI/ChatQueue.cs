using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatQueue<T> : Queue<T>
{
    public int MaxCount { get; set; } = 0;

    public ChatQueue(int num)
    {
        MaxCount = num;
    }
    public ChatQueue() : this(0) { }
    public void Push(T item)
    {
        if (this.Count > MaxCount)
            base.Dequeue();
        base.Enqueue(item);
    }
}