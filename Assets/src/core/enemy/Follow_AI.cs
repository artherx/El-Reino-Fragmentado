using UnityEngine;

public class FollowAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stopDistance = 1.5f;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target == null) return;

        // 1. Clave para 3D: Creamos una posición objetivo que comparte la misma "Y" (altura) que nuestro NPC.
        // Esto garantiza que el NPC solo se mueva en el plano horizontal (suelo) y no intente volar.
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        // 2. Calculamos la distancia plana
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance > stopDistance)
        {
            // 3. Movimiento (El equivalente 3D al código de tu imagen)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // 4. Rotación (El equivalente 3D al "Flip" de tu imagen)
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            // Animación: Si usas Blend Trees Direccionales 2D
            if (anim != null)
            {
                anim.SetFloat("VelX", 0); // No nos movemos de lado
                anim.SetFloat("VelZ", 1); // Nos movemos hacia adelante
            }
        }
        else
        {
            // Se detiene
            if (anim != null)
            {
                anim.SetFloat("VelX", 0);
                anim.SetFloat("VelZ", 0);
            }
        }
    }
}