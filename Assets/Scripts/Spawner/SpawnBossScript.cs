using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnBossScript : MonoBehaviour
{
    [SerializeField] GameObject _bossPrefab;

    public void SpawnBoss(Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject gameObject = Instantiate(_bossPrefab, position, rotation, parent);
        gameObject.GetComponent<NetworkObject>().Spawn();
    }
}
