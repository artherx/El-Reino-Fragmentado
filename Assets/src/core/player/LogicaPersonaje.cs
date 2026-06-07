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
    public float smoothTime = 30f; // Qué tan rápido hace la transición entre animaciones

    private Animator anim;
    private Rigidbody rb;
    private Transform cameraTransform;

    // Variables internas para el suavizado y el input
    private float currentVelX;
    private float currentVelY;
    private Vector2 currentInput;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Busca automáticamente la cámara principal en la escena
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // El Update se usa para leer botones y manejar la animación (Visual)
    void Update()
    {
        // 1. Leer el teclado directamente con el Nuevo Input System (Teclas WASD)
        currentInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) currentInput.y += 1;
            if (Keyboard.current.sKey.isPressed) currentInput.y -= 1;
            if (Keyboard.current.dKey.isPressed) currentInput.x += 1;
            if (Keyboard.current.aKey.isPressed) currentInput.x -= 1;
        }

        // Normalizamos para que caminar en diagonal no te haga ir más rápido
        if (currentInput.magnitude > 1f) currentInput.Normalize();

        // 2. LA MAGIA: Suavizar los valores brutos del teclado hacia las variables del Animator
        currentVelX = Mathf.Lerp(currentVelX, currentInput.x, Time.deltaTime * smoothTime);
        currentVelY = Mathf.Lerp(currentVelY, currentInput.y, Time.deltaTime * smoothTime);

        // 3. Mandar la información suave a tu Blend Tree
        anim.SetFloat("VelX", currentVelX);
        anim.SetFloat("VelY", currentVelY);
    }

    // El FixedUpdate es el lugar correcto para aplicar fuerzas físicas sin tirones
    void FixedUpdate()
    {
        if (cameraTransform == null) return;

        // 4. Calcular hacia dónde "adelante" y "derecha" basado en la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Anulamos la Y para que el personaje no intente flotar si miras hacia el cielo
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // 5. Crear el vector de dirección final combinando la cámara y el teclado
        Vector3 direction = (currentInput.x * right) + (currentInput.y * forward);

        // 6. Mover el Rigidbody conservando intacta su velocidad en Y (¡Para que el salto funcione!)
        Vector3 targetVelocity = direction * movementSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        // 7. Hacer que el modelo rote suavemente para mirar hacia donde camina
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}