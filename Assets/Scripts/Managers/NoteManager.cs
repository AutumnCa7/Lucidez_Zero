using TMPro;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance;

    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteText;

    private bool noteOpen = false;

    private void Awake()
    {
        Instance = this;
        notePanel.SetActive(false);
    }

    public void OpenNote(string text)
    {
        notePanel.SetActive(true);
        noteText.text = text;

        noteOpen = true;

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);

        noteOpen = false;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsNoteOpen()
    {
        return noteOpen;
    }
}
