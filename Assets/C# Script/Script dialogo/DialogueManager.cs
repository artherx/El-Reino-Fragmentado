using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance;

    [Header("UI")]
    public GameObject panelDialogo;
    public TextMeshProUGUI textoNombre;
    public TextMeshProUGUI textoDialogo;

    [Header("Configuración")]
    public float velocidadTexto = 0.05f; // segundos entre cada letra

    private DialogoData[] dialogos;
    private int indiceActual = 0;
    private bool dialogoActivo = false;
    private bool escribiendo = false;
    private Coroutine coroutineEscribir;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (panelDialogo != null)
            panelDialogo.SetActive(false);
    }

    void Update()
    {
        if (!dialogoActivo) return;

        // Avanza con E, espacio o clic
        if (Input.GetKeyDown(KeyCode.E) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetMouseButtonDown(0))
        {
            if (escribiendo)
            {
                // Si está escribiendo, muestra el texto completo de golpe
                if (coroutineEscribir != null)
                    StopCoroutine(coroutineEscribir);
                textoDialogo.text = dialogos[indiceActual].texto;
                escribiendo = false;
            }
            else
            {
                SiguienteDialogo();
            }
        }
    }

    public void IniciarDialogo(DialogoData[] nuevosDialogos)
    {
        if (dialogoActivo) return;

        dialogos = nuevosDialogos;
        indiceActual = 0;
        dialogoActivo = true;

        if (panelDialogo != null)
            panelDialogo.SetActive(true);

        MostrarDialogoActual();
    }

    private void MostrarDialogoActual()
    {
        textoNombre.text = dialogos[indiceActual].nombre;
        coroutineEscribir = StartCoroutine(EscribirTexto(dialogos[indiceActual].texto));
    }

    private IEnumerator EscribirTexto(string texto)
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

    private void SiguienteDialogo()
    {
        indiceActual++;

        if (indiceActual < dialogos.Length)
        {
            MostrarDialogoActual();
        }
        else
        {
            TerminarDialogo();
        }
    }

    private void TerminarDialogo()
    {
        dialogoActivo = false;

        if (panelDialogo != null)
            panelDialogo.SetActive(false);
    }

    public bool EstaActivo() => dialogoActivo;
}