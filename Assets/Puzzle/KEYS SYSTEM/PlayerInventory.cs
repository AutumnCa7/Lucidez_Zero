using System.Collections.Generic;
using UnityEngine;

// Esto crea una lista de opciones que podremos elegir en el Inspector
public enum KeyColor
{
    Blue,
    Green,
    Red,
    Black
}

public class PlayerInventory : MonoBehaviour
{
    [Header("Llaves del Jugador")]
    // Esta es la "mochila" donde se guardarán las llaves que recojas
    [SerializeField] private List<KeyColor> collectedKeys = new List<KeyColor>();

    // Función para agregar una llave al inventario
    public void AddKey(KeyColor newKey)
    {
        if (!collectedKeys.Contains(newKey))
        {
            collectedKeys.Add(newKey);
            Debug.Log("Has recogido la llave: " + newKey.ToString());
        }
    }

    // Función que preguntará la puerta para saber si tienes la llave
    public bool HasKey(KeyColor requiredKey)
    {
        return collectedKeys.Contains(requiredKey);
    }
}