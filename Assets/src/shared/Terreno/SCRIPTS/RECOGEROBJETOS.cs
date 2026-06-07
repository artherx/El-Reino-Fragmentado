using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Jugador")]
    public GameObject player;

    [Header("Nivel actual")]
    public int currentLevel = 1;

    [Header("Objetos por nivel")]
    public GameObject[] level1Objects;
    public GameObject[] level2Objects;
    public GameObject[] level3Objects;

    [Header("Puertas de puente")]
    public GameObject bridgeDoor1; // Puerta puente tras reto 1
    public GameObject bridgeDoor2; // Puerta puente tras portal nivel 2
    public GameObject bridgeDoor3; // Puerta puente tras portal nivel 3

    [Header("Puertas de portal")]
    public GameObject portalDoor2; // Puerta hacia portal nivel 2
    public GameObject portalDoor3; // Puerta hacia portal nivel 3

    [Header("Puerta extra opcional")]
    public GameObject extraDoor;

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
            case 1: totalToCollect = level1Objects.Length; break;
            case 2: totalToCollect = level2Objects.Length; break;
            case 3: totalToCollect = level3Objects.Length; break;
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
            UnlockChallengeDoor();
        }
    }

    void UnlockChallengeDoor()
    {
        switch (currentLevel)
        {
            case 1:
                if (bridgeDoor1 != null) bridgeDoor1.SetActive(false);
                Debug.Log("✅ Reto 1 completado. Puerta del puente desbloqueada.");
                break;

            case 2:
                if (portalDoor2 != null) portalDoor2.SetActive(false);
                Debug.Log("✅ Reto 2 completado. Puerta hacia portal desbloqueada.");
                break;

            case 3:
                if (portalDoor3 != null) portalDoor3.SetActive(false);
                Debug.Log("✅ Puzzle nivel 3 completado. Puerta hacia portal desbloqueada.");
                break;
        }

        if (extraDoor != null)
        {
            extraDoor.SetActive(false);
            Debug.Log("🚪 Puerta extra desbloqueada.");
        }
    }

    // Llamado desde el script del portal
    public void OnPortalEntered(int portalLevel)
    {
        if (portalLevel == 2 && bridgeDoor2 != null)
        {
            bridgeDoor2.SetActive(false);
            Debug.Log("⚡ Portal nivel 2 cruzado. Puerta del puente desbloqueada.");

            // 🔄 Actualizar automáticamente el nivel a 3
            currentLevel = 3;
            SetupLevel(); // reconfigura los objetos y targets del nivel 3
            Debug.Log("🔄 Nivel cambiado automáticamente a 3 al cruzar la zona 2 del puente.");
        }
        else if (portalLevel == 3 && bridgeDoor3 != null)
        {
            bridgeDoor3.SetActive(false);
            Debug.Log("⚡ Portal nivel 3 cruzado. Puerta del puente desbloqueada.");
        }
    }

    public string GetHUDInfo()
    {
        return "Objetos recogidos: " + collectedCount + " / " + totalToCollect;
    }
}
