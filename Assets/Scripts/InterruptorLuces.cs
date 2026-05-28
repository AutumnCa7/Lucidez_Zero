using UnityEngine;

public class InterruptorLuces : MonoBehaviour
{
    [SerializeField] private LightEventManager lightManager;

    private bool jugadorCerca = false;
    private bool usado = false;

    private void Update()
    {
        if (usado)
            return;

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            usado = true;

            lightManager.EncenderLuces();

            Debug.Log("Interruptor activado");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}