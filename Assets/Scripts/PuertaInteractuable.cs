using UnityEngine;

public class PuertaInteractuable : MonoBehaviour
{
    [Header("Configuración de la Puerta")]
    [SerializeField] private float anguloApertura = 90f; 
    [SerializeField] private float velocidadApertura = 3f; 
    
    [Header("Ajustes de Colisión")]
    [SerializeField] private Collider colliderPuerta; // Sigue yendo el Box Collider de la madera
    [SerializeField] private float tiempoDeMovimiento = 1.5f;
    
    private bool estaAbierta = false;
    private bool jugadorCerca = false;
    
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    // ---> NUEVA VARIABLE: Guardará el cuerpo de tu personaje <---
    private Collider colliderJugador; 

    void Start()
    {
        // Guardamos la rotación inicial como "cerrada"
        rotacionCerrada = transform.rotation;
        rotacionAbierta = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + anguloApertura, transform.eulerAngles.z);
    }

    void Update()
    {
        // Si el jugador está en la zona y presiona la tecla E
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            estaAbierta = !estaAbierta; 
            StartCoroutine(IgnorarColisionTemporalmente());
        }

        // Movimiento suave usando Quaternion.Slerp (Interpolar rotaciones)
        Quaternion rotacionObjetivo = estaAbierta ? rotacionAbierta : rotacionCerrada;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, Time.deltaTime * velocidadApertura);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            // ---> ATRAPAMOS EL COLISIONADOR DE TU PERSONAJE AQUÍ <---
            colliderJugador = other; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
        }
    }

// ---> LA SOLUCIÓN DEFINITIVA (Búsqueda Automática) <---
    private System.Collections.IEnumerator IgnorarColisionTemporalmente()
    {
        // 1. Busca TODOS los colisionadores en la bisagra, la madera, los picaportes, etc.
        Collider[] todosLosColliders = GetComponentsInChildren<Collider>();

        // 2. Apaga toda la madera (ignora el trigger para que la tecla E siga funcionando)
        foreach (Collider col in todosLosColliders)
        {
            if (col.isTrigger == false) 
            {
                col.enabled = false;
            }
        }
        
        // 3. Espera a que termine la animación
        yield return new WaitForSeconds(tiempoDeMovimiento);
        
        // 4. Vuelve a encender todos los colisionadores sólidos
        foreach (Collider col in todosLosColliders)
        {
            if (col.isTrigger == false) 
            {
                col.enabled = true;
            }
        }
    }
}