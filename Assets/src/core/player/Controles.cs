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

    public bool IsAttacking { get { return isAttacking; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
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
                // FIX: isJumping e isFalling solo pueden ser true si NO estamos en el suelo.
                // Antes, un rebote de pared podía dar velocidad Y negativa brevemente
                // aunque estuviéramos en el suelo, activando isFalling incorrectamente.
                if (rb.linearVelocity.y > 0.1f)
                {
                    isJumping = true;
                    isFalling = false;
                }
                else if (rb.linearVelocity.y < -0.1f)
                {
                    isJumping = false;
                    isFalling = true;
                }
            }
            else
            {
                // En el suelo: resetear siempre, sin importar la velocidad Y
                isJumping = false;
                isFalling = false;
            }

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

        // IsMoving lo maneja LogicaPersonaje.cs desde el input crudo
    }

    // --- RUTINAS ---

    private IEnumerator RutinaAtaque()
    {
        isAttacking = true;
        anim.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
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