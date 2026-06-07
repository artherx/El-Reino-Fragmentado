using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float startWaitTime = 0.3f;
    private float waitedTime;
    private bool playerOnPlatform = false;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        waitedTime = startWaitTime;
    }

    void Update()
    {
        bool downPressed = Keyboard.current.downArrowKey.isPressed
                        || Keyboard.current.sKey.isPressed;

        bool downReleased = Keyboard.current.downArrowKey.wasReleasedThisFrame
                         || Keyboard.current.sKey.wasReleasedThisFrame;

        if (!playerOnPlatform)
        {
            // Si el jugador no está en la plataforma, resetea siempre
            effector.rotationalOffset = 0f;
            waitedTime = startWaitTime;
            return;
        }

        if (downPressed)
        {
            waitedTime -= Time.deltaTime;

            if (waitedTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitedTime = startWaitTime;
            }
        }

        if (downReleased)
        {
            waitedTime = startWaitTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            playerOnPlatform = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            playerOnPlatform = false;
            // Pequeño delay para que el jugador alcance a pasar
            Invoke(nameof(ResetEffector), 0.2f);
        }
    }

    private void ResetEffector()
    {
        effector.rotationalOffset = 0f;
        waitedTime = startWaitTime;
    }
}