using UnityEngine;

public class EventoApagarLuces : MonoBehaviour
{
    [SerializeField] private LightEventManager lightManager;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado)
            return;

        if (other.CompareTag("Player"))
        {
            activado = true;

            lightManager.ApagarLuces();

            Debug.Log("Evento de oscuridad activado");

            Destroy(gameObject);
        }
    }
}
