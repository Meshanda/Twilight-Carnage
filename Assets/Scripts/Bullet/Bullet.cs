using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; set; }
    public int Life { get; set; }
    public float Speed { get; set; }

    
    public bool IsDead()
    {
        return Life <= 0;
    }
}
