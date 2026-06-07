using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker instance;

    [Header("Datos de progreso")]
    public string lastSceneName;
    public int currentLevel = 1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SaveCurrentScene()
    {
        lastSceneName = SceneManager.GetActiveScene().name;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }
}