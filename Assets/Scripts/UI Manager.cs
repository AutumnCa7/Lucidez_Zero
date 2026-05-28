using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Slider barraCordura;
    [SerializeField] private Image uiImagenRelleno;
    [SerializeField] private SanitySystem sanitySystem; 

    [SerializeField] private Color colorSaludable = Color.green;
    [SerializeField] private Color colorCritico = Color.red;
    [Range(0f, 1f)][SerializeField] private float umbralPanico = 0.2f;

    private void Start()
    {
        barraCordura.maxValue = sanitySystem.CorduraMaxima;
        sanitySystem.OnSanityUpdated += UpdateUI;
    }

    void UpdateUI(float actual, float max)
    {
        barraCordura.value= actual;

        float porcentaje = actual/max;
        Color colorActual = Color.Lerp(colorCritico, colorSaludable, porcentaje);
        uiImagenRelleno.color = colorActual;

        if (porcentaje <= umbralPanico)
        {
            float parpadeo = Mathf.Sin(Time.time * 20f);
            if (parpadeo > 0) uiImagenRelleno.color = Color.white;
        }
    }
}
