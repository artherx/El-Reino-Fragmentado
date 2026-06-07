using UnityEngine;
using UnityEngine.EventSystems;

public class BotonSonidoUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [Header("Fuente de audio")]
    public AudioSource audioSource;

    [Header("Sonidos")]
    public AudioClip sonidoHover;
    public AudioClip sonidoClick;

    [Header("Volumen")]
    public float volumenHover = 0.35f;
    public float volumenClick = 0.55f;

    [Header("Evitar sonido repetido muy rápido")]
    public float tiempoMinimoEntreHover = 0.08f;

    private float ultimoHover = -999f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSource == null || sonidoHover == null) return;

        if (Time.unscaledTime - ultimoHover >= tiempoMinimoEntreHover)
        {
            audioSource.PlayOneShot(sonidoHover, volumenHover);
            ultimoHover = Time.unscaledTime;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (audioSource == null || sonidoClick == null) return;

        audioSource.PlayOneShot(sonidoClick, volumenClick);
    }
}