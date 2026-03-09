using Code.Turrets.Bullets;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "SO/Enemy/EnemyDataSO", order = 0)]
public class EnemyDataSO : ScriptableObject
{
    public string enemyName;
    public EnemyType enemyType;
    public int maxHp;
    public float moveSpeed;
    public float escapeSpeed;
    public int stealValue;
    public float spawnInterval;

    [Header("Weakness Settings")]
    public BulletType weaknessBullet;           // 약점인 총알 타입
    public float weaknessMultiplier = 1.5f;     // 약점 데미지 배수
}
