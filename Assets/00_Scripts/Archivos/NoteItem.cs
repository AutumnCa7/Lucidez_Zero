using UnityEngine;

[RequireComponent(typeof(SphereCollider))] // Obliga a tener un detector 
public class NoteItem : MonoBehaviour
{
    [Header("Note Content")]
    
    [TextArea(5, 10)]
    [SerializeField] private string noteContent;

    [Header("Audio")]
    [SerializeField] private AudioClip paperSound;

    private AudioSource audioSource;
    private bool isPlayerNearby = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        // Configura autom del Collider por cod para evitar errores
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = 2f; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // TIP EXTRA: Aquí podrías activar un texto en el HUD del estilo "Press E to Read Document"
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void Update()
    {
        
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (paperSound != null)
            {
                audioSource.PlayOneShot(paperSound);
            }

            
            NoteUIManager.Instance.ShowNote(noteContent);
        }
    }
}