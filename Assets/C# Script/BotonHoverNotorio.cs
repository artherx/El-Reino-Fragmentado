using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotonHoverNotorio : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Image imagen;

    private Vector3 escalaOriginal;
    private Vector2 posicionOriginal;
    private Quaternion rotacionOriginal;
    private Color colorOriginal;

    [Header("Efecto al pasar el mouse")]
    public float escalaHover = 1.18f;
    public float movimientoX = 22f;
    public float movimientoY = 4f;
    public float rotacionHover = -1.5f;

    [Header("Pulso mientras está seleccionado")]
    public bool usarPulso = true;
    public float intensidadPulso = 0.035f;
    public float velocidadPulso = 8f;

    [Header("Efecto al hacer clic")]
    public float escalaClick = 0.92f;
    public float movimientoClickY = -5f;

    [Header("Suavidad")]
    public float velocidad = 16f;

    private bool mouseEncima = false;
    private bool presionado = false;

    private Vector3 escalaObjetivo;
    private Vector2 posicionObjetivo;
    private Quaternion rotacionObjetivo;
    private Color colorObjetivo;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        imagen = GetComponent<Image>();

        escalaOriginal = rectTransform.localScale;
        posicionOriginal = rectTransform.anchoredPosition;
        rotacionOriginal = rectTransform.localRotation;

        if (imagen != null)
            colorOriginal = imagen.color;

        escalaObjetivo = escalaOriginal;
        posicionObjetivo = posicionOriginal;
        rotacionObjetivo = rotacionOriginal;
        colorObjetivo = colorOriginal;
    }

    void Update()
    {
        float t = Time.unscaledDeltaTime * velocidad;

        Vector3 escalaFinal = escalaObjetivo;

        if (mouseEncima && usarPulso && !presionado)
        {
            float pulso = Mathf.Sin(Time.unscaledTime * velocidadPulso) * intensidadPulso;
            escalaFinal += Vector3.one * pulso;
        }

        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, escalaFinal, t);
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, posicionObjetivo, t);
        rectTransform.localRotation = Quaternion.Lerp(rectTransform.localRotation, rotacionObjetivo, t);

        if (imagen != null)
            imagen.color = Color.Lerp(imagen.color, colorObjetivo, t);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEncima = true;
        presionado = false;

        escalaObjetivo = escalaOriginal * escalaHover;
        posicionObjetivo = posicionOriginal + new Vector2(movimientoX, movimientoY);
        rotacionObjetivo = Quaternion.Euler(0, 0, rotacionHover);

        if (imagen != null)
            colorObjetivo = new Color(1f, 0.92f, 0.72f, colorOriginal.a);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEncima = false;
        presionado = false;

        escalaObjetivo = escalaOriginal;
        posicionObjetivo = posicionOriginal;
        rotacionObjetivo = rotacionOriginal;
        colorObjetivo = colorOriginal;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        presionado = true;

        escalaObjetivo = escalaOriginal * escalaClick;
        posicionObjetivo = posicionOriginal + new Vector2(0, movimientoClickY);
        rotacionObjetivo = Quaternion.Euler(0, 0, 1.5f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        presionado = false;

        if (mouseEncima)
        {
            escalaObjetivo = escalaOriginal * escalaHover;
            posicionObjetivo = posicionOriginal + new Vector2(movimientoX, movimientoY);
            rotacionObjetivo = Quaternion.Euler(0, 0, rotacionHover);
        }
        else
        {
            escalaObjetivo = escalaOriginal;
            posicionObjetivo = posicionOriginal;
            rotacionObjetivo = rotacionOriginal;
        }
    }

    void OnDisable()
    {
        if (rectTransform != null)
        {
            rectTransform.localScale = escalaOriginal;
            rectTransform.anchoredPosition = posicionOriginal;
            rectTransform.localRotation = rotacionOriginal;
        }

        if (imagen != null)
            imagen.color = colorOriginal;
    }
}
