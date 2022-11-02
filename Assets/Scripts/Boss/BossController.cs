using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BossController : NetworkBehaviour
{
    [SerializeField] private LaserManager _laser;

    public void LaunchAbilityOne()
    {
        _laser.StartAbility();
    }
}
