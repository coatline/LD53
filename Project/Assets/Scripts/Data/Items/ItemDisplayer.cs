using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayer
{
    public event Action<ItemDisplayer> ContentsChanged;

    ItemStack itemStack;
    public ItemStack ItemStack
    {
        get => itemStack;
        set
        {
            if (itemStack == value) return;

            itemStack = value;

            if (value != null)
            {
                itemStack.CountChanged += CountChanged;
                itemStack.Destroyed += StackDestroyed;
            }

            ContentsChanged?.Invoke(this);
            UpdateImage();
        }
    }

    void CountChanged(ItemStack stack)
    {
        UpdateCountText();
    }

    void StackDestroyed(ItemStack stack)
    {
        itemStack.Destroyed -= StackDestroyed;
        itemStack.CountChanged -= CountChanged;
        ItemStack = null;
    }

    public ItemDisplayer(ItemStack itemStack, bool hasVisuals = false, Image i = null, SpriteRenderer sr = null, TMP_Text t = null)
    {
        ItemStack = itemStack;

        if (!hasVisuals) return;

        this.sr = sr;
        this.itemImage = i;
        this.countText = t;

        UpdateVisuals();
    }

    // Visuals

    TMP_Text countText;
    SpriteRenderer sr;
    Image itemImage;

    public void RemoveAllVisuals()
    {
        itemImage = null;
        countText = null;
        sr = null;
    }

    public void SetSpriteRenderer(SpriteRenderer sr)
    {
        this.sr = sr;
        UpdateImage();
    }

    void UpdateVisuals()
    {
        UpdateImage();
        UpdateCountText();
    }

    void UpdateCountText()
    {
        if (countText != null)
        {
            if (ItemStack != null && ItemStack.Type.MaxStack > 1)
                countText.text = ItemStack.Count.ToString();
            else
                countText.text = "";
        }
    }

    void UpdateImage()
    {
        if (ItemStack != null)
        {
            if (itemImage != null)
            {
                itemImage.enabled = true;
                itemImage.sprite = ItemStack.Type.Sprite;
            }
            if (sr != null)
                this.sr.sprite = ItemStack.Type.Sprite;
        }
        else
        {
            if (itemImage != null)
                itemImage.enabled = false;
            if (sr != null)
                sr.sprite = null;
        }
    }
}
