using System;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerShootData")]
public class PlayerShootData : ScriptableObject
{
    [Header("Bullet Data")] 
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _nbEnemyTouch = 1;
    [SerializeField] private float _bulletSpeed = 10;
    [Header("Shoot Data")]
    [SerializeField] private int _nbShoot = 1;

    // Bullet data
    public int Damage { get => _damage; private set => _damage = value; }
    public int NbEnemyTouch { get => _nbEnemyTouch; private set => _nbEnemyTouch = value; } // == bullet life
    public float BulletSpeed { get => _bulletSpeed; private set => _bulletSpeed = value; }

    // Shoot data
    public int NbShoot { get => _nbShoot; private set => _nbShoot = value; }

    public enum ShootEnum
    {
        Damage, NbEnemyTouch, BulletSpeed, NbShoot
    }

    public struct ShootRPC : INetworkSerializable
    {
        public int Damage;
        public int NbEnemyTouch;
        public float Speed;
        public int NbShoot;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Damage);
            serializer.SerializeValue(ref NbEnemyTouch);
            serializer.SerializeValue(ref Speed);
            
            serializer.SerializeValue(ref NbShoot);
        }
    }
    
    public void ApplyEffect(IntEffect effect)
    {
        switch (effect.Type)
        {
            case ShootEnum.Damage:
                Damage += effect.Value;
                break;
            case ShootEnum.NbEnemyTouch:
                NbEnemyTouch += effect.Value;
                break;
            case ShootEnum.NbShoot:
                NbShoot += effect.Value;
                break;
            default:
                Debug.Log("Type (int) : " + effect.Type + " is not recognize");
                break;
        }
    }
    
    public void ApplyEffect(FloatEffect effect)
    {
        switch (effect.Type)
        {
            case ShootEnum.BulletSpeed:
                BulletSpeed += effect.Value;
                break;
            default:
                Debug.Log("Type (float) : " + effect.Type + " is not recognize");
                break;
        }
    }

    public ShootRPC ToStruct()
    {
        return new ShootRPC()
        {
            Damage = Damage,
            Speed = BulletSpeed,
            NbEnemyTouch = NbEnemyTouch,
            NbShoot = NbShoot
        };
    }
}
