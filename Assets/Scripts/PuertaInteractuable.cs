using UnityEngine;

public class PuertaInteractuable : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [SerializeField] private float anguloApertura = 90f; // Cuánto se abre la puerta
    [SerializeField] private float velocidadApertura = 3f; // Qué tan rápido se mueve
    
    private bool estaAbierta = false;
    private bool jugadorCerca = false;
    
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        rotacionCerrada = transform.rotation;
        
        // Calculamos cuál será la rotación "abierta" sumando el ángulo en el eje Y
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + anguloApertura, transform.eulerAngles.z);
    }

    void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            estaAbierta = !estaAbierta; // Cambiamos el estado (de abierta a cerrada o viceversa)
        }

        Quaternion rotacionObjetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadApertura);
    }

    // Detectar cuando el jugador ENTRA en el área del Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
        }
    }

    // Detectar cuando el jugador SALE del área del Trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }
}