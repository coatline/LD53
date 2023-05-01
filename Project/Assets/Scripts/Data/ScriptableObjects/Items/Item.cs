using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NEW ITEM JACK", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] ItemType type;

    //[SerializeField] Weapon weapon;
    [SerializeField] Sprite sprite;
    [SerializeField] Sound soundOnUse;
    [TextArea(0, 3)]
    [SerializeField] string description;
    [SerializeField] int cost;

    [SerializeField] bool autoUse;
    [SerializeField] float useRate;
    [SerializeField] int maxStack;
    [SerializeField] int healAmount;

    [SerializeField] RecoilSettings recoilSettings;

    public int HealAmount { get { return healAmount; } }
    //public Weapon Weapon { get { return weapon; } }
    public Sound SoundOnUse { get { return soundOnUse; } }
    public int Cost { get { return cost; } }
    public int MaxStack { get { return maxStack; } }
    public float UseRate { get { return useRate; } }
    public bool AutoUse { get { return autoUse; } }
    public Sprite Sprite { get { return sprite; } }
    public ItemType Type { get { return type; } }
    public string Description { get { return description; } }
    public RecoilSettings RecoilSettings => recoilSettings;

    public string Name => name;
}

public enum ItemType
{
    Gun,
    Sub,
    Special,
    Health,
    Structure,
    Generic
}