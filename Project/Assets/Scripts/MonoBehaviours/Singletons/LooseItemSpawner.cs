using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseItemSpawner : Singleton<LooseItemSpawner>
{
    [SerializeField] LooseItem looseItemPrefab;

    public void SpawnItem(ItemPackage ip, Vector3 pos, bool doForce = true)
    {
        LooseItem li = Instantiate(looseItemPrefab, pos, Quaternion.identity);
        li.Setup(ip, doForce);
    }

    //public void SpawnItems(ItemPool itemPool, Vector3 pos, bool doForce = true)
    //{
    //    ItemPackage[] items = itemPool.GetItems();

    //    for (int k = 0; k < items.Length; k++)
    //    {
    //        LooseItem li = Instantiate(looseItemPrefab, pos, Quaternion.identity, transform);
    //        li.Setup(items[k].Item, items[k].Count, doForce);
    //    }
    //}
}
