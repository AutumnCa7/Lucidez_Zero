using UnityEngine;
using TMPro; // Para TextMeshPro
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI interactionText; // Para el "Presiona E"
    [SerializeField] private TextMeshProUGUI messageText; // Para "Está cerrada"

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Nos aseguramos de que empiecen vacíos
        interactionText.text = "";
        messageText.text = "";
    }

    // Función para mostrar el aviso de "Presiona E"
    public void ShowInteraction(string text)
    {
        interactionText.text = text;
    }

    // Función para ocultar el aviso
    public void HideInteraction()
    {
        interactionText.text = "";
    }

    // Función para mostrar pensamientos/mensajes por unos segundos
    public void ShowMessage(string msg, float duration = 3f)
    {
        StopAllCoroutines(); // Detiene cualquier mensaje anterior
        StartCoroutine(MessageRoutine(msg, duration));
    }

    private IEnumerator MessageRoutine(string msg, float duration)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(duration);
        messageText.text = ""; // Lo borra después de los segundos
    }
}