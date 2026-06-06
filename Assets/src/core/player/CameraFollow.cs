using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform target;

    [Header("Distancia de desfase")]
    public Vector3 offset;

    private void LateUpdate()
    {
        if (target != null)
        {
       
            transform.position = target.position + offset;
        }
    }
}