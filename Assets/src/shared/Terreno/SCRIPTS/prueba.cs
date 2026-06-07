using UnityEngine;

public class RecogerObjetos : MonoBehaviour
{
    [Header("Jugador")]
    public GameObject player;

    [Header("Objetos por nivel")]
    public GameObject[] itemsNivel1; 
    public GameObject[] itemsNivel2; 
    public GameObject[] itemsNivel3; 

    [Header("Zonas de entrega")]
    public Transform deliveryNivel1;
    public Transform deliveryNivel2;
    public Transform[] deliveryNivel3;

    [Header("Puertas por nivel (solo bloquean paso)")]
    public GameObject puertaNivel1;
    public GameObject puertaNivel2;
    public GameObject puertaNivel3;

    [Header("Colliders que suben de nivel (se usan una sola vez)")]
    public Collider colliderNivel1; 
    public Collider colliderNivel2; 

    [Header("Nivel actual")]
    public int currentLevel = 1;

    private GameObject carriedItem = null;
    private int collectedCount = 0;
    private int totalToCollect = 0;

    void Start()
    {
        SetupLevel();
    }

    void SetupLevel()
    {
        switch (currentLevel)
        {
            case 1: totalToCollect = itemsNivel1.Length; break;
            case 2: totalToCollect = itemsNivel2.Length; break;
            case 3: totalToCollect = itemsNivel3.Length; break;
        }
        collectedCount = 0;
        Debug.Log("Nivel " + currentLevel + " iniciado. Objetos a recoger: " + totalToCollect);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("👉 El jugador tocó: " + other.name);

        // --- Nivel 1 ---
        if (currentLevel == 1)
        {
            foreach (GameObject obj in itemsNivel1)
            {
                if (carriedItem == null && other.gameObject == obj)
                {
                    PickupItem(obj);
                }
            }

            if (carriedItem != null && other.gameObject == deliveryNivel1.gameObject)
            {
                PlaceItem(deliveryNivel1);
                if (collectedCount >= totalToCollect) UnlockDoor(puertaNivel1, 1);
            }
        }

        // --- Nivel 2 ---
        if (currentLevel == 2)
        {
            foreach (GameObject obj in itemsNivel2)
            {
                if (carriedItem == null && other.gameObject == obj)
                {
                    PickupItem(obj);
                }
            }

            if (carriedItem != null && other.gameObject == deliveryNivel2.gameObject)
            {
                PlaceItem(deliveryNivel2);
                if (collectedCount >= totalToCollect) UnlockDoor(puertaNivel2, 2);
            }
        }

        // --- Nivel 3 ---
        if (currentLevel == 3)
        {
            foreach (GameObject obj in itemsNivel3)
            {
                if (carriedItem == null && other.gameObject == obj)
                {
                    PickupItem(obj);
                }
            }

            foreach (Transform point in deliveryNivel3)
            {
                if (carriedItem != null && other.gameObject == point.gameObject)
                {
                    PlaceItem(point);
                    if (collectedCount >= totalToCollect) UnlockDoor(puertaNivel3, 3);
                }
            }
        }

        // --- Colliders que suben de nivel ---
        if (other == colliderNivel1 && currentLevel == 1)
        {
            currentLevel++;
            colliderNivel1.enabled = false; 
            SetupLevel();
            Debug.Log("➡️ Pasaste el collider de nivel 1. Nivel actual: " + currentLevel);
        }

        if (other == colliderNivel2 && currentLevel == 2)
        {
            currentLevel++;
            colliderNivel2.enabled = false; 
            SetupLevel();
            Debug.Log("➡️ Pasaste el collider de nivel 2. Nivel actual: " + currentLevel);
        }
    }

    private void PickupItem(GameObject obj)
    {
        // Solo se puede recoger si no está entregado
        if (obj.CompareTag("Entregado")) return;

        carriedItem = obj;
        obj.transform.SetParent(player.transform);
        obj.transform.localPosition = new Vector3(0, 1, 1);
        Debug.Log("✅ Recogido: " + obj.name);
    }

    private void PlaceItem(Transform point)
    {
        carriedItem.transform.SetParent(null);
        carriedItem.transform.position = point.position;
        carriedItem.transform.rotation = point.rotation;

        // Desactivar física para que se quede fijo
        Rigidbody rb = carriedItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Marcar como entregado para que no se pueda volver a recoger
        carriedItem.tag = "Entregado";

        Debug.Log("📦 Entregado en: " + point.name);
        carriedItem = null;
        collectedCount++;
        Debug.Log("Progreso: " + collectedCount + "/" + totalToCollect);
    }

    private void UnlockDoor(GameObject puerta, int nivel)
    {
        if (puerta != null)
        {
            puerta.SetActive(false); 
            Debug.Log("🚪 Puerta del nivel " + nivel + " desbloqueada.");
        }
    }
}
