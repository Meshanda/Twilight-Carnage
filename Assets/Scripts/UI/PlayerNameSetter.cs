using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameSetter : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameLabel;

    public void SetName(string name)
    {
        _nameLabel.text = name;
    }
}
