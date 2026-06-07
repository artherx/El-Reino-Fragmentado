using UnityEngine;

namespace Core.Combat
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Enemy Data")]
    public class EnemyDataSO : ScriptableObject
    {
        [Header("Stats Base")]
        public int maxHP = 100;
        public float speed = 3f;
        public float detectionRange = 10f;
        public int damage = 10;
    }
}