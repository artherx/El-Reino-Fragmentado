using UnityEngine;

public class EnemySpike : MonoBehaviour
{
    private bool puedeHacerDaño = true;
    public float cooldownDaño = 0.3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!puedeHacerDaño) return;

        if (collision.transform.CompareTag("Player"))
        {
            puedeHacerDaño = false;

            if (SoundManagerMiniJuego.Instance != null)
                SoundManagerMiniJuego.Instance.PlaySpikeEnemy();

            PlayerRespawn respawn = collision.transform.GetComponent<PlayerRespawn>();

            if (respawn != null)
                respawn.Die();

            Invoke(nameof(ActivarDañoOtraVez), cooldownDaño);
        }
    }

    private void ActivarDañoOtraVez()
    {
        puedeHacerDaño = true;
    }
}