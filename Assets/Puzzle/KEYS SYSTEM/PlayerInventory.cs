using System.Collections.Generic;
using UnityEngine;


public enum KeyColor
{
    Blue,
    Green,
    Red,
    Black
}

public class PlayerInventory : MonoBehaviour
{
    [Header("Player Key s")]
    
    [SerializeField] private List<KeyColor> collectedKeys = new List<KeyColor>();

    
    public void AddKey(KeyColor newKey)
    {
        if (!collectedKeys.Contains(newKey))
        {
            collectedKeys.Add(newKey);
            Debug.Log("Has recogido la llave: " + newKey.ToString());
        }
    }

    
    public bool HasKey(KeyColor requiredKey)
    {
        return collectedKeys.Contains(requiredKey);
    }
}