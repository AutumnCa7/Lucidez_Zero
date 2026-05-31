using UnityEngine;
using UnityEngine.Video;

public class ControladorTele : MonoBehaviour
{
    [Header("Componentes")]
    public GameObject objetoPantalla;
    public VideoPlayer reproductorVideo;

    private bool estaEncendida = false;

    void Start()
    {
        if (objetoPantalla != null)
        {
            objetoPantalla.SetActive(false);
        }
    }

    public void AlternarTelevisor()
    {
        if (reproductorVideo == null || objetoPantalla == null) return;

        if (!estaEncendida)
        {
            objetoPantalla.SetActive(true);
            reproductorVideo.Play();
            estaEncendida = true;
        }
        else
        {
            reproductorVideo.Stop();
            objetoPantalla.SetActive(false);
            estaEncendida = false;
        }
    }
}