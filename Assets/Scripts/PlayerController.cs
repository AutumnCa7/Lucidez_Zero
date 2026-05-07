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

    [Header("Efecto de Desmayo / Cabeza Pesada")]
    [SerializeField] private float velocidadCaidaCabeza = 1.5f; // Qué tan rápido cabecea
    [SerializeField] private float anguloMaximoMareo = 15f; // Grados de inclinación hacia el hombro
    private float inclinacionActualZ = 0f;


    [Header("Game Over")]
    private bool juegoTerminado = false;
    private bool puedeControlar = true;
    [SerializeField] private GameUIManager gameUI;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sanitySystem.OnSanityReduced += GameOver;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

   
    void Update()
    {

        if (!puedeControlar) return;

        //perderCordura fue reemplazado por la clase SanitySystem para un manejo mas escalable de la mecanica a partir de Eventos

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isRunning = Input.GetKey(KeyCode.LeftShift);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // --- CÁLCULO DE CAÍDA DE CABEZA ---
        CalcularInclinacionCabeza();

        // Aplicamos la rotación normal del mouse + la inclinación Z del mareo
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, inclinacionActualZ);
        transform.Rotate(Vector3.up * mouseX);
    }

    void FixedUpdate()
    {
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
        sanitySystem.OnSanityReduced -= GameOver;
    }


    private void CalcularInclinacionCabeza()
    {
        float porcentajeLocura = 1f - (sanitySystem.CorduraActual / sanitySystem.CorduraMaxima);

        // Si la locura pasa de la mitad, empieza a caer la cabeza
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

    private void OnTriggerEnter(Collider other)
    {
        sanitySystem.SetZonaSegura(true);

    }

    private void OnTriggerExit(Collider other)
    {
        sanitySystem.SetZonaSegura(false);

    }

    public void DesactivarControles()
    {
        puedeControlar = false;
    }
}