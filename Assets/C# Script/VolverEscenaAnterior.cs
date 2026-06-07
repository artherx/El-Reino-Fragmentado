using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverEscenaAnterior : MonoBehaviour
{
    public void Volver()
    {
        if (!string.IsNullOrEmpty(SceneTracker.instance.lastSceneName))
        {
            SceneManager.LoadScene(SceneTracker.instance.lastSceneName);
        }
        else
        {
            Debug.LogWarning("No hay escena anterior guardada.");
        }
    }
}
