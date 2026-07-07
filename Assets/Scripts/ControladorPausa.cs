using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPausa : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject menuPausaUI;
    [SerializeField] private GameObject guiaJuegoUI;

    [Header("Referencias de Componentes")]
    // Cambiado a [SerializeField] para que lo puedas arrastrar en el Inspector
    [SerializeField] private PlayerController playerController; 

    private bool juegoPausado = false;
    private bool guiaAbierta = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (guiaAbierta)
            {
                OcultarGuia();
            }
            else if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        menuPausaUI.SetActive(false);
        guiaJuegoUI.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
        guiaAbierta = false;

        if (playerController != null) 
            playerController.ActivarControles();
        else
            Debug.LogError("¡Falta asignar el PlayerController en el ControladorPausa!");
    }

    public void Pausar()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;

        if (playerController != null) 
            playerController.DesactivarControles();
        else
            Debug.LogError("¡Falta asignar el PlayerController en el ControladorPausa!");
    }

    public void MostrarGuia()
    {
        menuPausaUI.SetActive(false);
        guiaJuegoUI.SetActive(true);
        guiaAbierta = true;
    }

    public void OcultarGuia()
    {
        guiaJuegoUI.SetActive(false);
        menuPausaUI.SetActive(true);
        guiaAbierta = false;
    }

    public void IrAlMenuPrincipal(string nombreEscena)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}