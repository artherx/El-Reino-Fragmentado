using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaCollider : MonoBehaviour
{
    public string nombreEscena;
    public int nivelAlEntrar = 3; // al pasar el primer portal, se queda en 3

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Guardar la escena actual
            SceneTracker.instance.SaveCurrentScene();

            // Actualizar el nivel
            SceneTracker.instance.SetLevel(nivelAlEntrar);

            Debug.Log("Entró al portal. Nivel fijado en " + nivelAlEntrar);

            // Cambiar de escena
            SceneManager.LoadScene(nombreEscena);
        }
    }
}
