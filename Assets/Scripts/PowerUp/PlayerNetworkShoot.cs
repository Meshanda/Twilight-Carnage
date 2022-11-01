using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkShoot : NetworkBehaviour
{
    [SerializeField] private Transform _bulletPrefab;

    private BulletManager _bulletManager;

    private void Start()
    {
        _bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootServerRpc();
        }
    }


    [ServerRpc]
    private void ShootServerRpc()
    {
        Transform spawnedObject = Instantiate(_bulletPrefab, transform.position + Vector3.forward, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);

        Bullet bullet = spawnedObject.GetComponent<Bullet>();
        
        // Set Bullet specs ...
        bullet.Life = 1;
        bullet.Speed = 10;
        
        _bulletManager.AddBullet( bullet );
        
    }
}
