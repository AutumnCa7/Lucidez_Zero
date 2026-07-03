using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private bool isLocked = true;
    [SerializeField] private float openAngle = 90f; // Cu�nto se va a abrir la puerta
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
        // Si el c�digo fue correcto, rotamos la puerta suavemente hacia su �ngulo abierta
        if (shouldOpen)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);
        }
    }

    // Esta funci�n la llamar� el Tablero Digital cuando el c�digo sea correcto
    public void UnlockAndOpen()
    {
        if (!isLocked) return;

        isLocked = false;
        shouldOpen = true;

        // Calculamos la rotaci�n final sumando el �ngulo de apertura a la rotaci�n actual en el eje Y
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