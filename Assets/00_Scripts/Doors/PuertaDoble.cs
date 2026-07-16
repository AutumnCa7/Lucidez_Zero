using UnityEngine;

public class PuertaDoble : MonoBehaviour
{
    [Header("Configuración de Bisagras")]
    [SerializeField] private Transform bisagraIzquierda;
    [SerializeField] private Transform bisagraDerecha;

    [Header("Ajustes de Apertura")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidadApertura = 3f;

    [Header("Ajustes de Colisión (Parche)")]
    [SerializeField] private float tiempoDeMovimiento = 3f;

    [Header("Audio de la Puerta Doble")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoAbrir;
    [SerializeField] private AudioClip sonidoCerrar;

    private bool estaAbierta = false;
    private bool jugadorCerca = false;

    private Quaternion cerradaIzq, abiertaIzq;
    private Quaternion cerradaDer, abiertaDer;

    void Start()
    {
        cerradaIzq = bisagraIzquierda.localRotation;
        cerradaDer = bisagraDerecha.localRotation;

        abiertaIzq = Quaternion.Euler(bisagraIzquierda.localEulerAngles.x, bisagraIzquierda.localEulerAngles.y + anguloApertura, bisagraIzquierda.localEulerAngles.z);
        abiertaDer = Quaternion.Euler(bisagraDerecha.localEulerAngles.x, bisagraDerecha.localEulerAngles.y - anguloApertura, bisagraDerecha.localEulerAngles.z);

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

            StartCoroutine(IgnorarColisionTemporalmente());

            if (audioSource != null)
            {
                if (estaAbierta && sonidoAbrir != null)
                {
                    audioSource.PlayOneShot(sonidoAbrir);
                }
                else if (!estaAbierta && sonidoCerrar != null)
                {
                    audioSource.PlayOneShot(sonidoCerrar);
                }
            }
        }

        Quaternion objetivoIzq = estaAbierta ? abiertaIzq : cerradaIzq;
        Quaternion objetivoDer = estaAbierta ? abiertaDer : cerradaDer;

        bisagraIzquierda.localRotation = Quaternion.Slerp(bisagraIzquierda.localRotation, objetivoIzq, Time.deltaTime * velocidadApertura);
        bisagraDerecha.localRotation = Quaternion.Slerp(bisagraDerecha.localRotation, objetivoDer, Time.deltaTime * velocidadApertura);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = false;
    }

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