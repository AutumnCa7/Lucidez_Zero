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

    [Header("Sistema de Cordura")]
    [SerializeField] private float corduraMaxima = 100f;
    [SerializeField] private float velocidadDrenado = 3f;
    [SerializeField] private Slider uiBarraCordura;
    [SerializeField] private Image uiImagenRelleno;
    [SerializeField] private Color colorSaludable = Color.green;
    [SerializeField] private Color colorCritico = Color.red;
    [Range(0f, 1f)][SerializeField] private float umbralPanico = 0.2f;

    private float corduraActual;
    private bool enZonaSegura = false;

    [Header("Efecto de Oscuridad (Vignette)")]
    [SerializeField] private Volume volumePostProcesado;
    [SerializeField] private float intensidadMaximaVignette = 0.6f;
    private Vignette vignetteEffect;

    [Header("Game Over")]
    private bool juegoTerminado = false;
    private bool puedeControlar = true;
    [SerializeField] private GameUIManager gameUI;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        corduraActual = corduraMaxima;

        if (uiBarraCordura != null)
        {
            uiBarraCordura.maxValue = corduraMaxima;
            uiBarraCordura.value = corduraActual;

            if (uiImagenRelleno != null) uiImagenRelleno.color = colorSaludable;
        }

        if (volumePostProcesado != null && volumePostProcesado.profile.TryGet(out vignetteEffect))
        {
            vignetteEffect.intensity.value = 0f;
        }
        else
        {
            Debug.LogWarning("No se encontró el efecto Vignette. Asegúrate de tener el Global Volume arrastrado.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!puedeControlar) return;

        if (!enZonaSegura)
        {
            PerderCordura(velocidadDrenado * Time.deltaTime);
        }
        else if (corduraActual < corduraMaxima)
        {
            corduraActual += (velocidadDrenado * 2) * Time.deltaTime;
            if (corduraActual > corduraMaxima) corduraActual = corduraMaxima;
            ActualizarUI();
        }

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

    public void PerderCordura(float cantidad)
    {
        if (juegoTerminado) return;

        corduraActual -= cantidad;
        ActualizarUI();

        if (corduraActual <= 0)
        {
            corduraActual = 0;
            juegoTerminado = true;
            DesactivarControles();
            gameUI.MostrarGameOver();
        }
    }

    private void ActualizarUI()
    {
        if (uiBarraCordura == null || uiImagenRelleno == null) return;

        uiBarraCordura.value = corduraActual;
        float porcentaje = corduraActual / corduraMaxima;

        Color colorActual = Color.Lerp(colorCritico, colorSaludable, porcentaje);
        uiImagenRelleno.color = colorActual;

        if (vignetteEffect != null)
        {
            float nuevaIntensidad = Mathf.Lerp(0f, intensidadMaximaVignette, 1f - porcentaje);
            vignetteEffect.intensity.value = nuevaIntensidad;
        }

        if (porcentaje <= umbralPanico)
        {
            float parpadeo = Mathf.Sin(Time.time * 20f);
            if (parpadeo > 0) uiImagenRelleno.color = Color.white;
        }
    }

    private void CalcularInclinacionCabeza()
    {
        float porcentajeLocura = 1f - (corduraActual / corduraMaxima);

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
        if (other.CompareTag("LuzSegura")) enZonaSegura = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LuzSegura")) enZonaSegura = false;
    }

    public void DesactivarControles()
    {
        puedeControlar = false;
    }
}