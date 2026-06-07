using UnityEngine;

public class ResetCharacterZones : MonoBehaviour
{
    [Header("Jugador")]
    public GameObject player;          // Arrastra aquí tu personaje
    public Transform respawnPoint;     // Punto de inicio o respawn

    [Header("Zonas prohibidas")]
    public Collider[] forbiddenZones;  // Arrastra aquí los colliders planos

    private Vector3 initialPosition;
    private Collider playerCollider;

    void Start()
    {
        // Guardamos la referencia al collider desde el inicio para no usar GetComponent en el Update
        if (player != null)
        {
            playerCollider = player.GetComponent<Collider>();
        }

        // Si no asignas un respawnPoint, usa la posición inicial del jugador
        if (respawnPoint == null && player != null)
            initialPosition = player.transform.position;
        else if (respawnPoint != null)
            initialPosition = respawnPoint.position;
    }

    void Update()
    {
        // CONTROL DE SEGURIDAD 1: Si el jugador ya no existe (por cambio de escena), salimos del Update
        if (player == null || playerCollider == null) return;

        // Revisamos constantemente si el jugador está tocando alguna zona prohibida
        foreach (Collider zone in forbiddenZones)
        {
            // CONTROL DE SEGURIDAD 2: Si la zona fue destruida o está vacía, la ignoramos
            if (zone == null) continue;

            if (zone.bounds.Intersects(playerCollider.bounds))
            {
                ResetPlayer();
                break; // Salimos del bucle si ya colisionó con una
            }
        }
    }

    private void ResetPlayer()
    {
        // Control extra por seguridad antes de moverlo
        if (player == null) return;

        // Reinicia posición y rotación
        player.transform.position = initialPosition;
        player.transform.rotation = Quaternion.identity;

        // Si tiene Rigidbody, también reinicia su velocidad
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}