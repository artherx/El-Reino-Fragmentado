using UnityEngine;

public class MirarCamara : MonoBehaviour
{
    private Transform camara;

    void Start()
    {
        camara = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (camara == null) return;

        // El panel siempre mira hacia la cámara
        transform.position = camara.position + camara.forward * 2f;
        transform.rotation = camara.rotation;
    }
}