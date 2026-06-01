using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class ControladorTele : MonoBehaviour
{
    [Header("Componentes de la Tele (Opcionales)")]
    public GameObject objetoPantalla;
    public VideoPlayer reproductorVideo;
    public Light luzDeLaTele;

    [Header("Configuración del Jumpscare")]
    public GameObject imagenJumpscare;
    public float dudaricionSusto = 1.5f;

    private bool estaEncendida = false;

    void Start()
    {
        if (objetoPantalla != null) objetoPantalla.SetActive(false);
        if (luzDeLaTele != null) luzDeLaTele.enabled = false;
        if (imagenJumpscare != null) imagenJumpscare.SetActive(false);
    }

    public void AlternarTelevisor()
    {
        if (!estaEncendida)
        {
            if (objetoPantalla != null) objetoPantalla.SetActive(true);
            if (reproductorVideo != null) reproductorVideo.Play();
            if (luzDeLaTele != null) luzDeLaTele.enabled = true;

            if (imagenJumpscare != null)
            {
                StartCoroutine(RutinaJumpscare());
            }

            estaEncendida = true;
        }
        else
        {
            if (reproductorVideo != null) reproductorVideo.Stop();
            if (objetoPantalla != null) objetoPantalla.SetActive(false);
            if (luzDeLaTele != null) luzDeLaTele.enabled = false;

            estaEncendida = false;
        }
    }

    private IEnumerator RutinaJumpscare()
    {
        imagenJumpscare.SetActive(true);
        Debug.Log("¡BUUUH!");

        yield return new WaitForSeconds(dudaricionSusto);

        imagenJumpscare.SetActive(false);
    }
}