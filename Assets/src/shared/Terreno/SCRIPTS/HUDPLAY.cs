using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Si tienes un panel de UI para mostrar

public class PausaManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel; // Arrastra aquí tu panel de pausa en el Inspector

    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Reanudar();
            else Pausar();
        }
    }

    void Pausar()
    {
        Time.timeScale = 0f;   // Congela todo: físicas, animaciones, corrutinas
        isPaused = true;

        if (pausePanel != null) pausePanel.SetActive(true);

        // Opcional: liberar el cursor si lo tienes bloqueado
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Reanudar()
    {
        Time.timeScale = 1f;   // Reanuda todo
        isPaused = false;

        if (pausePanel != null) pausePanel.SetActive(false);

        // Opcional: volver a bloquear el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}