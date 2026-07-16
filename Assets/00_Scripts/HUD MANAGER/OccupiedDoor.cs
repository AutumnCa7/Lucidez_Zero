using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OccupiedDoor : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [SerializeField] private float openAngle = 90f; 
    [SerializeField] private float openSpeed = 2f;

    [Header("Textos del HUD")]
    [SerializeField] private string interactPrompt = "Presiona E para abrir";
    [SerializeField] private string occupiedMessage = "¡Ocupado!";
    [SerializeField] private string secondPrompt = "Presiona E para abrir de todos modos..."; 

    [Header("Audios")]
    [SerializeField] private AudioClip occupiedVoiceSound; 
    [SerializeField] private AudioClip doorCreakSound; 

    private AudioSource audioSource;
    private bool isPlayerNearby = false;
    private int interactionStep = 0; 
    private bool isOpening = false;
    private Quaternion targetRotation;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        // Quitamos que suene al iniciar por si acaso
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactionStep < 2)
        {
            isPlayerNearby = true;

            
            if (interactionStep == 0)
                HUDManager.Instance.ShowInteraction(interactPrompt);
            else if (interactionStep == 1)
                HUDManager.Instance.ShowInteraction(secondPrompt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            HUDManager.Instance.HideInteraction();
        }
    }

    private void Update()
    {
        
        if (isOpening)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
        }

        // Detectar el boton E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionStep == 0)
            {
                
                if (occupiedVoiceSound != null) audioSource.PlayOneShot(occupiedVoiceSound);

                
                HUDManager.Instance.ShowMessage(occupiedMessage, 2.5f);

                
                HUDManager.Instance.ShowInteraction(secondPrompt);

                interactionStep = 1; 
            }
            else if (interactionStep == 1)
            {
                
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        interactionStep = 2; 
        HUDManager.Instance.HideInteraction(); 

        if (doorCreakSound != null) audioSource.PlayOneShot(doorCreakSound);

        
        targetRotation = Quaternion.Euler(0, transform.localEulerAngles.y + openAngle, 0);
        isOpening = true;

        
        Collider trigger = GetComponent<Collider>();
        if (trigger != null)
        {
            trigger.enabled = false;
        }
    }
}