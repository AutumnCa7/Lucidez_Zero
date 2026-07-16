using UnityEngine;

public class KeyLock : MonoBehaviour
{
    [Header("Configuración de Cerradura")]
    [SerializeField] private KeyColor requiredColor;
    [SerializeField] private LockedDoor doorToOpen;

    [Header("Textos")]
    [SerializeField] private string interactPrompt = "Presiona E para usar la cerradura";
    [SerializeField] private string lockedMessage = "Está cerrada... Necesito buscar algo para abrirla!";

    [Header("Audio")]
    [SerializeField] private AudioClip myVoiceLockedSound; 
    [SerializeField] private AudioClip unlockSound;

    private AudioSource audioSource;
    private bool isPlayerNearby = false;
    private PlayerInventory playerInventory;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && doorToOpen.IsLocked())
        {
            isPlayerNearby = true;
            playerInventory = other.GetComponent<PlayerInventory>();

            
            HUDManager.Instance.ShowInteraction(interactPrompt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerInventory = null;

            // Ocultamos el aviso al alejarnos
            HUDManager.Instance.HideInteraction();
        }
    }

    private void Update()
    {
        if (!doorToOpen.IsLocked())
        {
            this.enabled = false;
            return;
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && playerInventory != null)
        {
            if (playerInventory.HasKey(requiredColor))
            {
                
                HUDManager.Instance.HideInteraction();
                if (unlockSound != null) audioSource.PlayOneShot(unlockSound);
                doorToOpen.UnlockAndOpen();
            }
            else
            {
                
                HUDManager.Instance.ShowMessage(lockedMessage, 3f);

                
                if (myVoiceLockedSound != null && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(myVoiceLockedSound);
                }
            }
        }
    }
}