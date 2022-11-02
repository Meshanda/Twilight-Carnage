using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHpSetter : MonoBehaviour
{
    [SerializeField] private ScriptableObjects.Variables.FloatVariable _hp;
    [SerializeField] private float _maxHp;  

    // Start is called before the first frame update
    void Start()
    {
        _hp.value = _maxHp;
    }


    public void TakeDamage( float hp) 
    {
        Debug.Log(hp + "" + _hp.value);
        _hp.value -= hp;
    }
}
