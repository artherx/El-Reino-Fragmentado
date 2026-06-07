using UnityEngine;
using UnityEngine.EventSystems;

public class BotonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Vector3 escalaOriginal;
    private Vector2 posicionOriginal;

    [Header("Efecto Hover")]
    public float escalaHover = 1.08f;
    public float movimientoY = 8f;
    public float velocidad = 10f;

    private Vector3 escalaObjetivo;
    private Vector2 posicionObjetivo;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        escalaOriginal = rectTransform.localScale;
        posicionOriginal = rectTransform.anchoredPosition;

        escalaObjetivo = escalaOriginal;
        posicionObjetivo = posicionOriginal;
    }

    void Update()
    {
        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            escalaObjetivo,
            Time.deltaTime * velocidad
        );

        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            posicionObjetivo,
            Time.deltaTime * velocidad
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal * escalaHover;
        posicionObjetivo = posicionOriginal + new Vector2(0, movimientoY);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaObjetivo = escalaOriginal;
        posicionObjetivo = posicionOriginal;
    }
}