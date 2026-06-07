using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float runSpeed = 2f;
    public float jumpSpeed = 3f;
    public bool betterJump = false;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public Animator animator;

    private Rigidbody2D rb2D;
    private SpriteRenderer spriteRenderer;
    private bool jumpRequested = false;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb2D != null)
            rb2D.freezeRotation = true;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && CheckGround.isGrounded)
        {
            jumpRequested = true;

            if (SoundManagerMiniJuego.Instance != null)
                SoundManagerMiniJuego.Instance.PlaySalto();
        }

        if (!CheckGround.isGrounded)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetBool("Jump", false);
        }
    }

    void FixedUpdate()
    {
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            rb2D.linearVelocity = new Vector2(runSpeed, rb2D.linearVelocity.y);
            spriteRenderer.flipX = false;

            if (CheckGround.isGrounded)
                animator.SetBool("Run", true);
        }
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            rb2D.linearVelocity = new Vector2(-runSpeed, rb2D.linearVelocity.y);
            spriteRenderer.flipX = true;

            if (CheckGround.isGrounded)
                animator.SetBool("Run", true);
        }
        else
        {
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
            animator.SetBool("Run", false);
        }

        if (jumpRequested)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpSpeed);
            jumpRequested = false;
        }

        if (betterJump)
        {
            if (rb2D.linearVelocity.y < 0)
            {
                rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
            }
            else if (rb2D.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
            {
                rb2D.linearVelocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.fixedDeltaTime;
            }
        }
    }
}