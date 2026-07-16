using UnityEngine;

public class ExitDoorTrigger : MonoBehaviour
{
    [SerializeField] private FadeManager fadeManager;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        if (other.CompareTag("Player"))
        {
            activado = true;

            fadeManager.FadeToScene("Final Cinematic");
        }
    }
}