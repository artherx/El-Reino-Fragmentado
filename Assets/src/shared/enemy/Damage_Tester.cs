using UnityEngine;
using UnityEngine.InputSystem;
using Shared;

public class DamageTester : MonoBehaviour
{
    [Header("Configuración de Prueba")]
    public GameObject enemyTarget;
    public int damageToApply = 25;

    void Update()
    {
        // Presiona la barra espaciadora para hacer daño
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (enemyTarget != null)
            {
                // Buscamos la interfaz IDamageable
                if (enemyTarget.TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log($"[Tester] Aplicando {damageToApply} de daño...");
                    damageable.TakeDamage(damageToApply);
                }
                else
                {
                    Debug.LogWarning("[Tester] El objetivo no tiene un componente que implemente IDamageable.");
                }
            }
        }
    }
}