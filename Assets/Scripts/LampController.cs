using UnityEngine;

public class LampController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private BoxCollider zonaSegura;
    [SerializeField] private Light pointLight;

    public void Apagar()
    {
        zonaSegura.enabled = false;
        pointLight.enabled = false;
    }

    public void Encender()
    {
        zonaSegura.enabled = true;
        pointLight.enabled = true;
    }
}