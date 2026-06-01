using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteManager : MonoBehaviour
{
    [SerializeField] private SanitySystem sanitySystem;

    [SerializeField] private Volume volumePostProcesado;
    [SerializeField] private float intensidadMaximaVignette = 0.6f;
    private Vignette vignetteEffect;

    void Start()
    {

        volumePostProcesado.profile.TryGet(out vignetteEffect);
        sanitySystem.OnSanityUpdated += UpdateVignette;

    }

    private void UpdateVignette(float actual, float max)
    {
        if (vignetteEffect == null) return;
        float porcentaje= actual/max;
        vignetteEffect.intensity.value = Mathf.Lerp(0f, intensidadMaximaVignette, 1f - porcentaje);
    }
    
}
