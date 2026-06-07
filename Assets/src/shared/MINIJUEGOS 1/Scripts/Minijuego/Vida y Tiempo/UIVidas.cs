using UnityEngine;
using UnityEngine.UI;

public class UIVidas : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    [Header("Imágenes de corazones")]
    public Image[] corazones; // arrastra Corazon1, Corazon2, Corazon3

    void Update()
    {
        if (GameManager.Instance == null) return;
        ActualizarCorazones(GameManager.Instance.vidasActuales);
    }

    void ActualizarCorazones(int vidasActuales)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidasActuales)
                corazones[i].sprite = corazonLleno;
            else
                corazones[i].sprite = corazonVacio;
        }
    }
}