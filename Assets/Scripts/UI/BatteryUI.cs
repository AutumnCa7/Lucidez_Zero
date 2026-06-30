using TMPro;
using UnityEngine;

public class BatteryUI : MonoBehaviour
{
    [SerializeField] private BatteryManager batteryManager;
    [SerializeField] private BatteryCounterUI batteryCounterUI;

    private void Start()
    {
        batteryCounterUI.UpdateText(batteryManager.NumberOfBatteries);
    }
    private void OnEnable()
    {
        batteryManager.OnUpdateBattery += batteryCounterUI.UpdateText;
    }

    private void OnDisable()
    {
        batteryManager.OnUpdateBattery -= batteryCounterUI.UpdateText;
    }
}
