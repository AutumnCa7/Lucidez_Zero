using System;
using UnityEngine;

public class FlashlightSystem : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Camera camaraJugador; 
    [SerializeField] private GameObject linternaEnMano; 
    [SerializeField] private Light luzLinterna;
    [SerializeField] private BatteryManager batteryManager;
    
    [Header("Interacción")]
    [SerializeField] private float distanciaRecojo = 10f; 
    [SerializeField] private LayerMask capaItems;

    [Header("Batería de Supervivencia")]
    [SerializeField] private float bateriaActual = 100f;
    [SerializeField] private float bateriaMaxima = 100f;
    [SerializeField] private float velocidadDrenado = 2f; // Baja solo si está prendida
    [SerializeField] private float recargaPorPila = 40f;

    public event Action<float, float> OnFlashlightBatteryUpdated;
    public event Action<bool> OnFlashlightObtained;

    private bool tieneLinterna = false;
    private bool estaPrendida = false;
    private float intensidadOriginal;

    public bool EstaPrendida => estaPrendida;

    void Start()
    {
        if (linternaEnMano != null) linternaEnMano.SetActive(false);
        if (luzLinterna != null) intensidadOriginal = luzLinterna.intensity;
        bateriaActual = bateriaMaxima;

        OnFlashlightBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);
    }

    void Update()
    {
        if (camaraJugador == null) return;

        // --- LÓGICA DE CONSUMO: Solo gasta si la tienes Y está prendida ---
        if (tieneLinterna && estaPrendida && bateriaActual > 0)
        {
            bateriaActual -= velocidadDrenado * Time.deltaTime;

            OnFlashlightBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);

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
        RecargarInput();
        
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
               
                OnFlashlightObtained?.Invoke(true);

                Destroy(hit.collider.gameObject);
                
                // --- AUTO-ENCENDIDO AL RECOGER ---
                estaPrendida = true;
                if (luzLinterna != null) luzLinterna.enabled = true;
                
                Debug.Log("Linterna recogida y encendida. Gastando batería...");
            }
            else if (hit.collider.CompareTag("Item_Bateria"))
            {
                Debug.Log("Flashlight recogio bateria");
                batteryManager.AddBattery();
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void Recargar(float amount)//amount=recargarPorPila
    {
        bateriaActual += amount;
        if (bateriaActual > bateriaMaxima) bateriaActual = bateriaMaxima;

        OnFlashlightBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);

        
        Debug.Log("Recarga exitosa. Batería al " + bateriaActual + "%");
    }
    private void RecargarInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            if (bateriaActual < bateriaMaxima && batteryManager.ConsumeBattery())
            {

                Recargar(recargaPorPila);
            }
        }
    }
}