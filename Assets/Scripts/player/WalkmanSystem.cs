using System;
using UnityEngine;

public class WalkmanSystem : MonoBehaviour
{
    [SerializeField] private SanitySystem sanitySystem;

    [Header("Referencias")]
    [SerializeField] private Camera camaraJugador;
    [SerializeField] private BatteryManager batteryManager;
    [SerializeField] private AudioSource audioSource;

    [Header("Interacción")]
    [SerializeField] private float distanciaRecojo = 10f;
    [SerializeField] private LayerMask capaItems;

    [Header("Batería de Supervivencia")]
    [SerializeField] private float bateriaActual = 75f;
    [SerializeField] private float bateriaMaxima = 75f;
    [SerializeField] private float velocidadDrenado = 5f;
    [SerializeField] private float recargaPorPila = 40f;

    [Header("Cordura")]
    [SerializeField] private float sanityRestoring = 0.5f;

    private bool tieneWalkman = false;
    private bool estaPrendido = false;

    public event Action<float, float> OnWalkmanBatteryUpdated;
    public event Action<bool> OnWalkmanObtained;

    void Start()
    {
        bateriaActual = bateriaMaxima;

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
        }

        OnWalkmanBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            IntentarRecoger();
        }

        if (tieneWalkman && Input.GetKeyDown(KeyCode.G))
        {
            ToggleWalkman();
        }

        if (estaPrendido && bateriaActual > 0)
        {
            LoseBattery();
            RestoreSanity(sanityRestoring * Time.deltaTime);
        }

        RecargarInput();
    }

    private void ToggleWalkman()
    {
        if (bateriaActual <= 0)
            return;

        estaPrendido = !estaPrendido;

        if (audioSource != null)
        {
            if (estaPrendido)
            {
                // Reinicia siempre la canción desde el principio
                audioSource.Stop();
                audioSource.time = 0f;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }

        Debug.Log("Walkman " + (estaPrendido ? "encendido" : "apagado"));
    }

    private void IntentarRecoger()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            camaraJugador.transform.position,
            camaraJugador.transform.forward,
            out hit,
            distanciaRecojo,
            capaItems))
        {
            Debug.Log("COLIDER WALKMAN " + hit.collider.name);

            if (hit.collider.CompareTag("Item_Walkman"))
            {
                tieneWalkman = true;

                OnWalkmanObtained?.Invoke(true);

                // Auto-encendido al recoger
                estaPrendido = true;

                if (audioSource != null)
                {
                    audioSource.Stop();
                    audioSource.time = 0f;
                    audioSource.Play();
                }

                Destroy(hit.collider.gameObject);

                Debug.Log("Walkman recogido y encendido.");
            }
        }
    }

    private void LoseBattery()
    {
        bateriaActual -= velocidadDrenado * Time.deltaTime;

        if (bateriaActual <= 0)
        {
            bateriaActual = 0;
            estaPrendido = false;

            if (audioSource != null)
            {
                audioSource.Stop();
            }

            Debug.Log("Walkman sin batería.");
        }

        OnWalkmanBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);
    }

    private void Recargar(float amount)
    {
        bateriaActual += amount;

        if (bateriaActual > bateriaMaxima)
        {
            bateriaActual = bateriaMaxima;
        }

        OnWalkmanBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);

        Debug.Log("Recarga exitosa. Batería al " + bateriaActual + "%");
    }

    private void RecargarInput()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (bateriaActual < bateriaMaxima && batteryManager.ConsumeBattery())
            {
                Recargar(recargaPorPila);
            }
        }
    }

    public void RestoreSanity(float cantidad)
    {
        if (sanitySystem != null)
        {
            sanitySystem.ModifySanity(cantidad);
        }
    }
}