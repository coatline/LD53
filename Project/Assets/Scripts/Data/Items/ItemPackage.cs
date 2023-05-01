using UnityEngine;

[System.Serializable]
public class ItemPackage
{
    [SerializeField] Item item;
    [SerializeField] int count;

    public ItemPackage(Item item, int count)
    {
        this.Item = item;
        this.Count = count;
    }

    public Item Item { get { return item; } set { item = value; } }
    public int Count { get { return count; } set { count = value; } }
}
