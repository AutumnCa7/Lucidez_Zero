using System;
using UnityEngine;

public class SanitySystem : MonoBehaviour
{
    [SerializeField] private float corduraMaxima = 100f;
    [SerializeField] private float velocidadDrenado = 3f;

    public float CorduraMaxima => corduraMaxima;
    public float CorduraActual=> corduraActual;

    private float corduraActual;
    private bool enZonaSegura = false;

    public event Action<float, float> OnSanityUpdated;
    public event Action OnSanityReduced;

    void Start()
    {
        corduraActual = corduraMaxima;
        OnSanityUpdated?.Invoke(corduraActual,corduraMaxima);
    }

   
    void Update()
    {
        if (enZonaSegura) return;
        if (!enZonaSegura)
        {
            ModifySanity(-velocidadDrenado * Time.deltaTime);
        }
        
    }

    public void ModifySanity (float amount)
    {
        corduraActual= Mathf.Clamp(corduraActual+amount,0,corduraMaxima);
        OnSanityUpdated?.Invoke(corduraActual, corduraMaxima);

        if (corduraActual <= 0)
        {
            OnSanityReduced?.Invoke();
        }
    }
    
    public void SetZonaSegura (bool value)
    { enZonaSegura = value; }
}
