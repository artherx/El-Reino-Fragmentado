using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Jugador")]
    public GameObject player;

    [Header("Nivel actual")]
    public int currentLevel = 1; // 1, 2 o 3

    [Header("Objetos por nivel")]
    public GameObject[] level1Objects; // 1 objeto
    public GameObject[] level2Objects; // 3 objetos
    public GameObject[] level3Objects; // 4 cabezas

    [Header("Puestos de entrega")]
    public Transform level1Delivery;   // 1 puesto
    public Transform level2Delivery;   // 1 puesto
    public Transform[] level3Deliveries; // 2 puestos de diferente color

    [Header("Puertas de desbloqueo")]
    public GameObject doorLevel1; // Puerta invisible que se activa al completar nivel 1
    public GameObject doorLevel2; // Puerta invisible que se activa al completar nivel 2
    public GameObject doorLevel3; // Puerta invisible que se activa al completar nivel 3

    private int collectedCount = 0;
    private int totalToCollect = 0;

    void Start()
    {
        SetupLevel();
    }

    void SetupLevel()
    {
        // Configura cuántos objetos hay que recoger según el nivel
        switch (currentLevel)
        {
            case 1:
                totalToCollect = level1Objects.Length;
                break;
            case 2:
                totalToCollect = level2Objects.Length;
                break;
            case 3:
                totalToCollect = level3Objects.Length;
                break;
        }

        collectedCount = 0;
        Debug.Log("Nivel " + currentLevel + " iniciado. Objetos a recoger: " + totalToCollect);
    }

    public void CollectObject(GameObject obj)
    {
        collectedCount++;
        Debug.Log("Objeto recogido: " + obj.name + " | Total: " + collectedCount + "/" + totalToCollect);

        if (collectedCount >= totalToCollect)
        {
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        switch (currentLevel)
        {
            case 1:
                if (doorLevel1 != null) doorLevel1.SetActive(false); // desactiva el collider invisible
                Debug.Log("✅ Nivel 1 completado. Puerta desbloqueada.");
                break;
            case 2:
                if (doorLevel2 != null) doorLevel2.SetActive(false);
                Debug.Log("✅ Nivel 2 completado. Puerta desbloqueada.");
                break;
            case 3:
                if (doorLevel3 != null) doorLevel3.SetActive(false);
                Debug.Log("✅ Nivel 3 completado. Puerta desbloqueada.");
                break;
        }
    }

    // Método para mostrar datos en el HUD
    public string GetHUDInfo()
    {
        return "Objetos recogidos: " + collectedCount + " / " + totalToCollect;
    }
}
