using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    // Singleton: Permite que las notas del mundo llamen a este script sin arrastrarlo
    public static NoteUIManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TextMeshProUGUI noteTextDisplay;

    [Header("Player Reference")]
    [SerializeField] private PlayerController playerController;

    private bool isReading = false;

    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
        notePanel.SetActive(false);
    }

    public void ShowNote(string content)
    {
        isReading = true;
        noteTextDisplay.text = content;
        notePanel.SetActive(true);

        // Congelamos al jugador para que no camine mientras lee
        if (playerController != null) playerController.enabled = false;

        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        isReading = false;
        notePanel.SetActive(false);

        
        if (playerController != null) playerController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Si está leyendo y presiona E o Escape, se cierra la nota
        if (isReading && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)))
        {
            CloseNote();
        }
    }
}