using UnityEngine;
using UnityEngine.InputSystem;   // Nuevo Input System
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelMenuPausa;   // Panel del menú de pausa
    public GameObject botonAbrirMenu;   // Botón que abre el menú

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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenuInicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }
}
