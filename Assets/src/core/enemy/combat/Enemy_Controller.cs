using UnityEngine;
using Shared;
using Shared.Events;

namespace Core.Combat
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Death
    }

    [RequireComponent(typeof(HealthComponent))]
    public class EnemyController : MonoBehaviour, IDamageable
    {
        [Header("Configuration")]
        [SerializeField] protected EnemyDataSO enemyData;
        [SerializeField] protected GameEventSO onEnemyDeathEvent;
        
        protected HealthComponent healthComponent;
        protected EnemyState currentState;

        protected virtual void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
        }

        protected virtual void Start()
        {
            // Inicializar vida usando los datos del SO
            if (enemyData != null)
            {
                healthComponent.Initialize(enemyData.maxHP);
            }
            else
            {
                Debug.LogWarning($"EnemyDataSO no asignado en {gameObject.name}");
            }

            // Suscribirse al evento de muerte local
            healthComponent.OnDeath += HandleDeath;
            
            ChangeState(EnemyState.Idle);
        }

        protected virtual void Update()
        {
            if (currentState == EnemyState.Death) return;

            // FSM - Update Loop
            switch (currentState)
            {
                case EnemyState.Idle:
                    UpdateIdle();
                    break;
                case EnemyState.Patrol:
                    UpdatePatrol();
                    break;
                case EnemyState.Chase:
                    UpdateChase();
                    break;
                case EnemyState.Attack:
                    UpdateAttack();
                    break;
            }
        }

        protected virtual void OnDestroy()
        {
            if (healthComponent != null)
            {
                healthComponent.OnDeath -= HandleDeath;
            }
        }

        // --- MÁQUINA DE ESTADOS (FSM) ---

        protected void ChangeState(EnemyState newState)
        {
            if (currentState == EnemyState.Death) return;
            
            currentState = newState;
            // Aquí puedes añadir lógica de "EnterState" si es necesario
        }

        // Métodos virtuales para que los enemigos específicos (ej. MeleeEnemy, RangedEnemy) los sobrescriban
        protected virtual void UpdateIdle() { }
        protected virtual void UpdatePatrol() { }
        protected virtual void UpdateChase() { }
        protected virtual void UpdateAttack() { }

        // --- IMPLEMENTACIÓN DE IDAMAGEABLE ---

        public virtual void TakeDamage(int amount)
        {
            if (currentState == EnemyState.Death) return;
            
            healthComponent.ApplyDamage(amount);
            // Aquí podrías cambiar el estado a Chase si el enemigo es atacado por la espalda, por ejemplo.
        }

        // --- MANEJO DE MUERTE ---

        protected virtual void HandleDeath()
        {
            ChangeState(EnemyState.Death);
            
            // Disparar el evento global (GameEventSO)
            if (onEnemyDeathEvent != null)
            {
                onEnemyDeathEvent.Raise();
            }

            // Lógica base de muerte (desactivar colliders, reproducir animación, destruir objeto, etc.)
            Debug.Log($"{gameObject.name} ha muerto.");
            
            // Opcional: Destruir el objeto después de un delay
            // Destroy(gameObject, 2f); 
        }

        // Utilidad para visualizar el rango de detección en el Editor
        protected virtual void OnDrawGizmosSelected()
        {
            if (enemyData != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, enemyData.detectionRange);
            }
        }
    }
}