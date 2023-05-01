using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-2)]
public class DataLibrary : Singleton<DataLibrary>
{
    public Getter<Gun> Guns { get; private set; }
    public Getter<Level> Levels { get; private set; }
    public Getter<SmartAIController> Allies { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Guns = new Getter<Gun>(Resources.LoadAll<Gun>("Guns"));
        Allies = new Getter<SmartAIController>(Resources.LoadAll<SmartAIController>("Prefabs/Allies"));
        Levels = new Getter<Level>(Resources.LoadAll<Level>("Levels"));
    }
}
