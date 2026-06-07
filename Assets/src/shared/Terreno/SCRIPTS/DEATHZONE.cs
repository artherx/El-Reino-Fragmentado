using UnityEngine;

public class ResetCharacterZones : MonoBehaviour
{
    [Header("Jugador")]
    public GameObject player;          // Arrastra aquí tu personaje
    public Transform respawnPoint;     // Punto de inicio o respawn

    [Header("Zonas prohibidas")]
    public Collider[] forbiddenZones;  // Arrastra aquí los colliders planos (AGUA, VACIO TUTORIAL, etc.)

    private Vector3 initialPosition;

    void Start()
    {
        // Si no asignas un respawnPoint, usa la posición inicial del jugador
        if (respawnPoint == null)
            initialPosition = player.transform.position;
        else
            initialPosition = respawnPoint.position;
    }

    void Update()
    {
        // Revisamos constantemente si el jugador está tocando alguna zona prohibida
        foreach (Collider zone in forbiddenZones)
        {
            if (zone.bounds.Intersects(player.GetComponent<Collider>().bounds))
            {
                ResetPlayer();
                break;
            }
        }
    }

    private void ResetPlayer()
    {
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
