using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Effect")]
public class EffectSO : ScriptableObject
{
    public int Damage = 0;
    public int NbEnemyTouch = 0;
    public float BulletSpeed = 0;
    
    public int NbShoot = 0;
}
