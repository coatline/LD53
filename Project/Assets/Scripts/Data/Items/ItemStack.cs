using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
    public event Action<ItemStack> CountChanged;
    public event Action<ItemStack> Destroyed;

    public readonly Item Type;
    public int Count
    {
        get => count;
        set
        {
            if (value == count) return;

            count = Mathf.Clamp(value, 0, Type.MaxStack);

            CountChanged?.Invoke(this);

            if (count == 0)
                Destroyed?.Invoke(this);
        }
    }
    int count;

    public ItemStack(Item item, int count)
    {
        Type = item;
        Count = count;
    }

    public int SpaceLeft() => Type.MaxStack - Count;

    /// <returns>Remainder count</returns>
    public int TryAdd(int count)
    {
        int initialCount = Count;
        Count += count;
        return Mathf.Max((count + initialCount) - Count, 0);
    }
}
