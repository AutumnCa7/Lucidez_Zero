using UnityEngine;

public class PuertaInteractuable : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [SerializeField] private float anguloApertura = 90f;
    [SerializeField] private float velocidadApertura = 3f;

    
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
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + anguloApertura, transform.eulerAngles.z);

        
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadApertura);
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