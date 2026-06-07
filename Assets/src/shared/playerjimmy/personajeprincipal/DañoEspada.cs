using UnityEngine;

public class DañoEspada : MonoBehaviour
{
    private Controles playerControls;

    void Start()
    {
        playerControls = GetComponentInParent<Controles>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && playerControls != null && playerControls.IsAttacking)
        {
            UnityEngine.Debug.Log("💥 ¡La espada cortó al Enemy!");

        }
    }
}
