using UnityEngine;
using UnityEngine.SceneManagement; // Obligatorio para cambiar de escenas

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de Escenas")]
    [SerializeField] private string nombreDelNivel = "SampleScene";
    [SerializeField] private string nombreDelMenu = "MainMenu";

    // 1. Método para iniciar el juego
    public void Jugar()
    {
        // Reiniciamos el tiempo por si venimos de una pausa
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreDelNivel);
    }

    // 2. Método para volver al menú principal
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreDelMenu);
    }

    // 3. Método para cerrar el juego
    public void Salir()
    {
        Application.Quit(); // Cierra el .exe o la app
    }
}