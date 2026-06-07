using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Shared;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthComponent))]
public class Controles : MonoBehaviour, IDamageable
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

    [Header("Vidas")]
    public int maxHealth = 3;
    public string menuSceneName = "Menu";

    private Rigidbody rb;
    private Animator anim;
    private HealthComponent healthComponent;
    private GameObject healthCanvas;
    private Text healthText;
    private bool isDead;

    // Estados de Movimiento/Salto
    private bool isGrounded;
    private bool isJumping;
    private bool isFalling;
    private bool isJumpPending;

    // Estados de Combate
    private bool isEnemyNear;
    private bool isAttacking;
    private bool isDamage;

    // Propiedades públicas
    public bool IsAttacking { get { return isAttacking; } }
    public bool IsDead { get { return isDead; } }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        healthComponent = GetComponent<HealthComponent>();
        healthComponent.Initialize(maxHealth);
        healthComponent.OnDeath += HandleDeath;
        healthComponent.OnHealthChanged += UpdateHealthUI;

        CreateHealthUI();
        UpdateHealthUI(maxHealth);
    }

    void Update()
    {
        if (isDead) return;

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

        bool isMoving = Mathf.Abs(velX) > 0.1f || Mathf.Abs(velY) > 0.1f;
        anim.SetBool("IsMoving", isMoving);
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

    public void TakeDamage(int amount)
    {
        if (isDead || isDamage)
        {
            Debug.Log($"[Player] TakeDamage({amount}) BLOQUEADO: isDead={isDead}, isDamage={isDamage}");
            return;
        }
        Debug.Log($"[Player] Recibido {amount} de daño. Vida actual: {healthComponent.CurrentHealth}");
        healthComponent.ApplyDamage(amount);
        if (!isDead)
            StartCoroutine(RutinaRecibirDaño());
    }

    private void HandleDeath()
    {
        isDead = true;
        isDamage = false;
        anim.SetTrigger("Die");
        StartCoroutine(DeathTransition());
        Destroy(healthCanvas);
    }

    private IEnumerator DeathTransition()
    {
        float timer = 0f;
        while (timer < 2f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(menuSceneName);
    }

    private void UpdateHealthUI(int currentHP)
    {
        if (healthText != null)
            healthText.text = $"❤️ {currentHP}/{maxHealth}";
    }

    private void CreateHealthUI()
    {
        healthCanvas = new GameObject("HealthCanvas");
        Canvas canvas = healthCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        healthCanvas.AddComponent<CanvasScaler>();

        GameObject textObj = new GameObject("HealthText");
        textObj.transform.SetParent(healthCanvas.transform);

        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);

        healthText = textObj.AddComponent<Text>();
        healthText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        healthText.fontSize = 36;
        healthText.color = Color.white;
        healthText.alignment = TextAnchor.UpperLeft;
    }

    // --- RECIBIR DAÑO EN EL CUERPO ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isDamage)
        {
            TakeDamage(1);
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