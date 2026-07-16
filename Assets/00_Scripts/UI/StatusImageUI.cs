using UnityEngine;
using UnityEngine.UI;


public class StatusImageUI : MonoBehaviour
{
  
    [SerializeField] private Image image;

    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite brokenSprite;

    [Range(0f, 1f)][SerializeField] private float umbralPanico = 0.2f;
    [SerializeField] private float minColorAlpha = 0.2f;
    [SerializeField] private float maxColorAlpha = 1f;


    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void UpdateImageUI(float actual, float max)
    {
        float porcentaje = actual / max;

        if (porcentaje <= 0)
        {
            image.sprite = brokenSprite;
            image.color = Color.red;
            return;
        }
        else { image.sprite = normalSprite; }

        Color color = image.color;
        float alphaActual = Mathf.Lerp(minColorAlpha, maxColorAlpha, porcentaje);
        color.a = alphaActual;

        // TINTE ROJO
        color.r = 1f;
        color.g = porcentaje;
        color.b = porcentaje;

        // PARPADEO
        if (porcentaje <= umbralPanico)
        {
            float parpadeo = Mathf.Sin(Time.time * 20f);

            if (parpadeo > 0)
            {
                color = Color.white;

                color.a = alphaActual;
            }

        }

        image.color = color;
    }
    public void Show(bool value)
    {
        Debug.Log("SetVisible " + value);
        image.enabled = value;
    }

  

    
}
