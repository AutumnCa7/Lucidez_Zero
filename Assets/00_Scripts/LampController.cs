using UnityEngine;

public class LampController : MonoBehaviour
{
    [SerializeField] private Collider zonaSegura;
    [SerializeField] private Light pointLight;
    [SerializeField] private ParticleSystem sparks;

    public void SetVisual(bool state)
    {
        pointLight.enabled = state;
    }

    public void ApagarCompleto()
    {
        pointLight.enabled = false;
        zonaSegura.enabled = false;
    }

    public void EncenderCompleto()
    {
        pointLight.enabled = true;
        zonaSegura.enabled = true;
    }
    public void PlaySparks()
    {
        if (sparks != null)
        {
            sparks.Play();
        }
    }
}