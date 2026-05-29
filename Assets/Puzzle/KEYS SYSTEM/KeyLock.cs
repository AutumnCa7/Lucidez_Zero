using UnityEngine;

public class KeyLock : MonoBehaviour
{
    [Header("Configuración de Cerradura")]
    [SerializeField] private KeyColor requiredColor; // El color de llave que necesita

    [Header("Puerta a Abrir")]
    [SerializeField] private LockedDoor doorToOpen; // El script de la puerta que hicimos antes

    [Header("Audio")]
    [SerializeField] private AudioClip lockedSound; // Sonido de puerta trancada
    [SerializeField] private AudioClip unlockSound; // Sonido de cerradura abriendo

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
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerInventory = other.GetComponent<PlayerInventory>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerInventory = null;
        }
    }

    private void Update()
    {
        // Si la puerta ya no está trancada, apagamos este script para no seguir comprobando
        if (!doorToOpen.IsLocked())
        {
            this.enabled = false;
            return;
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && playerInventory != null)
        {
            // Verificamos si el jugador tiene la llave del color correcto
            if (playerInventory.HasKey(requiredColor))
            {
                // ¡Tiene la llave!
                if (unlockSound != null) audioSource.PlayOneShot(unlockSound);

                // Le damos la orden a tu puerta de que se abra
                doorToOpen.UnlockAndOpen();
            }
            else
            {
                // No tiene la llave correcta, suena trancado
                if (lockedSound != null) audioSource.PlayOneShot(lockedSound);
                Debug.Log("Necesitas la llave " + requiredColor.ToString() + " para abrir esto.");
            }
        }
    }
}