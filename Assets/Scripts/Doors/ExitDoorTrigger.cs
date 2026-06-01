using UnityEngine;

public class ExitDoorTrigger : MonoBehaviour
{
    public GameUIManager gameUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameUI.MostrarVictoria();
        }
    }
}