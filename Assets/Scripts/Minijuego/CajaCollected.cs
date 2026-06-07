using UnityEngine;

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
            transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResetCaja()
    {
        recogida = false;

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