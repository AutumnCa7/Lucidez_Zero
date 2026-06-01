using UnityEngine;

public class WalkmanSystem : MonoBehaviour
{
    [SerializeField] private SanitySystem sanitySystem;

    [Header("Referencias")]
    [SerializeField] private Camera camaraJugador;
    [SerializeField] private GameObject walkmanEnMano;

    [Header("Interacción")]
    [SerializeField] private float distanciaRecojo = 10f;
    [SerializeField] private LayerMask capaItems;

    [Header("Batería de Supervivencia")]
    [SerializeField] private float bateriaActual = 75f;
    [SerializeField] private float bateriaMaxima = 75f;
    [SerializeField] private float velocidadDrenado = 2f; // Baja solo si está prendida
    [SerializeField] private float recargaPorPila = 40f;

    [SerializeField] private float sanityRestoring; //sube la cordura lentamente al utilizar el objeto
    private bool tieneWalkman = false;
    private bool estaPrendido = false;

    void Start()
    {
        bateriaActual = bateriaMaxima;

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) IntentarRecoger();
        if (tieneWalkman && Input.GetKeyDown(KeyCode.G))
        { estaPrendido = !estaPrendido; }

        if (estaPrendido && bateriaActual > 0)
        {
            LoseBattery();
            RestoreSanity(sanityRestoring * Time.deltaTime);
        } //mas tarde me gustaria especificar
    }

    private void IntentarRecoger()
    {
        RaycastHit hit;
        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out hit, distanciaRecojo, capaItems))
        {
            if (hit.collider.CompareTag("Item_Walkman"))
            {
                tieneWalkman = true;
                walkmanEnMano.SetActive(true);
                Destroy(hit.collider.gameObject);

                // --- AUTO-ENCENDIDO AL RECOGER ---
                estaPrendido = true;

                Debug.Log("Walkman recogida y encendida. Gastando batería...");
            }
            else if (hit.collider.CompareTag("Item_Bateria"))
            {
                Recargar(hit.collider.gameObject);
            }
        }
    }
    void LoseBattery()
    {
        if (tieneWalkman && estaPrendido && bateriaActual > 0)
        {
            bateriaActual -= velocidadDrenado * Time.deltaTime;
        }

        if (bateriaActual <= 0)
        {
            bateriaActual = 0;
            estaPrendido=false;
        }
    }
    private void Recargar(GameObject pila)
    {
        bateriaActual += recargaPorPila;
        if (bateriaActual > bateriaMaxima) bateriaActual = bateriaMaxima;
        Destroy(pila);
        Debug.Log("Recarga exitosa. Batería al " + bateriaActual + "%");
    }

    public void RestoreSanity(float cantidad)
    {
        if (sanitySystem != null)
        {
            sanitySystem.ModifySanity(cantidad);
        }
    }

    public void NullLoseSanityBySound()
    {

    }

}

/*Cosas que corregir:
Baja muy lento la bateria
Sube un montonazo la cordura, hay que bajarlo
Aplicar sistema de gestion de pilas, las pilas se utilizan automaticamente y cargan los dos objetos.
*/