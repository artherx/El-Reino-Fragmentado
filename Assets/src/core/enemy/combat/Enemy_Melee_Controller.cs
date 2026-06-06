using UnityEngine;
using Shared.Combat;

namespace Core.Combat
{
    public class EnemyMeleeController : EnemyController
    {
        [Header("Melee References")]
        [SerializeField] private Animator animator;
        [SerializeField] private HitboxComponent hitbox;
        [SerializeField] private string targetTag = "Player";
        
        private EnemyMeleeDataSO meleeData;
        private Transform target;
        private Vector3 startPosition;
        private Vector3 currentPatrolDestination;
        
        private float stateTimer;

        protected override void Awake()
        {
            base.Awake();
            if (animator == null) animator = GetComponentInChildren<Animator>();
        }

        protected override void Start()
        {
            base.Start();
            
            meleeData = enemyData as EnemyMeleeDataSO;
            if (meleeData == null)
            {
                Debug.LogError("EnemyData no es de tipo EnemyMeleeDataSO!");
                return;
            }

            // Buscar al jugador (idealmente a través de un GameManager o un Singleton, pero usamos Tag por simplicidad inicial)
            GameObject playerObj = GameObject.FindGameObjectWithTag(targetTag);
            if (playerObj != null) target = playerObj.transform;

            startPosition = transform.position;
            hitbox.Setup(meleeData.damage, targetTag);
            
            PickNewPatrolDestination();
        }

        // --- FSM IMPLEMENTATION ---

        protected override void UpdateIdle()
        {
            animator.SetBool("IsWalking", false);
            
            stateTimer -= Time.deltaTime;
            
            if (CheckForPlayer()) return;

            if (stateTimer <= 0)
            {
                PickNewPatrolDestination();
                ChangeState(EnemyState.Patrol);
            }
        }

        protected override void UpdatePatrol()
        {
            animator.SetBool("IsWalking", true);

            if (CheckForPlayer()) return;

            // Moverse hacia el destino (Usa NavMeshAgent aquí si es 3D, o Vector2.MoveTowards si es 2D)
            transform.position = Vector3.MoveTowards(transform.position, currentPatrolDestination, meleeData.speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentPatrolDestination) < 0.1f)
            {
                stateTimer = meleeData.patrolWaitTime;
                ChangeState(EnemyState.Idle);
            }
        }

        protected override void UpdateChase()
        {
            animator.SetBool("IsWalking", true);

            if (target == null)
            {
                ChangeState(EnemyState.Idle);
                return;
            }

            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            if (distanceToPlayer <= meleeData.attackRange)
            {
                ChangeState(EnemyState.Attack);
                return;
            }

            if (distanceToPlayer > meleeData.detectionRange * 1.5f) // Pierde el aggro con un poco de margen
            {
                ChangeState(EnemyState.Idle);
                return;
            }

            // Moverse hacia el jugador
            transform.position = Vector3.MoveTowards(transform.position, target.position, meleeData.speed * Time.deltaTime);
            LookAtTarget(target.position);
        }

        protected override void UpdateAttack()
        {
            animator.SetBool("IsWalking", false);
            
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0)
            {
                // Iniciar ataque
                animator.SetTrigger("Attack");
                stateTimer = meleeData.attackCooldown;
                // Nota: La activación de la hitbox se hace típicamente vía Animation Events
            }
            else if (stateTimer <= meleeData.attackCooldown - 0.5f) // Si ya terminó la animación base, verifica si sigue en rango
            {
                float distanceToPlayer = Vector3.Distance(transform.position, target.position);
                if (distanceToPlayer > meleeData.attackRange)
                {
                    ChangeState(EnemyState.Chase);
                }
            }
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
            animator.SetTrigger("Die");
            hitbox.DeactivateHitbox();
            
            // Desactivar físicas o componentes de movimiento
            if (TryGetComponent(out Collider col)) col.enabled = false;
            
            this.enabled = false; // Desactivar el script
        }

        // --- UTILS ---

        private bool CheckForPlayer()
        {
            if (target == null) return false;

            if (Vector3.Distance(transform.position, target.position) <= meleeData.detectionRange)
            {
                ChangeState(EnemyState.Chase);
                return true;
            }
            return false;
        }

        private void PickNewPatrolDestination()
        {
            // Crea un punto aleatorio en un radio (Para 2D usa Random.insideUnitCircle)
            Vector2 randomDir = Random.insideUnitCircle * meleeData.patrolRadius;
            currentPatrolDestination = startPosition + new Vector3(randomDir.x, 0, randomDir.y);
        }

        private void LookAtTarget(Vector3 lookPos)
        {
            // Gira el modelo hacia el target. 
            // Si es 2D: cambia el localScale.x. Si es 3D: usa transform.LookAt
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (enemyData is EnemyMeleeDataSO meleeDataSO)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(Application.isPlaying ? startPosition : transform.position, meleeDataSO.patrolRadius);
                
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, meleeDataSO.attackRange);
            }
        }
    }
}