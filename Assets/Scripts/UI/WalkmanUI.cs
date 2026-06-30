using UnityEngine;

public class WalkmanUI : MonoBehaviour
{
    [SerializeField] private WalkmanSystem walkmanSystem;
    [SerializeField] private StatusImageUI statusImageUI;

    private void OnEnable()
    {
        walkmanSystem.OnWalkmanBatteryUpdated += statusImageUI.UpdateImageUI;
        walkmanSystem.OnWalkmanObtained += statusImageUI.Show;
        ;
    }

    private void OnDisable()
    {
        walkmanSystem.OnWalkmanBatteryUpdated -= statusImageUI.UpdateImageUI;
        walkmanSystem.OnWalkmanObtained -= statusImageUI.Show;
    }

}
