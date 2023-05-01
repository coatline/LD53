using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    public event System.Action<ItemStack> SwitchedItem;
    public ItemStack ItemStack { get; private set; }
    public Item Item
    {
        get
        {
            if (ItemStack == null)
                return null;

            return ItemStack.Type;
        }
    }
    ItemDisplayer itemDisplayer;


    [SerializeField] SpriteRenderer muzzleFlash;
    [SerializeField] SpriteRenderer itemSprite;

    [SerializeField] Transform handTransform;
    [SerializeField] Transform handSprite;

    [SerializeField] RecoilAnimation recoil;
    [SerializeField] Damageable damageable;
    [SerializeField] ItemUser itemUser;

    [SerializeField] bool dontDropItem;
    [SerializeField] float reach;


    private void Awake()
    {
        itemDisplayer = new ItemDisplayer(ItemStack, true, null, itemSprite);
        damageable.Died += DropItem;
    }

    void DropItem()
    {
        if (dontDropItem) return;

        if (Item != null)
            LooseItemSpawner.I.SpawnItem(new ItemPackage(Item, 1), transform.position);
    }

    public void ChangeItem(ItemStack stack)
    {
        ItemStack = stack;
        itemDisplayer.ItemStack = ItemStack;

        SwitchedItem?.Invoke(stack);

        // If we use up the item in our hand.
        stack.Destroyed += StackDepleted;
    }

    void StackDepleted(ItemStack stack) => SwitchedItem?.Invoke(null);

    /// <summary>
    /// Aims the hand and item towards designated position.
    /// </summary>
    /// <param name="toPosition">The position you want to aim at.</param>
    /// <param name="aimVariability">For variability in AI attacks.</param>
    public void Aim(Vector3 toPosition, Vector2 aimVariability)
    {
        float angle = Extensions.AngleFromPosition(transform.position, toPosition);

        angle += Random.Range((float)aimVariability.x, (float)aimVariability.y);

        float flip = 0;

        if (angle > 0 || angle < -180)
        {
            flip = 180;
        }

        toPosition.z = 0;
        Vector2 pos = (toPosition - transform.position).normalized * (Mathf.Clamp(Vector2.Distance(transform.position, toPosition), 0f, reach));

        handTransform.localPosition = pos;

        handTransform.transform.localRotation = Quaternion.Euler(0, 0, (angle + 90));
        handSprite.transform.localRotation = Quaternion.Euler(flip, 0, 0);
    }
}