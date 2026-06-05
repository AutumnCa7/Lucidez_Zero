using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    public PlayerController player;

    public void MostrarGameOver()
    {
        player.DesactivarControles();
        gameOverPanel.SetActive(true);
        PausarJuego();
    }

    public void MostrarVictoria()
    {
        player.DesactivarControles();
        SceneManager.LoadScene("Final Cinematic");
    }

    private void PausarJuego()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}