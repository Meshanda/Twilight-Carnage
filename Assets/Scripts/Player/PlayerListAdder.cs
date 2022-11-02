using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListAdder : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObjectListVariable _players;
    void Start()
    {
        _players.AddGameObject(gameObject);
    }
}
