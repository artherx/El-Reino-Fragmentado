using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogicaPersonaje : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 10.0f;

    private Vector3 forward, right;
    private Animator anim;

    [Header("Input System")]
    public InputAction moveAction;

    void Start()
    {
        if (Camera.main != null)
        {
         
            forward = Camera.main.transform.forward;
            forward.y = 0;
            forward = Vector3.Normalize(forward);

            right = Camera.main.transform.right;
            right.y = 0;
            right = Vector3.Normalize(right);
        }
        

        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
       
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        
        Vector3 direction = (moveValue.x * right) + (moveValue.y * forward);

        
        if (direction.magnitude > 0.1f)
        {
            transform.position += direction * movementSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        anim.SetFloat("VelX", moveValue.x);
        anim.SetFloat("VelY", moveValue.y);
    }
}