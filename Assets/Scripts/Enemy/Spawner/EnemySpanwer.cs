using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    [SerializeField] private GameObject EnemyToSpawn;
    [SerializeField] private float TimeBetweenSpawn;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();

            StartSpawning();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    void StartSpawning()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("StartSpawn"))
            {
                StopCoroutine(SpawnEnemy());
                StartCoroutine(SpawnEnemy());
            }
        }
    }
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject Enemy = Instantiate(EnemyToSpawn, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            Enemy.GetComponent<NetworkObject>().Spawn();
            yield return new WaitForSeconds(TimeBetweenSpawn);
        }
    }
}
