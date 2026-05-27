using UnityEngine;

public class FocusTriggerArea : MonoBehaviour
{
    [Header("Focus Target")]
    [SerializeField] private Transform ghostTransform;

    private bool hasTriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Lo marca como usado para que no se repita

            PlayerCameraFocus cameraFocus = other.GetComponentInChildren<PlayerCameraFocus>();
            if (cameraFocus != null)
            {
                cameraFocus.StartFocus(ghostTransform);
            }

            
            GetComponent<Collider>().enabled = false;
        }
    }
}