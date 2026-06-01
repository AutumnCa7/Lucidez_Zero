using UnityEngine;

public class PuertaDoble : MonoBehaviour
{
    [Header("Configuración de Bisagras")]
    [SerializeField] private Transform bisagraIzquierda;
    [SerializeField] private Transform bisagraDerecha;

    [Header("Ajustes de Apertura")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidadApertura = 3f;

    
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
        
        cerradaIzq = bisagraIzquierda.rotation;
        cerradaDer = bisagraDerecha.rotation;

         
        abiertaIzq = Quaternion.Euler(bisagraIzquierda.eulerAngles.x, bisagraIzquierda.eulerAngles.y + anguloApertura, bisagraIzquierda.eulerAngles.z);
        abiertaDer = Quaternion.Euler(bisagraDerecha.eulerAngles.x, bisagraDerecha.eulerAngles.y - anguloApertura, bisagraDerecha.eulerAngles.z);

        
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

        bisagraIzquierda.rotation = Quaternion.Slerp(bisagraIzquierda.rotation, objetivoIzq, Time.deltaTime * velocidadApertura);
        bisagraDerecha.rotation = Quaternion.Slerp(bisagraDerecha.rotation, objetivoDer, Time.deltaTime * velocidadApertura);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) jugadorCerca = false;
    }
}