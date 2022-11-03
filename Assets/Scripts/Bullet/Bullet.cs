using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; set; }
    public int Life { get; set; }
    public float Speed { get; set; }

    private void Update()
    {
        Transform bulletTransform = transform;
        Vector3 newPosition = bulletTransform.position + bulletTransform.forward * (Speed * Time.deltaTime);
        bulletTransform.position = newPosition;
    }

    public bool IsDead()
    {
        return Life <= 0;
    }
}
