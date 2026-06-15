using System;
using UnityEngine;
using TMPro; // Asegúrate de tener esta librería para manejar TextMeshPro

public class WalkmanSystem : MonoBehaviour
{
    [SerializeField] private SanitySystem sanitySystem;

    [Header("Referencias")]
    [SerializeField] private Camera camaraJugador;
    [SerializeField] private BatteryManager batteryManager;
    [SerializeField] private AudioSource audioSource; // Este es para la música

    [Header("Efectos de Sonido")]
    [SerializeField] private AudioSource audioEfectos; // Arrastra acá el mismo AudioSource u otro secundario
    [SerializeField] private AudioClip clipEncender;   // Arrastra el "Click"
    [SerializeField] private AudioClip clipApagar;     // Arrastra el "Shhht"

    [Header("Interfaz UI (Texto Recoger)")]
    [SerializeField] private TextMeshProUGUI textoRecogerUI; // Arrastra el texto de tu Canvas acá

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

        // Nos aseguramos de que el texto arranque apagado
        if (textoRecogerUI != null)
        {
            textoRecogerUI.gameObject.SetActive(false);
        }

        OnWalkmanBatteryUpdated?.Invoke(bateriaActual, bateriaMaxima);
    }

    void Update()
    {
        //detecta constante para mostrar el texto en pantalla
        ChequearMirada();

        
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

    
    private void ChequearMirada()
    {
        
        if (textoRecogerUI == null) return;

        RaycastHit hit;
        // Lanzamos un rayo invisible desde la cámara
        if (Physics.Raycast(camaraJugador.transform.position, camaraJugador.transform.forward, out hit, distanciaRecojo, capaItems))
        {
            // Si el rayo choca contra CUALQUIER ítem recogible, mostramos el texto
            if (hit.collider.CompareTag("Item_Walkman") || hit.collider.CompareTag("Item_Llave") || hit.collider.CompareTag("Item_Bateria"))
            {
                textoRecogerUI.gameObject.SetActive(true);
            }
            else
            {
                textoRecogerUI.gameObject.SetActive(false);
            }
        }
        else
        {
            // Si no miramos a nada, apagamos el texto
            textoRecogerUI.gameObject.SetActive(false);
        }
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
                // Reproducir "Click"
                if (audioEfectos != null && clipEncender != null) audioEfectos.PlayOneShot(clipEncender);

                audioSource.Stop();
                audioSource.time = 0f;
                audioSource.Play();
            }
            else
            {
                // Reproducir "Shhht"
                if (audioEfectos != null && clipApagar != null) audioEfectos.PlayOneShot(clipApagar);

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
            if (hit.collider.CompareTag("Item_Walkman"))
            {
                tieneWalkman = true;

                OnWalkmanObtained?.Invoke(true);

                // Auto-encendido al recoger
                estaPrendido = true;

                if (audioSource != null)
                {
                    // Reproducir "Click" al agarrarlo
                    if (audioEfectos != null && clipEncender != null) audioEfectos.PlayOneShot(clipEncender);

                    audioSource.Stop();
                    audioSource.time = 0f;
                    audioSource.Play();
                }

                // Apagamos el texto antes de destruir el objeto para que no quede trabado en pantalla
                if (textoRecogerUI != null) textoRecogerUI.gameObject.SetActive(false);

                Destroy(hit.collider.gameObject);

                Debug.Log("Walkman recogido y encendido.");
            }

            // Aquí puedes agregar otros "if" para recoger llaves o pilas
            // else if (hit.collider.CompareTag("Item_Llave")) { ... }
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
                // Reproducir "Shhht" porque se apagó solo
                if (audioEfectos != null && clipApagar != null) audioEfectos.PlayOneShot(clipApagar);

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