using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Controles : MonoBehaviour
{
    [Header("Configuración del Salto")]
    public float jumpForce = 6.0f;
    public float jumpDelay = 0.15f;
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Configuración de Combate")]
    public float detectionRadius = 5f;
    public LayerMask enemyLayer;

    private Rigidbody rb;
    private Animator anim;

    // Estados de Movimiento/Salto
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private bool isJumpPending;

    // Estados de Combate
    private bool isEnemyNear;
    private bool isAttacking;
    private bool isDamage;

    // Propiedad pública para que el script DañoEspada sepa si estamos atacando
    public bool IsAttacking { get { return isAttacking; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // ------------------ 1. FÍSICAS Y SALTO ------------------
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && !isJumpPending && !isAttacking && !isDamage)
        {
            StartCoroutine(SaltoConRetraso());
        }

        if (!isJumpPending)
        {
            if (!isGrounded)
            {
                if (rb.linearVelocity.y > 0.1f) { isJumping = true; isFalling = false; }
                else if (rb.linearVelocity.y < -0.1f) { isJumping = false; isFalling = true; }
            }
            else { isJumping = false; isFalling = false; }

            anim.SetBool("IsJumping", isJumping);
            anim.SetBool("IsFalling", isFalling);
        }
        anim.SetBool("IsGrounded", isGrounded);


        // ------------------ 2. LÓGICA DE COMBATE (Radar) ------------------
        Collider[] enemigosCercanos = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        isEnemyNear = false;

        foreach (Collider col in enemigosCercanos)
        {
            if (col.CompareTag("Enemy"))
            {
                isEnemyNear = true;
                break;
            }
        }
        anim.SetBool("IsEnemyNear", isEnemyNear);

        // ------------------ 3. ATACAR ------------------
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            if (isEnemyNear && !isAttacking && !isDamage && isGrounded)
            {
                StartCoroutine(RutinaAtaque());
            }
        }

        float velX = anim.GetFloat("VelX");
        float velY = anim.GetFloat("VelY");

        // Comprobamos si hay movimiento en cualquier dirección (Mathf.Abs convierte los números negativos a positivos para la comprobación)
        bool isMoving = Mathf.Abs(velX) > 0.1f || Mathf.Abs(velY) > 0.1f;
        anim.SetBool("IsMoving", isMoving);
    }

    // --- RUTINAS ---

    private IEnumerator RutinaAtaque()
    {
        isAttacking = true; // Al encender esto, el cubo de la espada empieza a hacer daño
        anim.SetBool("IsAttacking", true);

        // Esperamos a que termine la animación de ataque (Ajusta este tiempo a lo que dure tu estocada)
        yield return new WaitForSeconds(0.8f);

        isAttacking = false; // Apagamos el daño de la espada
        anim.SetBool("IsAttacking", false);
    }

    private IEnumerator SaltoConRetraso()
    {
        isJumpPending = true;
        isJumping = true;
        anim.SetBool("IsJumping", true);
        yield return new WaitForSeconds(jumpDelay);
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        yield return new WaitForSeconds(0.1f);
        isJumpPending = false;
    }

    // --- RECIBIR DAÑO EN EL CUERPO ---
    // Si el collider del cuerpo del personaje toca un enemigo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isDamage)
        {
            UnityEngine.Debug.Log("🩸 ¡El CUERPO del personaje recibió daño!");
            StartCoroutine(RutinaRecibirDaño());
        }
    }

    private IEnumerator RutinaRecibirDaño()
    {
        isDamage = true;
        anim.SetBool("IsDamage", true);

        // Si nos pegan mientras atacábamos, se cancela el ataque
        isAttacking = false;
        anim.SetBool("IsAttacking", false);

        yield return new WaitForSeconds(0.6f);

        isDamage = false;
        anim.SetBool("IsDamage", false);
    }

    // --- GIZMOS ---
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}