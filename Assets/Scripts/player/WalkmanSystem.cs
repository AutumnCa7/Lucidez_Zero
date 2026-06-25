using System;
using UnityEngine;

public class WalkmanSystem : MonoBehaviour
{
    [SerializeField] private SanitySystem sanitySystem;

    [Header("Referencias")]
    [SerializeField] private Camera camaraJugador;
    [SerializeField] private BatteryManager batteryManager;

    [Header("Interacción")]
    [SerializeField] private float distanciaRecojo = 1.5f; //no es el problema
    [SerializeField] private LayerMask capaItems;

    [Header("Batería de Supervivencia")]
    [SerializeField] private float bateriaActual = 75f;
    [SerializeField] private float bateriaMaxima = 75f;
    [SerializeField] private float velocidadDrenado = 5f; // Baja solo si está prendida
    [SerializeField] private float recargaPorPila = 40f;

    [SerializeField] private float sanityRestoring= 0.5f; //sube la cordura lentamente al utilizar el objeto
    private bool tieneWalkman = false;
    private bool estaPrendido = false;

    public event Action<float, float> OnWalkmanBatteryUpdated;
    public event Action<bool> OnWalkmanObtained;

    void Start()
    {
        bateriaActual = bateriaMaxima;

        OnWalkmanBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) IntentarRecoger();
        if (tieneWalkman && Input.GetKeyDown(KeyCode.G))
        {
            if (bateriaActual > 0)
            {
                estaPrendido = !estaPrendido;
            }
        }
        if (estaPrendido && bateriaActual > 0)
        {
            LoseBattery();
            RestoreSanity(sanityRestoring * Time.deltaTime);


        } //mas tarde me gustaria especificar
        RecargarInput();
    }

    private void IntentarRecoger()
    {
        RaycastHit hit;
        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out hit, distanciaRecojo, capaItems))
        {
            Debug.Log("COLIDER WALKMAN"+hit.collider.name);
            if (hit.collider.CompareTag("Item_Walkman"))
            {
                tieneWalkman = true;
      
                OnWalkmanObtained?.Invoke(true);

                Destroy(hit.collider.gameObject);

                // --- AUTO-ENCENDIDO AL RECOGER ---
                estaPrendido = true;

                Debug.Log("Walkman recogida y encendida. Gastando batería...");
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
    private void Recargar(float amount)
    {
        bateriaActual += amount;
        if (bateriaActual > bateriaMaxima) bateriaActual = bateriaMaxima;

        OnWalkmanBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);

        
        Debug.Log("Recarga exitosa. Batería al " + bateriaActual + "%");
    }

    private void RecargarInput ()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
         
            if (bateriaActual  < bateriaMaxima && batteryManager.ConsumeBattery())
            {
                Recargar(recargaPorPila);
            }
        }
    }

    public void RestoreSanity(float cantidad )
    {
       
        if (sanitySystem != null)
        {
            sanitySystem.ModifySanity(cantidad);
            OnWalkmanBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);

        }
    }

    //el walkman se da a entender que esta prendido (tienewalkman=true) con el sonido, no utiliza ningun sprite como la linterna. Se debe programar al momento de aplicar sonido al juego


}
