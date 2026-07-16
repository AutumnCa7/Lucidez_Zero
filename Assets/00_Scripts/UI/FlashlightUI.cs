using UnityEngine;

public class FlashlightUI : MonoBehaviour
{
    [SerializeField] private FlashlightSystem flashlightSystem;
    [SerializeField] private StatusImageUI statusImageUI;

    private void OnEnable()
    {
        flashlightSystem.OnFlashlightBatteryUpdated += statusImageUI.UpdateImageUI;
        flashlightSystem.OnFlashlightObtained += statusImageUI.Show;
        ;
    }

    private void OnDisable()
    {
        flashlightSystem.OnFlashlightBatteryUpdated -= statusImageUI.UpdateImageUI;
        flashlightSystem.OnFlashlightObtained -= statusImageUI.Show;
    }


}
