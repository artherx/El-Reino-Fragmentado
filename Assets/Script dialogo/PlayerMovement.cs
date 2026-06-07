using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 5f;
    public float fuerzaSalto = 5f;

    private Rigidbody rb;
    private bool saltoPendiente = false;
    private bool estaEnSuelo = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && estaEnSuelo)
            saltoPendiente = true;
    }

    void FixedUpdate()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            horizontal = 1f;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            horizontal = -1f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            vertical = 1f;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            vertical = -1f;

        Vector3 movimiento = new Vector3(horizontal, 0f, vertical).normalized * velocidad;
        rb.linearVelocity = new Vector3(movimiento.x, rb.linearVelocity.y, movimiento.z);

        if (saltoPendiente)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, fuerzaSalto, rb.linearVelocity.z);
            saltoPendiente = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            estaEnSuelo = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            estaEnSuelo = false;
    }
}