using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    private void LateUpdate()
    {
        transform.LookAt(_cam.transform);
    }
}
