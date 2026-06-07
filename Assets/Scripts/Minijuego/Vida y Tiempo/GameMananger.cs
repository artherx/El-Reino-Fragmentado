using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuración")]
    public int vidasMaximas = 3;
    public float tiempoLimite = 180f;
    public int escenaGameOver = 0;
    public int escenaInicioMinijuego = 1;

    [Header("Pantalla de derrota")]
    public GameObject pantallaDerrota;

    [HideInInspector] public int vidasActuales;
    [HideInInspector] public float tiempoRestante;

    private bool juegoActivo = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            IniciarJuego();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!juegoActivo) return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            PerderTodo();
        }
    }

    public void IniciarJuego()
    {
        vidasActuales = vidasMaximas;
        tiempoRestante = tiempoLimite;
        juegoActivo = true;

        if (pantallaDerrota != null)
            pantallaDerrota.SetActive(false);
    }

    public bool PerderVida()
    {
        if (!juegoActivo) return false;

        vidasActuales--;
        Debug.Log("Vidas restantes: " + vidasActuales);

        if (vidasActuales <= 0)
        {
            // Sin vidas — reinicia vidas y el nivel pero el tiempo sigue
            vidasActuales = vidasMaximas;
            ReiniciarNivel();
            return true;
        }
        return false;
    }

    private void ReiniciarNivel()
    {
        // Reinicia las cajas
        CajasMananger cajas = FindFirstObjectByType<CajasMananger>();
        if (cajas != null)
            cajas.ResetBoxes();

        // Reposiciona al jugador al spawn inicial
        PlayerRespawn respawn = FindFirstObjectByType<PlayerRespawn>();
        if (respawn != null)
            respawn.RespawnAlInicio();
    }

    public void PerderTodo()
    {
        if (!juegoActivo) return;
        juegoActivo = false;

        Time.timeScale = 0f;

        if (pantallaDerrota != null)
            pantallaDerrota.SetActive(true);
        else
        {
            Destroy(gameObject);
            SceneManager.LoadScene(escenaGameOver);
        }
    }

    public void GanarMinijuego()
    {
        juegoActivo = false;
        Destroy(gameObject);
    }
}