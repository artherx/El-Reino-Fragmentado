// PlayerRespawn.cs
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 spawnPoint;
    private Vector2 lastSafePosition;
    private float safePositionTimer = 0f;
    public float safePositionInterval = 0.5f; // Guarda posición segura cada 0.5 segundos

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
        // Guarda la posición segura cada X segundos si está en el suelo
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

    public void Die()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (CajasMananger.todasRecogidas)
        {
            // Reaparece en la última posición segura (antes de las púas)
            transform.position = lastSafePosition;
        }
        else
        {
            // Normal: vuelve al spawn y resetea cajas
            transform.position = spawnPoint;
            CajasMananger cajas = FindFirstObjectByType<CajasMananger>();
            if (cajas != null)
                cajas.ResetBoxes();
        }
    }
}