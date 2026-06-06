using UnityEngine;

public class DamageObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerRespawn respawn = collision.transform.GetComponent<PlayerRespawn>();
            if (respawn != null)
                respawn.Die();
        }
    }
}