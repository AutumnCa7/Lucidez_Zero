using UnityEngine;

public class GhostEventTrigger : MonoBehaviour
{
    [Header("Event Configuration")]
    [SerializeField] private GhostController ghost;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private GameObject windowFocusTriggerBox;

    
    public enum EventType { WindowScare, FastCross, StealChase }
    [SerializeField] private EventType currentEvent;

    
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // No se repite

            
            ghost.transform.position = spawnPoint.position;

            if (currentEvent == EventType.WindowScare)
            {
                ghost.gameObject.SetActive(true);
                if (windowFocusTriggerBox != null) windowFocusTriggerBox.SetActive(false);

                PlayerCameraFocus camFocus = other.GetComponentInChildren<PlayerCameraFocus>();
                if (camFocus != null) camFocus.StopFocus();
            }
            else if (currentEvent == EventType.FastCross)
            {
                ghost.TriggerHallwayCross(destinationPoint);
            }
            
            else if (currentEvent == EventType.StealChase)
            {
                ghost.TriggerStealChase();
            }

            GetComponent<Collider>().enabled = false;
        }
    }
}