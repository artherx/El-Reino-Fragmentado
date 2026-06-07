using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscenaDirecta : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribe exactamente el nombre de la escena a la que quieres ir.")]
    public string nombreDeLaEscena;

    // Esta es la función que llamarás desde tus botones o eventos
    public void CargarEscena()
    {
        // Verificamos que no hayas olvidado poner el nombre en el Inspector
        if (!string.IsNullOrEmpty(nombreDeLaEscena))
        {
            SceneManager.LoadScene(nombreDeLaEscena);
        }
    }
}