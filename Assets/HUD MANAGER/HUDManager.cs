using UnityEngine;
using TMPro; 
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI interactionText; 
    [SerializeField] private TextMeshProUGUI messageText; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        
        interactionText.text = "";
        messageText.text = "";
    }

    
    public void ShowInteraction(string text)
    {
        interactionText.text = text;
    }

    
    public void HideInteraction()
    {
        interactionText.text = "";
    }

    
    public void ShowMessage(string msg, float duration = 3f)
    {
        StopAllCoroutines(); 
        StartCoroutine(MessageRoutine(msg, duration));
    }

    private IEnumerator MessageRoutine(string msg, float duration)
    {
        messageText.text = msg;
        yield return new WaitForSeconds(duration);
        messageText.text = ""; 
    }
}