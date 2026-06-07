using UnityEngine;

public class CajasMananger : MonoBehaviour
{
    private CajaCollected[] allBoxes;
    private Checkpoint checkpoint;
    public static bool todasRecogidas = false;

    [Header("Panel de victoria")]
    public GameObject panelVictoria; // Arrastra aquí el Panel del Canvas en el Inspector

    void Start()
    {
        todasRecogidas = false;
        allBoxes = GetComponentsInChildren<CajaCollected>(true);
        checkpoint = FindFirstObjectByType<Checkpoint>();

        if (checkpoint != null)
            checkpoint.DesactivarCheckpoint();

        // Asegura que el panel esté oculto al inicio
        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    private void Update()
    {
        if (!todasRecogidas && TodasLasCajasRecogidas())
        {
            todasRecogidas = true;
            Debug.Log("¡Todas las cajas recogidas!");

            if (checkpoint != null)
                checkpoint.ActivarCheckpoint();

            // Muestra el panel de victoria
            if (panelVictoria != null)
                panelVictoria.SetActive(true);

            enabled = false;
        }
    }

    private bool TodasLasCajasRecogidas()
    {
        foreach (CajaCollected caja in allBoxes)
        {
            if (caja != null && !caja.EstaRecogida())
                return false;
        }
        return true;
    }

    public void ResetBoxes()
    {
        todasRecogidas = false;

        foreach (CajaCollected caja in allBoxes)
        {
            if (caja != null)
                caja.ResetCaja();
        }

        if (checkpoint != null)
            checkpoint.DesactivarCheckpoint();

        // Oculta el panel si se reinicia
        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        enabled = true;
    }
}