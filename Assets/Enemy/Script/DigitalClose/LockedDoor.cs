using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private bool isLocked = true;
    [SerializeField] private float openAngle = 90f; // Cuánto se va a abrir la puerta
    [SerializeField] private float openSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip openDoorSound;

    private AudioSource audioSource;
    private bool shouldOpen = false;
    private Quaternion targetRotation;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        // Si el código fue correcto, rotamos la puerta suavemente hacia su ángulo abierta
        if (shouldOpen)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }

    // Esta función la llamará el Tablero Digital cuando el código sea correcto
    public void UnlockAndOpen()
    {
        if (!isLocked) return;

        isLocked = false;
        shouldOpen = true;

        // Calculamos la rotación final sumando el ángulo de apertura a la rotación actual en el eje Y
        targetRotation = Quaternion.Euler(0, transform.localEulerAngles.y + openAngle, 0);

        if (openDoorSound != null)
        {
            audioSource.PlayOneShot(openDoorSound);
        }
    }

    public bool IsLocked()
    {
        return isLocked;
    }
}