using UnityEngine;

public class RecogerObjetos : MonoBehaviour
{
    [Header("Jugador")]
    public GameObject player; // Arrastra aquí tu jugador

    [Header("Objeto a recoger")]
    public GameObject item; // Arrastra aquí el objeto que se puede recoger

    [Header("Zona de entrega")]
    public Transform deliveryPoint; // Arrastra aquí el punto de entrega

    private bool isCarrying = false;

    void Update()
    {
        // Si estamos cerca del objeto y presionamos E, lo recogemos
        if (!isCarrying && Input.GetKeyDown(KeyCode.E))
        {
            float distance = Vector3.Distance(player.transform.position, item.transform.position);
            if (distance < 2f) // distancia mínima para recoger
            {
                PickupItem();
            }
        }

        // Si estamos cargando y llegamos a la zona de entrega
        if (isCarrying)
        {
            float distanceToDelivery = Vector3.Distance(player.transform.position, deliveryPoint.position);
            if (distanceToDelivery < 2f)
            {
                PlaceItem();
            }
        }
    }

    private void PickupItem()
    {
        isCarrying = true;
        // Hacemos que el objeto siga al jugador
        item.transform.SetParent(player.transform);
        item.transform.localPosition = new Vector3(0, 1, 1); // posición relativa al jugador
    }

    private void PlaceItem()
    {
        isCarrying = false;
        // Soltamos el objeto en el punto de entrega
        item.transform.SetParent(null);
        item.transform.position = deliveryPoint.position;
    }
}
