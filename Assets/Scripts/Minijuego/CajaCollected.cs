using UnityEngine;

public class CajaCollected : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false; // Oculta la caja
            gameObject.transform.GetChild(0).gameObject.SetActive(true); // Activa el hijo (partículas)

            Destroy(gameObject, 0.5f); // Destruye la caja después de 0.5 segundos


        }
    }
}
