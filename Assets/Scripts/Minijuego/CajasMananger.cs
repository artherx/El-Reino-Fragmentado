
using UnityEngine;

public class CajasMananger : MonoBehaviour
{
    private GameObject[] allBoxes;
    private Checkpoint checkpoint;
    public static bool todasRecogidas = false; // <- accesible desde PlayerRespawn

    void Start()
    {
        todasRecogidas = false;
        allBoxes = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            allBoxes[i] = transform.GetChild(i).gameObject;

        checkpoint = FindFirstObjectByType<Checkpoint>();
        if (checkpoint != null)
            checkpoint.DesactivarCheckpoint();
    }

    private void Update()
    {
        if (!todasRecogidas && transform.childCount == 0)
        {
            todasRecogidas = true;
            Debug.Log("¡Todas las cajas recogidas!");
            if (checkpoint != null)
                checkpoint.ActivarCheckpoint();
            enabled = false;
        }
    }

    public void ResetBoxes()
    {
        todasRecogidas = false;
        foreach (GameObject box in allBoxes)
        {
            if (box != null)
                box.SetActive(true);
        }
        if (checkpoint != null)
            checkpoint.DesactivarCheckpoint();

        enabled = true;
    }
}