using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecoilSettings
{
    [SerializeField] float recoilAmount;
    [SerializeField] float recoverySpeed;
    [SerializeField] float recoveryDelay;

    public RecoilSettings(float recoilAmount, float recoverySpeed, float recoveryDelay)
    {
        this.recoilAmount = recoilAmount;
        this.recoverySpeed = recoverySpeed;
        this.recoveryDelay = recoveryDelay;
    }

    public float RecoilAmount { get { return recoilAmount; } }
    public float RecoverySpeed { get { return recoverySpeed; } }
    public float RecoveryDelay { get { return recoveryDelay; } }
}