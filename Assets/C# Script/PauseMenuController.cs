using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Objetos del menú")]
    public GameObject panelMenuPausa;
    public GameObject botonAbrirMenu;

    public void AbrirMenu()
    {
        panelMenuPausa.SetActive(true);
        botonAbrirMenu.SetActive(false);

        // Esto pausa el tiempo del juego
        Time.timeScale = 0f;
    }

    public void CerrarMenu()
    {
        panelMenuPausa.SetActive(false);
        botonAbrirMenu.SetActive(true);

        // Esto reanuda el tiempo del juego
        Time.timeScale = 1f;
    }

    public void SalirAlMenuInicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuInicio");
    }

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
}