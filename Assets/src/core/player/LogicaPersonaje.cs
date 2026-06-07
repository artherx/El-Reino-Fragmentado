using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class LogicaPersonaje : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    [Header("Suavizado de Animación")]
    public float smoothTime = 30f;

    private Animator anim;
    private Rigidbody rb;
    private Transform cameraTransform;

    private float currentVelX;
    private float currentVelY;
    private Vector2 currentInput;

    private Quaternion lastValidRotation;
    private Controles controles;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Congelar rotación del Rigidbody completamente:
        // Que la física NUNCA gire al personaje, solo nuestro código lo hace.
        rb.freezeRotation = true;

        lastValidRotation = transform.rotation;
        controles = GetComponent<Controles>();

        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (controles != null && controles.IsDead) return;

        currentInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) currentInput.y += 1;
            if (Keyboard.current.sKey.isPressed) currentInput.y -= 1;
            if (Keyboard.current.dKey.isPressed) currentInput.x += 1;
            if (Keyboard.current.aKey.isPressed) currentInput.x -= 1;
        }

        if (currentInput.magnitude > 1f) currentInput.Normalize();

        // Suavizado SOLO para animaciones
        currentVelX = Mathf.Lerp(currentVelX, currentInput.x, Time.deltaTime * smoothTime);
        currentVelY = Mathf.Lerp(currentVelY, currentInput.y, Time.deltaTime * smoothTime);

        anim.SetFloat("VelX", currentVelX);
        anim.SetFloat("VelY", currentVelY);

        // IsMoving basado en input CRUDO, no en el Lerp
        // Así responde instantáneamente al soltar o presionar teclas
        bool isMoving = currentInput.magnitude > 0.1f;
        anim.SetBool("IsMoving", isMoving);
    }

    void FixedUpdate()
    {
        if (controles != null && controles.IsDead) return;
        if (cameraTransform == null) return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 rawDirection = (currentInput.x * right) + (currentInput.y * forward);

        if (rawDirection.magnitude > 0.1f)
        {
            // Mover
            Vector3 targetVelocity = rawDirection * movementSpeed;
            // IMPORTANTE: conservamos Y intacta para no matar el salto
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

            // Rotar
            Quaternion targetRotation = Quaternion.LookRotation(rawDirection.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            lastValidRotation = transform.rotation;
        }
        else
        {
            // Sin input: solo detener XZ, Y libre para salto/caída
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            transform.rotation = lastValidRotation;
        }
    }
}