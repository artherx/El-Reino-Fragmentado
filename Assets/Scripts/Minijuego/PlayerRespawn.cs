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
        spawnPoint = transform.position;
        lastSafePosition = transform.position;

        if (PlayerPrefs.GetFloat("checkPointPositionX") != 0)
        {
            spawnPoint = new Vector2(
                PlayerPrefs.GetFloat("checkPointPositionX"),
                PlayerPrefs.GetFloat("checkPointPositionY")
            );

            transform.position = spawnPoint;
        }
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

        PlayerPrefs.SetFloat("checkPointPositionX", x);
        PlayerPrefs.SetFloat("checkPointPositionY", y);
    }

    public bool Die()
    {
        if (isRespawning) return false;

        isRespawning = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (CajasMananger.todasRecogidas)
        {
            transform.position = lastSafePosition;
        }
        else
        {
            transform.position = spawnPoint;

            CajasMananger cajas = FindFirstObjectByType<CajasMananger>();

            if (cajas != null)
                cajas.ResetBoxes();
        }

        Invoke(nameof(ActivarMuerteOtraVez), respawnCooldown);

        return true;
    }

    private void ActivarMuerteOtraVez()
    {
        isRespawning = false;
    }
}