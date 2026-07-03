using UnityEngine;

public class PuertaInteractuable : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidadApertura = 3f;

    [Header("Ajustes de Colisión (Parche)")]
    [SerializeField] private float tiempoDeMovimiento = 2.5f; // Tiempo para que no te empuje

    [Header("Audio de la Puerta")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoAbrir;
    [SerializeField] private AudioClip sonidoCerrar;

    private bool estaAbierta = false;
    private bool jugadorCerca = false;

    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        // CAMBIO A LOCAL: Para evitar bugs si el hospital está rotado
        rotacionCerrada = transform.localRotation;
        rotacionAbierta = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + anguloApertura, transform.localEulerAngles.z);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            estaAbierta = !estaAbierta;

            // ACTIVADOR: Arranca el modo fantasma al mismo tiempo que el sonido
            StartCoroutine(IgnorarColisionTemporalmente());

            // reproducir el sonido al apretar la E
            if (audioSource != null)
            {
                if (estaAbierta && sonidoAbrir != null)
                {
                    audioSource.PlayOneShot(sonidoAbrir); // Suena al abrir
                }
                else if (!estaAbierta && sonidoCerrar != null)
                {
                    audioSource.PlayOneShot(sonidoCerrar); // Suena al cerrar
                }
            }
        }

        Quaternion rotacionObjetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        
        // CAMBIO A LOCAL: Movimiento suave relativo al marco
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotacionObjetivo, Time.deltaTime * velocidadApertura);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = false;
    }

    // CORRUTINA: Apaga la madera automáticamente para que pases limpio
    private System.Collections.IEnumerator IgnorarColisionTemporalmente()
    {
        Collider[] todosLosColliders = GetComponentsInChildren<Collider>();

        foreach (Collider col in todosLosColliders)
        {
            if (col.isTrigger == false) col.enabled = false;
        }
        
        yield return new WaitForSeconds(tiempoDeMovimiento);
        
        foreach (Collider col in todosLosColliders)
        {
            if (col.isTrigger == false) col.enabled = true;
        }
    }
}