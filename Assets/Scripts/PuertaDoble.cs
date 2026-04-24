using UnityEngine;

public class PuertaDoble : MonoBehaviour
{
    [Header("Configuración de Bisagras")]
    [SerializeField] private Transform bisagraIzquierda;
    [SerializeField] private Transform bisagraDerecha;
    
    [Header("Ajustes de Apertura")]
    [SerializeField] private float anguloApertura = 90f; 
    [SerializeField] private float velocidadApertura = 3f;

    private bool estaAbierta = false;
    private bool jugadorCerca = false;

    // Variables para guardar las rotaciones
    private Quaternion cerradaIzq, abiertaIzq;
    private Quaternion cerradaDer, abiertaDer;

    void Start()
    {
        // 1. Guardamos la posición original (cerrada) de ambas puertas
        cerradaIzq = bisagraIzquierda.rotation;
        cerradaDer = bisagraDerecha.rotation;

        // 2. Calculamos a dónde deben rotar al abrirse. 
        // Nota: A una le sumamos el ángulo y a la otra se lo restamos para que se abran como un libro.
        abiertaIzq = Quaternion.Euler(bisagraIzquierda.eulerAngles.x, bisagraIzquierda.eulerAngles.y + anguloApertura, bisagraIzquierda.eulerAngles.z);
        abiertaDer = Quaternion.Euler(bisagraDerecha.eulerAngles.x, bisagraDerecha.eulerAngles.y - anguloApertura, bisagraDerecha.eulerAngles.z);
    }

    void Update()
    {
        // Si el jugador está cerca y presiona E
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            estaAbierta = !estaAbierta; // Cambia el estado de abrir a cerrar o viceversa
        }

        // Interpolación suave para ambas bisagras al mismo tiempo
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