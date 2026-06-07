using UnityEngine;

namespace Core.Combat
{
    [CreateAssetMenu(fileName = "NewEnemyMeleeData", menuName = "Game/Enemy Data/Melee")]
    public class EnemyMeleeDataSO : EnemyDataSO
    {
        [Header("Melee Specifics")]
        public float attackRange = 1.5f;
        public float attackCooldown = 2f;
        public float patrolRadius = 5f;
        public float patrolWaitTime = 2f;
    }
}