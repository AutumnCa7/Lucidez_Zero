using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SanitySystem sanitySystem;
    [SerializeField] private FlashlightSystem flashlightSystem;
    [SerializeField] private WalkmanSystem walkmanSystem;
    [SerializeField] private BatteryManager batteryManager;
    
    [Header("Images")]
    [SerializeField] private Image imageFlashlightUI;
    [SerializeField] private Image imageWalkmanUI;
    [SerializeField] private Image imageSanityUI;
    [SerializeField] private Image imageBatteryUI;

    [SerializeField] private TextMeshProUGUI amountBateries;
    
    [Header("Sprites")]
    [SerializeField] private Sprite imageNormalFlashlightUI;
    [SerializeField] private Sprite imageBrokenFlashlightUI;
    [SerializeField] private Sprite imageNormalWalkmanUI;
    [SerializeField] private Sprite imageBrokenWalkmanUI;
    [SerializeField] private Sprite imageNormalSanityUI;
    [SerializeField] private Sprite imageBrokenSanityUI;


    [Range(0f, 1f)][SerializeField] private float umbralPanico = 0.2f;
    [SerializeField] private float minColorAlpha=0.2f;
    [SerializeField] private float maxColorAlpha = 1f;


    private void Start()
    {
        Debug.Log("UIManager iniciado");

        batteryManager.OnUpdateBattery += UpdateBatteryCountUI;
        UpdateBatteryCountUI(batteryManager.NumberOfBatteries);

        flashlightSystem.OnFlashlightBatteryUpdated += UpdateBatteryFlashlightUI;
        walkmanSystem.OnWalkmanBatteryUpdated += UpdateBatteryWalkmanUI;
        sanitySystem.OnSanityUpdated += UpdateSanityUI;
        walkmanSystem.OnWalkmanObtained += ActivateWalkmanUI;
        flashlightSystem.OnFlashlightObtained += ActivateFlashlightUI; ;
    }
    private void OnDestroy()
    {
        flashlightSystem.OnFlashlightBatteryUpdated -= UpdateBatteryFlashlightUI;
        walkmanSystem.OnWalkmanBatteryUpdated -= UpdateBatteryWalkmanUI;
        sanitySystem.OnSanityUpdated -= UpdateSanityUI;

        walkmanSystem.OnWalkmanObtained -= ActivateWalkmanUI;
        flashlightSystem.OnFlashlightObtained -= ActivateFlashlightUI;
    }

    
    private void ActivateFlashlightUI( bool obtained)
    {
        imageFlashlightUI.gameObject.SetActive(obtained);
    }
    private void ActivateWalkmanUI(bool obtained)
    {
        imageWalkmanUI.gameObject.SetActive(obtained);
    }

    private void UpdateBatteryCountUI(int amount)
    {
        amountBateries.text = amount.ToString();
    }

    private void UpdateImageUI(Image image,Sprite brokenImage, Sprite normalImage,float actual, float max)
    {
        float porcentaje = actual / max;

        if (porcentaje <= 0)
        {
            image.sprite = brokenImage;
            image.color = Color.red;
            return;
        }
        else { image.sprite = normalImage; }

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
    private void UpdateSanityUI(float actual, float max)
    {
        UpdateImageUI(imageSanityUI, imageBrokenSanityUI, imageNormalSanityUI, actual, max);
    }
    private void UpdateBatteryFlashlightUI (float actual, float max)
    {
        UpdateImageUI (imageFlashlightUI,imageBrokenFlashlightUI, imageNormalFlashlightUI, actual, max);
    }
    private void UpdateBatteryWalkmanUI(float actual, float max)
    {
        UpdateImageUI(imageWalkmanUI, imageBrokenWalkmanUI, imageNormalWalkmanUI, actual, max);
    }
}