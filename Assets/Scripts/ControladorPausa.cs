using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorPausa : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject menuPausaUI;
    [SerializeField] private GameObject guiaJuegoUI; // <- NUEVA: Arrastra aquí el panel de la guía

    private bool juegoPausado = false;
    private bool guiaAbierta = false; // <- NUEVA: Controla si estamos viendo la guía

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Si la guía está abierta, Escape la cierra y vuelve al menú de pausa
            if (guiaAbierta)
            {
                OcultarGuia();
            }
            // Si el juego ya está pausado (pero la guía no está abierta), reanuda el juego
            else if (juegoPausado)
            {
                Reanudar();
            }
            // Si el juego corre normal, lo pausa
            else
            {
                Pausar();
            }
        }
    }

    public void Reanudar()
    {
        menuPausaUI.SetActive(false);
        guiaJuegoUI.SetActive(false); // Nos aseguramos de cerrar la guía también
        Time.timeScale = 1f;
        juegoPausado = false;
        guiaAbierta = false;
    }

    public void Pausar()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    // ==========================================
    // NUEVOS MÉTODOS PARA LA GUÍA DE JUEGO
    // ==========================================

    public void MostrarGuia()
    {
        menuPausaUI.SetActive(false); // Oculta los botones principales de pausa
        guiaJuegoUI.SetActive(true);   // Muestra la pantalla con la imagen de la guía
        guiaAbierta = true;
    }

    public void OcultarGuia()
    {
        guiaJuegoUI.SetActive(false);  // Oculta la guía
        menuPausaUI.SetActive(true);   // Reaparece el menú de pausa principal
        guiaAbierta = false;
    }

    // ==========================================

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