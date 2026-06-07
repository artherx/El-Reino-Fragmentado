using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CambiarEscenaDirecta : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribe exactamente el nombre de la escena a la que quieres ir.")]
    public string nombreDeLaEscena;

    public void CargarEscena()
    {
        if (string.IsNullOrEmpty(nombreDeLaEscena))
        {
            return;
        }

        // Reinicia el tiempo por si venías de pausa, derrota, diálogo, etc.
        Time.timeScale = 1f;

        // Reactiva audio por si algún menú lo pausó
        AudioListener.pause = false;

        // Limpia selección de botones UI
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }


        SceneManager.LoadScene(nombreDeLaEscena, LoadSceneMode.Single);
    }
}