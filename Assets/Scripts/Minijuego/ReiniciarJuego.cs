using UnityEngine;
using UnityEngine.SceneManagement;

public class ReiniciarJuego : MonoBehaviour
{
    public void Reiniciar()
    {
        Time.timeScale = 1f;

        // Si existe GameManager, reinicia vidas y tiempo
        if (GameManager.Instance != null)
            GameManager.Instance.IniciarJuego();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}