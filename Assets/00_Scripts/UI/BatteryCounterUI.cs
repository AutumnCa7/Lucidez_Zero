using TMPro;
using UnityEngine;

public class BatteryCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;

    public void UpdateText(int amount)
    {
        amountText.text = amount.ToString();
    }
}
