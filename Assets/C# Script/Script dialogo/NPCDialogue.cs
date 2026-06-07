using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class NPCGranMago : MonoBehaviour
{
    [Header("UI Diálogo")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoDialogo;
    public TextMeshProUGUI textoNombre;

    [Header("UI Interacción")]
    public GameObject hintE;
    public TextMeshProUGUI hintText;

    [Header("Configuración")]
    public float velocidadTexto = 0.03f;

    [TextArea(2, 4)]
    public string[] dialogos = new string[]
    {
        "Guerrero… al fin has llegado.",
        "He visto el futuro, y no trae buenas noticias para nuestro reino.",
        "Una gran tormenta viene. Esta desruirá todo lo que está hecho de cartón.",
        "Incluso el castillo desaparecerá si no actuamos pronto.",
        "Pero aún existe una esperanza: la antigua Cápsula Protectora.",
        "Si logramos activarla, cubrirá el reino y lo protegerá de la tormenta.",
        "Para hacerlo, primero debes explorar este mapa y encontrar tres objetos.",
        "Luego ve a los portales mágicos. Cada uno te llevará a un mundo diferente.",
        "Dentro de estos tendrás que recolectar todos los objetos antes de salir.",
        "Ten cuidado con las trampas… y no pierdas demasiado tiempo.",
        "Si fallas, la tormenta llegará y el Reino de Cartón será destruido.",
        "Ve, guerrero. Cumple la profecía y salva nuestro mundo."
    };

    private bool jugadorDentro = false;
    private bool dialogoAbierto = false;
    private bool yaHablo = false;
    private bool escribiendo = false;
    private int indiceDialogo = 0;
    private bool botonPresionadoAntes = false;
    private Coroutine coroutineEscribir;

    void Start()
    {
        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (textoNombre != null) textoNombre.text = "Gran Mago";
        if (hintE != null)
        {
            hintE.SetActive(false);
            if (hintText != null)
                hintText.text = "E - Hablar";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") &&
            !other.transform.root.CompareTag("Player")) return;

        jugadorDentro = true;

        if (hintE != null) 
            hintE.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") &&
            !other.transform.root.CompareTag("Player")) return;

        jugadorDentro = false;

        if (dialogoAbierto)
            CerrarDialogo();

        if (hintE != null)
            hintE.SetActive(false);

        Debug.Log("Jugador salió del rango");
    }

    void Update()
    {
        bool botonAhora = false;
        if (Keyboard.current != null && Keyboard.current.eKey.isPressed)
            botonAhora = true;

        // Click izquierdo del mouse
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            botonAhora = true;

        bool botonPresionadoAhora = botonAhora && !botonPresionadoAntes;

        if (botonPresionadoAhora)
        {
            if (jugadorDentro && !dialogoAbierto && !yaHablo)
                AbrirDialogo();
            else if (dialogoAbierto)
            {
                if (escribiendo)
                    MostrarTextoCompleto();
                else
                    SiguienteDialogo();
            }
        }

        botonPresionadoAntes = botonAhora;
    }

    void AbrirDialogo()
    {
        dialogoAbierto = true;
        indiceDialogo = 0;

        if (panelDialogo != null) panelDialogo.SetActive(true);
        if (hintE != null) hintE.SetActive(false);

        MostrarDialogoActual();
    }

    void MostrarDialogoActual()
    {
        if (coroutineEscribir != null)
            StopCoroutine(coroutineEscribir);

        coroutineEscribir = StartCoroutine(EscribirTexto(dialogos[indiceDialogo]));
    }

    IEnumerator EscribirTexto(string texto)
    {
        escribiendo = true;
        textoDialogo.text = "";

        foreach (char letra in texto)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        escribiendo = false;
    }

    void MostrarTextoCompleto()
    {
        if (coroutineEscribir != null)
            StopCoroutine(coroutineEscribir);

        textoDialogo.text = dialogos[indiceDialogo];
        escribiendo = false;
    }

    void SiguienteDialogo()
    {
        indiceDialogo++;

        if (indiceDialogo < dialogos.Length)
            MostrarDialogoActual();
        else
            CerrarDialogo();
    }

    void CerrarDialogo()
    {
        dialogoAbierto = false;
        escribiendo = false;

        if (coroutineEscribir != null)
            StopCoroutine(coroutineEscribir);

        if (panelDialogo != null) panelDialogo.SetActive(false);
        if (hintE != null) hintE.SetActive(true); // muestra el hint de nuevo
    }

    // Botón UI opcional
    public void BotonContinuar()
    {
        if (escribiendo)
            MostrarTextoCompleto();
        else
            SiguienteDialogo();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Collider col = GetComponent<Collider>();
        if (col != null)
            Gizmos.DrawWireSphere(transform.position,
                Mathf.Max(col.bounds.extents.x, col.bounds.extents.z));
    }
}