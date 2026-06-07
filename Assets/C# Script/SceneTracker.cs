using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker instance;

    [Header("Jugador persistente")]
    public GameObject player;

    [Header("Datos de progreso")]
    public string lastSceneName;
    public int currentLevel = 1; // nivel inicial

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (player != null)
            {
                DontDestroyOnLoad(player);
            }

            // Suscribirse al evento de carga de escena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCurrentScene()
    {
        lastSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Escena guardada: " + lastSceneName);
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        Debug.Log("🔄 Nivel actualizado a: " + currentLevel);
    }

    // Se ejecuta cada vez que se carga una nueva escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada: " + scene.name);

        // Reasignar automáticamente el Player persistente
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                DontDestroyOnLoad(player);
                Debug.Log("✅ Player reasignado automáticamente en la nueva escena.");
            }
            else
            {
                Debug.LogWarning("⚠️ No se encontró un objeto con Tag 'Player' en la nueva escena.");
            }
        }
    }
}
