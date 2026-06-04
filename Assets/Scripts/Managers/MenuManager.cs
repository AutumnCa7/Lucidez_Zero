using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Paneles de la Interfaz")]
    public GameObject menuPrincipal;
    public GameObject panelHowToPlay;
    public GameObject panelDevelopers;

    // 1. BOTÓN START (Carga la escena "SampleScene" directamente sin pedir texto)
    public void Jugar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    // 2. BOTÓN HOW TO PLAY (Abre)
    public void OpenHowToPlay()
    {
        menuPrincipal.SetActive(false);
        panelHowToPlay.SetActive(true);
    }

    // BOTÓN VOLVER DE CONTROLES (Cierra)
    public void CloseHowToPlay()
    {
        panelHowToPlay.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    // 3. BOTÓN DEVELOPERS (Abre)
    public void OpenDevelopers()
    {
        menuPrincipal.SetActive(false);
        panelDevelopers.SetActive(true);
    }

    // BOTÓN VOLVER DE CRÉDITOS (Cierra)
    public void CloseDevelopers()
    {
        panelDevelopers.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    // 4. BOTÓN EXIT
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}