using UnityEngine;

public class LightEventManager : MonoBehaviour
{
    [SerializeField] private LampController[] lamparas;

    private bool apagadas = false;

    public void ApagarLuces()
    {
        if (apagadas)
            return;

        apagadas = true;

        foreach (LampController lampara in lamparas)
        {
            lampara.Apagar();
        }
    }

    public void EncenderLuces()
    {
        apagadas = false;

        foreach (LampController lampara in lamparas)
        {
            lampara.Encender();
        }
    }
}