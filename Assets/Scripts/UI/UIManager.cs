using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : GenericSingleton<UIManager>
{
    [SerializeField] private Texture2D _menuCursor;
    [SerializeField] private Texture2D _crosshairCursor;

    private void Start()
    {
        SetMenuCursor();
    }

    public void SetMenuCursor()
    {
        Cursor.SetCursor(_menuCursor, Vector2.zero, CursorMode.ForceSoftware);
    }
    
    public void SetCrosshairCursor()
    {
        Cursor.SetCursor(_crosshairCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
    }
}
