using UnityEngine;

public class FlashlightSystem : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera camaraJugador; 
    [SerializeField] private GameObject linternaEnMano; 
    [SerializeField] private Light luzLinterna; 
    
    [Header("Interacción")]
    [SerializeField] private float distanciaRecojo = 10f; 
    [SerializeField] private LayerMask capaItems;

    [Header("Batería de Supervivencia")]
    [SerializeField] private float bateriaActual = 100f;
    [SerializeField] private float bateriaMaxima = 100f;
    [SerializeField] private float velocidadDrenado = 2f; // Baja solo si está prendida
    [SerializeField] private float recargaPorPila = 40f; 
    
    private bool tieneLinterna = false;
    private bool estaPrendida = false;
    private float intensidadOriginal; 

    void Start()
    {
        if (linternaEnMano != null) linternaEnMano.SetActive(false);
        if (luzLinterna != null) intensidadOriginal = luzLinterna.intensity;
        bateriaActual = bateriaMaxima;
    }

    void Update()
    {
        if (camaraJugador == null) return;

        // --- LÓGICA DE CONSUMO: Solo gasta si la tienes Y está prendida ---
        if (tieneLinterna && estaPrendida && bateriaActual > 0)
        {
            bateriaActual -= velocidadDrenado * Time.deltaTime;

            // El parpadeo visual ocurre mientras se gasta
            ControlarParpadeoVisual();

            if (bateriaActual <= 0)
            {
                bateriaActual = 0;
                ForzarApagadoTotal();
            }
        }

        // Interacciones normales
        if (Input.GetKeyDown(KeyCode.E)) IntentarRecoger();
        if (tieneLinterna && Input.GetKeyDown(KeyCode.F)) AlternarLuz();
        
        Debug.DrawRay(camaraJugador.transform.position, camaraJugador.transform.forward * distanciaRecojo, Color.yellow);
    }

    private void ControlarParpadeoVisual()
    {
        float ratio = bateriaActual / bateriaMaxima;
        if (ratio < 0.20f) // Umbral de parpadeo al 20%
        {
            float ruido = Mathf.PerlinNoise(Time.time * 25f, 0f);
            luzLinterna.intensity = intensidadOriginal * ruido;
        }
        else
        {
            luzLinterna.intensity = intensidadOriginal;
        }
    }

    private void AlternarLuz()
    {
        if (bateriaActual > 0)
        {
            estaPrendida = !estaPrendida;
            luzLinterna.enabled = estaPrendida;
            
            // Si la apagamos, restauramos la intensidad original por si estaba parpadeando
            if (!estaPrendida) luzLinterna.intensity = intensidadOriginal;
        }
    }

    private void ForzarApagadoTotal()
    {
        estaPrendida = false;
        if (luzLinterna != null) luzLinterna.enabled = false;
        Debug.Log("La batería se ha agotado. Presionar F no hará nada hasta recargar.");
    }

    private void IntentarRecoger()
    {
        RaycastHit hit;
        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out hit, distanciaRecojo, capaItems))
        {
            if (hit.collider.CompareTag("Item_Linterna"))
            {
                tieneLinterna = true;
                linternaEnMano.SetActive(true);
                Destroy(hit.collider.gameObject);
                
                // --- AUTO-ENCENDIDO AL RECOGER ---
                estaPrendida = true;
                if (luzLinterna != null) luzLinterna.enabled = true;
                
                Debug.Log("Linterna recogida y encendida. Gastando batería...");
            }
            else if (hit.collider.CompareTag("Item_Bateria"))
            {
                Recargar(hit.collider.gameObject);
            }
        }
    }

    private void Recargar(GameObject pila)
    {
        bateriaActual += recargaPorPila;
        if (bateriaActual > bateriaMaxima) bateriaActual = bateriaMaxima;
        Destroy(pila);
        Debug.Log("Recarga exitosa. Batería al " + bateriaActual + "%");
    }
}