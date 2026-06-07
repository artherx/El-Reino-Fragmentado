using UnityEngine;
using Shared.Events;

public class DeathEventListener : MonoBehaviour
{
    [Header("Evento a Escuchar")]
    public GameEventSO onEnemyDeathEvent;

    private void OnEnable()
    {
        if (onEnemyDeathEvent != null)
        {
            onEnemyDeathEvent.RegisterListener(OnEnemyDiedGlobal);
        }
    }

    private void OnDisable()
    {
        if (onEnemyDeathEvent != null)
        {
            onEnemyDeathEvent.UnregisterListener(OnEnemyDiedGlobal);
        }
    }

    private void OnEnemyDiedGlobal()
    {
        // Este log confirma que la comunicación desacoplada por SO funciona
        Debug.Log("<b>[Global Listener]</b> ¡Se ha detectado la muerte de un enemigo en el sistema global!");
    }
}