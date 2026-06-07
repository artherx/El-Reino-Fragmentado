using UnityEngine;
using System.Collections;

public class CajaCollected : MonoBehaviour
{
    private bool recogida = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recogida) return;
        if (collision.CompareTag("Player"))
        {
            RecogerCaja();
        }
    }

    private void RecogerCaja()
    {
        recogida = true;

        if (SoundManagerMiniJuego.Instance != null)
            SoundManagerMiniJuego.Instance.PlayCaja();

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (col != null)
            col.enabled = false;

        if (transform.childCount > 0)
        {
            GameObject efecto = transform.GetChild(0).gameObject;
            efecto.SetActive(true);
            StartCoroutine(DesactivarEfecto(efecto));
        }
    }

    private IEnumerator DesactivarEfecto(GameObject efecto)
    {
        // Obtiene la duración real de la animación
        Animator anim = efecto.GetComponent<Animator>();
        if (anim != null)
        {
            // Espera un frame para que el Animator inicialice el clip
            yield return null;
            float duracion = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(duracion);
        }
        else
        {
            // Si no tiene Animator, usa un tiempo fijo de seguridad
            yield return new WaitForSeconds(0.5f);
        }

        efecto.SetActive(false);
    }

    public void ResetCaja()
    {
        recogida = false;
        StopAllCoroutines(); // Cancela si el respawn ocurre durante la animación

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (col != null)
            col.enabled = true;

        if (transform.childCount > 0)
            transform.GetChild(0).gameObject.SetActive(false);
    }

    public bool EstaRecogida()
    {
        return recogida;
    }
}