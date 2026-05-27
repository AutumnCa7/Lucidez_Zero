using UnityEngine;
using System.Collections; // Coroutines

public class PlayerCameraFocus : MonoBehaviour
{
    [Header("Focus Settings")]
    [SerializeField] private float focusSpeed = 3f;
    [SerializeField] private float focusDuration = 3f; 

    private PlayerController playerController;
    private Transform targetToLookAt;
    private bool isFocusing = false;
    private Coroutine focusCoroutine; 

    private void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        if (playerController == null) playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (isFocusing && targetToLookAt != null)
        {
            Vector3 directionToTarget = targetToLookAt.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * focusSpeed);
        }
    }

    public void StartFocus(Transform target)
    {
        if (isFocusing) return; 

        targetToLookAt = target;
        isFocusing = true;

        // Desactivamos el control
        if (playerController != null) playerController.enabled = false;

        // Iniciamos el temporizador
        focusCoroutine = StartCoroutine(FocusTimer());
    }

    
    private IEnumerator FocusTimer()
    {
        yield return new WaitForSeconds(focusDuration);
        StopFocus();
    }

    public void StopFocus()
    {
        if (!isFocusing) return; 

        isFocusing = false;
        targetToLookAt = null;

        
        if (playerController != null) playerController.enabled = true;

        // Limpiamos la corrut por si se detuvo antes de tiempo
        if (focusCoroutine != null)
        {
            StopCoroutine(focusCoroutine);
        }
    }
}