using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMiniGame : MonoBehaviour
{
    [SerializeField] private string nombreEscena;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entró al trigger: " + other.name +" con tag: "+ other.tag);

        if (other.name == "Player" || other.tag == "Player")
        {
            Debug.Log("🚀 El jugador entró al trigger");
            SceneManager.LoadScene(1);
        }
    }
}