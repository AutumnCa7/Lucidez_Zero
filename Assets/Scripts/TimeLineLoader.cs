using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineLoader : MonoBehaviour
{
    [SerializeField] private string siguienteEscena = "MainMenu";
    [SerializeField] private float tiempoEspera = 5f;

    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        director.stopped += AlTerminarTimeline;
    }

    private void OnDisable()
    {
        director.stopped -= AlTerminarTimeline;
    }

    private void AlTerminarTimeline(PlayableDirector director)
    {
        StartCoroutine(EsperarYCambiarEscena());
    }

    private IEnumerator EsperarYCambiarEscena()
    {
        yield return new WaitForSeconds(tiempoEspera);

        Time.timeScale = 1f;
        SceneManager.LoadScene(siguienteEscena);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}