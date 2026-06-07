using UnityEngine;
public class PlayerRespawn : MonoBehaviour
{
    private Vector2 spawnPoint;
    private Vector2 lastSafePosition;
    private float safePositionTimer = 0f;
    public float safePositionInterval = 0.5f;
    public float respawnCooldown = 0.3f;
    private bool isRespawning = false;

    void Start()
    {
        // Siempre usa la posición del GameObject en la escena
        spawnPoint = transform.position;
        lastSafePosition = transform.position;

        // Limpia checkpoints viejos de escenas anteriores
        PlayerPrefs.DeleteKey("checkPointPositionX");
        PlayerPrefs.DeleteKey("checkPointPositionY");
    }

    void Update()
    {
        safePositionTimer += Time.deltaTime;
        if (safePositionTimer >= safePositionInterval && CheckGround.isGrounded)
        {
            lastSafePosition = transform.position;
            safePositionTimer = 0f;
        }
    }

    public void ReachedCheckPoint(float x, float y)
    {
        // Guarda el checkpoint solo en memoria, sin PlayerPrefs
        spawnPoint = new Vector2(x, y);
    }

    public bool Die()
    {
        if (isRespawning) return false;
        isRespawning = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (CajasMananger.todasRecogidas)
            transform.position = lastSafePosition;
        else
            transform.position = spawnPoint;

        Invoke(nameof(ActivarMuerteOtraVez), respawnCooldown);
        return true;
    }

    private void ActivarMuerteOtraVez()
    {
        isRespawning = false;
    }
}