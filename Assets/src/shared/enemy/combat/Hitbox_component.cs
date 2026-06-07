using UnityEngine;
using Shared;

namespace Shared.Combat
{
    [RequireComponent(typeof(Collider))]
    public class HitboxComponent : MonoBehaviour
    {
        private int damageAmount;
        private string targetTag;
        private Collider hitboxCollider;

        private void Awake()
        {
            hitboxCollider = GetComponent<Collider>();
            hitboxCollider.isTrigger = true;
            DeactivateHitbox();
        }

        public void Setup(int damage, string target)
        {
            damageAmount = damage;
            targetTag = target;
        }

        public void ActivateHitbox()
        {
            hitboxCollider.enabled = true;
        }

        public void DeactivateHitbox()
        {
            hitboxCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Verificamos si el objeto tiene el tag correcto (ej. "Player")
            if (other.CompareTag(targetTag))
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damageAmount);
                    // Opcional: Desactivar la hitbox tras el primer golpe para no hacer doble daño
                    DeactivateHitbox(); 
                }
            }
        }
    }
}