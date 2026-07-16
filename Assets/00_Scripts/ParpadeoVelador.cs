using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ParpadeoVelador : MonoBehaviour
{
    private Light2D luz;
    public float intensidadMinima = 0.8f;
    public float intensidadMaxima = 1.2f;
    public float velocidad = 0.1f;

    void Start()
    {
        luz = GetComponent<Light2D>();
        InvokeRepeating("Flicker", 0, velocidad);
    }

    void Flicker()
    {
        luz.intensity = Random.Range(intensidadMinima, intensidadMaxima);
    }
}