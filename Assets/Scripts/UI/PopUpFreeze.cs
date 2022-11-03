using System;
using System.Collections;
using System.Collections.Generic;
using DelegateToolBox;
using Unity.Netcode;
using UnityEngine;

public class PopUpFreeze : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    
    private float _freezeSpeed = 1.0f; // 1 -> 1sec/1 = 1sec, 0.5 -> 1sec/0.5 = 2sec, 2 -> 1sec/2 = 0.5sec
    private WaitForEndOfFrame _cachedYield = new WaitForEndOfFrame();

    private bool _isAnimating;

    public void OnEventTriggerred() // Unity Event
    {
        ToggleFreeze();
    }

    private void ToggleFreeze()
    {
        if (IsTimeFreezed())
        {
            StopFreeze();
        }
        else
        {
            StartFreeze();
        }
    }
    
    private bool IsTimeFreezed()
    {
        return Time.timeScale <= 0;
    }

    private void StopFreezeTimeCoroutine()
    {
        StopCoroutine(nameof(FreezeTimeCoroutine));
    }

    private IEnumerator FreezeTimeCoroutine(bool freeze = true)
    {
        if (freeze)
        {
            while (Time.timeScale > 0)
            {
                
                Time.timeScale = Mathf.Max(Time.timeScale - Time.unscaledDeltaTime * _freezeSpeed, 0);
                yield return _cachedYield;
            }

            TimeFreezed();
        }
        else
        {
            while (Time.timeScale < 1)
            {
                Time.timeScale = Mathf.Min(Time.timeScale + Time.unscaledDeltaTime * _freezeSpeed, 1);
                yield return _cachedYield;
            }

            TimeUnfreezed();
        }
    }

    public void StartFreeze()
    {
        if (_isAnimating)
            return;

        _isAnimating = true;
        
        StopFreezeTimeCoroutine();
        StartCoroutine(FreezeTimeCoroutine());
        // ...
    }
    
    private void TimeFreezed()
    {
        _isAnimating = false;
        
        // show UI
        _canvas.enabled = true;
    }
    
    public void StopFreeze()
    {
        if (_isAnimating)
            return;

        _isAnimating = true;
        
        StopFreezeTimeCoroutine();
        StartCoroutine(FreezeTimeCoroutine(false));
        // hide UI
        _canvas.enabled = false;
    }
    
    private void TimeUnfreezed()
    {
        _isAnimating = false;
        
        // ...
    }
}
