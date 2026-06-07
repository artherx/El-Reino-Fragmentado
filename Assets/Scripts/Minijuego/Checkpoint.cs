using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    private Collider2D col;
    private bool flagActivated = false;

    [Header("Pantalla de Ganaste")]
    public GameObject pantallaGanaste;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        DesactivarCheckpoint();
    }

    public void ActivarCheckpoint()
    {
        if (col == null) return;
        col.enabled = true;
        flagActivated = false;
    }

    public void DesactivarCheckpoint()
    {
        if (col == null) return;
        col.enabled = false;
        flagActivated = false;
        if (anim != null)
            anim.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || flagActivated) return;
        flagActivated = true;

        PlayerRespawn respawn = collision.GetComponent<PlayerRespawn>();
        if (respawn != null)
            respawn.ReachedCheckPoint(transform.position.x, transform.position.y);

        if (anim != null)
            anim.enabled = true;

        StartCoroutine(WinRoutine());
    }

    private IEnumerator WinRoutine()
    {
        yield return new WaitForSeconds(4.5f);

        // Avisa al GameManager que el minijuego fue completado
        if (GameManager.Instance != null)
            GameManager.Instance.GanarMinijuego();

        if (SoundManagerMiniJuego.Instance != null)
            SoundManagerMiniJuego.Instance.PlayGanar();

        Time.timeScale = 0f;

        if (pantallaGanaste != null)
            pantallaGanaste.SetActive(true);
    }
}