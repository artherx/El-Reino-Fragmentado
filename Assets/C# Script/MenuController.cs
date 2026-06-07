using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string nombreEscenaJuego = "EscenaPrueba";

    [Header("Tiempo para dejar sonar el click")]
    public float retrasoClick = 0.25f;

    public void IniciarPartida()
    {
        StartCoroutine(IniciarPartidaConDelay());
    }

    IEnumerator IniciarPartidaConDelay()
    {
        yield return new WaitForSecondsRealtime(retrasoClick);
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void SalirJuego()
    {
        StartCoroutine(SalirJuegoConDelay());
    }

    IEnumerator SalirJuegoConDelay()
    {
        yield return new WaitForSecondsRealtime(retrasoClick);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}