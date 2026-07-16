using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento (WASD y Shift)")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 12f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    private bool isGrounded;

    [SerializeField] private SanitySystem sanitySystem;

    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private bool isRunning;

    [Header("Cámara")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 4f;
    private float xRotation = 0f;

    [Header("Interacción")]
    [SerializeField] private Camera camaraJugador;
    [SerializeField] private float distanciaInteraccion = 3f;
    [SerializeField] private LayerMask capaNotas;

    [Header("Efecto de Desmayo / Cabeza Pesada")]
    [SerializeField] private float velocidadCaidaCabeza = 1.5f; 
    [SerializeField] private float anguloMaximoMareo = 15f; 
    private float inclinacionActualZ = 0f;


    [Header("Game Over")]
    private bool juegoTerminado = false;
    private bool puedeControlar = true;
    [SerializeField] private GameUIManager gameUI;

    void Start()
    {
        Time.timeScale = 1f;
        rb = GetComponent<Rigidbody>();
        sanitySystem.OnSanityReduced += GameOver;
        ActivarControles();
    }

    void Update()
    {
        // 1. SI NO PUEDE CONTROLAR (PAUSA/GAME OVER), SE DETIENE TODO EL UPDATE INMEDIATAMENTE
        if (!puedeControlar) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (NoteManager.Instance != null && NoteManager.Instance.IsNoteOpen())
            {
                NoteManager.Instance.CloseNote();
                return;
            }

            IntentarLeerNota();
        }

        if (NoteManager.Instance != null && NoteManager.Instance.IsNoteOpen())
        {
            return;
        }

        // Inputs de movimiento
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Rotación de cámara
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        CalcularInclinacionCabeza();

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, inclinacionActualZ);
        transform.Rotate(Vector3.up * mouseX);
    }

    void FixedUpdate()
    {
        if (!puedeControlar) return; // Evita que las físicas calculen movimiento en pausa

        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 targetVelocity = transform.TransformDirection(new Vector3(horizontalInput, 0f, verticalInput).normalized * currentSpeed);
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }

    void GameOver()
    {
        juegoTerminado = true;
        DesactivarControles();
        gameUI.MostrarGameOver();
    }

    void OnDestroy()
    {
        if (sanitySystem != null)
            sanitySystem.OnSanityReduced -= GameOver;
    }

    private void CalcularInclinacionCabeza()
    {
        float porcentajeLocura = 1f - (sanitySystem.CorduraActual / sanitySystem.CorduraMaxima);

        if (porcentajeLocura > 0.5f)
        {
            float intensidad = (porcentajeLocura - 0.5f) * 2f;
            inclinacionActualZ = Mathf.Sin(Time.time * velocidadCaidaCabeza) * (anguloMaximoMareo * intensidad);
        }
        else
        {
            inclinacionActualZ = Mathf.Lerp(inclinacionActualZ, 0f, Time.deltaTime * 5f);
        }
    }

    private void IntentarLeerNota()
    {
        RaycastHit hit;

        if (Physics.Raycast(
            camaraJugador.transform.position,
            camaraJugador.transform.forward,
            out hit,
            distanciaInteraccion,
            capaNotas))
        {
            NoteInteractable nota = hit.collider.GetComponent<NoteInteractable>();

            if (nota != null)
            {
                Debug.Log("Leyendo nota");
                NoteManager.Instance.OpenNote(nota.noteText);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            sanitySystem.SetZonaSegura(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SafeZone"))
        {
            sanitySystem.SetZonaSegura(false);
        }
    }

    public void ActivarControles()
    {
        puedeControlar = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DesactivarControles()
    {
        puedeControlar = false;

        // Limpiamos fuerzas e inputs pendientes para evitar "ghost walking"
        horizontalInput = 0f;
        verticalInput = 0f;
        isRunning = false;
        if (rb != null) 
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}