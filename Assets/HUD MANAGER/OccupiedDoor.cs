using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OccupiedDoor : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [SerializeField] private float openAngle = 90f; // Ángulo de apertura
    [SerializeField] private float openSpeed = 2f;

    [Header("Textos del HUD")]
    [SerializeField] private string interactPrompt = "Presiona E para abrir";
    [SerializeField] private string occupiedMessage = "¡Ocupado!";
    [SerializeField] private string secondPrompt = "Presiona E para abrir de todos modos..."; // Texto para el segundo intento

    [Header("Audios")]
    [SerializeField] private AudioClip occupiedVoiceSound; // Tu voz o audio diciendo "¡Ocupado!"
    [SerializeField] private AudioClip doorCreakSound; // Sonido de puerta abriéndose

    private AudioSource audioSource;
    private bool isPlayerNearby = false;
    private int interactionStep = 0; // 0 = Cerrada, 1 = Gritó ocupado, 2 = Abierta
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

            // Mostramos un texto u otro dependiendo de si ya nos gritaron
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
        // Si se dio la orden de abrir, rotamos la puerta suavemente en cada frame
        if (isOpening)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
        }

        // Detectar el botón E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionStep == 0)
            {
                // PRIMER INTENTO: Gritan "¡Ocupado!"
                if (occupiedVoiceSound != null) audioSource.PlayOneShot(occupiedVoiceSound);

                // Mostramos el texto en el centro de la pantalla
                HUDManager.Instance.ShowMessage(occupiedMessage, 2.5f);

                // Cambiamos el texto de la E para el siguiente intento
                HUDManager.Instance.ShowInteraction(secondPrompt);

                interactionStep = 1; // Pasamos a la siguiente fase
            }
            else if (interactionStep == 1)
            {
                // SEGUNDO INTENTO: Se abre la puerta
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        interactionStep = 2; // Terminó la interacción
        HUDManager.Instance.HideInteraction(); // Ocultamos el texto

        if (doorCreakSound != null) audioSource.PlayOneShot(doorCreakSound);

        // Calculamos hacia dónde debe girar
        targetRotation = Quaternion.Euler(0, transform.localEulerAngles.y + openAngle, 0);
        isOpening = true;

        // Desactivamos el Collider (trigger) para que ya no nos salga el mensaje de la "E" al pasar cerca
        Collider trigger = GetComponent<Collider>();
        if (trigger != null)
        {
            trigger.enabled = false;
        }
    }
}