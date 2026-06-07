using UnityEngine;

public class CajasMananger : MonoBehaviour
{
    private CajaCollected[] allBoxes;
    private Checkpoint checkpoint;

    public static bool todasRecogidas = false;

    void Start()
    {
        todasRecogidas = false;

        allBoxes = GetComponentsInChildren<CajaCollected>(true);

        checkpoint = FindFirstObjectByType<Checkpoint>();

        if (checkpoint != null)
            checkpoint.DesactivarCheckpoint();
    }

    private void Update()
    {
        if (!todasRecogidas && TodasLasCajasRecogidas())
        {
            todasRecogidas = true;
            Debug.Log("¡Todas las cajas recogidas!");

            if (checkpoint != null)
                checkpoint.ActivarCheckpoint();

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

        enabled = true;
    }
}