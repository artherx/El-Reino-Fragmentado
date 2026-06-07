using System.Diagnostics;
using UnityEngine;

public class ResetEstadoEscena : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

    }
}