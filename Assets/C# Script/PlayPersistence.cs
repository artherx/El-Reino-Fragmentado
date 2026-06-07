using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    private static PlayerPersistence instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // mantiene al Player entre escenas
        }
        else
        {
            Destroy(gameObject); // evita duplicados si otra escena trae otro Player
        }
    }
}
