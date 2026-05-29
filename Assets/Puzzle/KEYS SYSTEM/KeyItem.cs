using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Configuración de la Llave")]
    [SerializeField] private KeyColor myColor;
    [SerializeField] private string interactPrompt = "Presiona E para tomar la llave";

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    private bool isPlayerNearby = false;
    private PlayerInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            playerInventory = other.GetComponent<PlayerInventory>();

            // Mostramos el texto
            HUDManager.Instance.ShowInteraction(interactPrompt);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerInventory = null;

            // Ocultamos el texto
            HUDManager.Instance.HideInteraction();
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && playerInventory != null)
        {
            playerInventory.AddKey(myColor);

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Ocultamos el texto porque el objeto va a desaparecer
            HUDManager.Instance.HideInteraction();
            Destroy(gameObject);
        }
    }
}