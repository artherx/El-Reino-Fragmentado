using UnityEngine;
using TMPro;

public class UIJuego : MonoBehaviour
{
    [Header("Tiempo")]
    public TextMeshProUGUI textoTiempo;

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (textoTiempo == null) return;

        float t = GameManager.Instance.tiempoRestante;
        int minutos = Mathf.FloorToInt(t / 60f);
        int segundos = Mathf.FloorToInt(t % 60f);
        textoTiempo.text = "Tiempo: " + minutos + ":" + segundos.ToString("00");
    }
}