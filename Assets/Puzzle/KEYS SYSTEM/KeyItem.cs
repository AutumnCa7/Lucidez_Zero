using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Configuración de la Llave")]
    [SerializeField] private KeyColor myColor; // Eliges el color en el Inspector

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound; // Sonido de llaves tintineando

    private bool isPlayerNearby = false;
    private PlayerInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // Buscamos el inventario en el jugador
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
        // Si el jugador está cerca y presiona la E, recoge la llave
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && playerInventory != null)
        {
            // 1. Guardamos la llave en el inventario
            playerInventory.AddKey(myColor);

            // 2. Reproducimos el sonido (usamos PlayClipAtPoint para que suene aunque el objeto se destruya)
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // 3. Destruimos el objeto 3D de la llave en el mundo
            Destroy(gameObject);
        }
    }
}