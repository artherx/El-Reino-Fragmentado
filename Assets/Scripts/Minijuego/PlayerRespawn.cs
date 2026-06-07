using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 spawnPoint;
    private Vector2 spawnInicial; // guarda el punto de inicio original
    private Vector2 lastSafePosition;
    private float safePositionTimer = 0f;
    public float safePositionInterval = 0.5f;
    public float respawnCooldown = 0.3f;
    private bool isRespawning = false;

    void Start()
    {
        spawnPoint = transform.position;
        spawnInicial = transform.position; // nunca cambia
        lastSafePosition = transform.position;
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
        spawnPoint = new Vector2(x, y);
    }

    // Llamado por GameManager cuando se agotan las vidas
    public void RespawnAlInicio()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        transform.position = spawnInicial;
        spawnPoint = spawnInicial;
        lastSafePosition = spawnInicial;
        isRespawning = false;
    }

    public bool Die()
    {
        if (isRespawning) return false;
        isRespawning = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Descuenta vida — si se agotan, GameManager llama RespawnAlInicio
        if (GameManager.Instance != null)
        {
            bool sinVidas = GameManager.Instance.PerderVida();
            if (sinVidas) return true;
        }

        // Tiene vidas restantes — solo reubica
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