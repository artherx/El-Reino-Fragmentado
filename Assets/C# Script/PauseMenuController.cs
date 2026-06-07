using UnityEngine;
using UnityEngine.InputSystem;   // Nuevo Input System
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelMenuPausa;   // Panel del menú de pausa
    public GameObject botonAbrirMenu;   // Botón que abre el menú

    [Header("Configuración del Minijuego")]
    [Tooltip("El tiempo en segundos al que volverá el reloj al reiniciar (ej: 120 para 2 minutos)")]
    public float tiempoInicialDelNivel = 60f;

    private bool isPaused = false;

    void Update()
    {
        // Detecta la tecla ESC con el nuevo Input System
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) CerrarMenu();
            else AbrirMenu();
        }
    }

    // --- Funciones de pausa ---
    public void AbrirMenu()
    {
        panelMenuPausa.SetActive(true);
        if (botonAbrirMenu != null) botonAbrirMenu.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;

        // Cursor libre para poder usar la UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CerrarMenu()
    {
        panelMenuPausa.SetActive(false);
        if (botonAbrirMenu != null) botonAbrirMenu.SetActive(true);

        Time.timeScale = 1f;
        isPaused = false;

        // Cursor bloqueado de nuevo para gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // --- Botones del menú ---
    public void Reanudar()
    {
        CerrarMenu();
    }

    public void Continuar()
    {
        CerrarMenu();
    }

    public void ReiniciarEscena()
    {
        // 1. Devolvemos el tiempo a la normalidad
        Time.timeScale = 1f;

        // 2. REINICIAMOS EL TIEMPO EN EL GAME MANAGER
        if (GameManager.Instance != null)
        {
            GameManager.Instance.tiempoRestante = tiempoInicialDelNivel;
        }

        // 3. Recargamos la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenuInicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}