using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Variables/GameObjects", fileName = "New GameObjects")]
public class GameObjectListVariable : ScriptableObject
{
    public List<GameObject> gos;

   public void AddGameObject(GameObject go) 
    {
        if(gos == null)
            gos = new List<GameObject>();
        gos.Add(go);
    }

    public List<GameObject> GetGos() { return gos; }
}
