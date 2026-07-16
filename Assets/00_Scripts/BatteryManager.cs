using System;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{

    [SerializeField] private int numberOfBatteries= 0;
   
    public int NumberOfBatteries=> numberOfBatteries;

    public event Action<int> OnUpdateBattery; 
    void Start()
    {
        OnUpdateBattery?.Invoke(numberOfBatteries);
    }

    public void AddBattery(int amount = 1)
    {
        
        numberOfBatteries += amount;
        OnUpdateBattery?.Invoke(numberOfBatteries);

    }
    public bool ConsumeBattery()
    {
        if (numberOfBatteries <= 0)
            return false;

        numberOfBatteries--;

        OnUpdateBattery?.Invoke(numberOfBatteries);
        return true;
    }

  
}
