using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Controles : MonoBehaviour
{
    [Header("Configuraci�n del Salto")]
    public float jumpForce = 6.0f;        // Fuerza del impulso del salto
    public Transform groundCheck;        // Objeto vac�o en los pies del personaje
    public float groundDistance = 0.2f;  // Radio del sensor de suelo
    public LayerMask groundLayer;        // Capa asignada al suelo

    private Rigidbody rb;
    private Animator anim;

    // Estados internos
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // Bloqueamos las rotaciones en X y Z para que el Rigidbody de f�sicas no se tropiece y caiga de lado
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // 1. Detectar si el sensor de los pies est� tocando la capa del suelo
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        // 2. Si est� en el suelo y presionamos la barra espaciadora (o bot�n configurado de salto)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Aplicamos velocidad vertical directamente manteniendo el movimiento horizontal intacto
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

            isJumping = true;
            isFalling = false;
        }

        // 3. Control de estados a�reos autom�ticos seg�n la velocidad del Rigidbody
        if (!isGrounded)
        {
            // Si el personaje va hacia arriba en el eje Y
            if (rb.linearVelocity.y > 0.1f)
            {
                isJumping = true;
                isFalling = false;
            }
            // Si el personaje empieza a caer (velocidad Y negativa)
            else if (rb.linearVelocity.y < -0.1f)
            {
                isJumping = false;
                isFalling = true;
            }
        }
        else
        {
            // Si ya toc� el suelo, desactivamos los estados a�reos
            isJumping = false;
            isFalling = false;
        }

        // 4. Comunicar los estados exactos a los booleanos de tu Animator
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsJumping", isJumping);
        anim.SetBool("IsFalling", isFalling);
    }

    // Dibuja la esferita de detecci�n en la vista de Escena (Scene) para ajustar los pies visualmente
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}