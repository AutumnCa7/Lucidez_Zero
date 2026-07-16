using System.Collections;
using UnityEngine;

public class LightEventManager : MonoBehaviour
{
    [SerializeField] private LampController[] lamparas;

    private bool eventoEjecutado = false;

    public void ActivarApagonConParpadeo()
    {
        if (eventoEjecutado)
            return;

        eventoEjecutado = true;
        StartCoroutine(CorrutinaApagon());
    }

    private IEnumerator CorrutinaApagon()
    {
        int parpadeos = 2;
        float delay = 0.15f;

        for (int i = 0; i < parpadeos; i++)
        {
            SetVisual(true);
            yield return new WaitForSeconds(delay);

            SetVisual(false);

            PlaySparks();

            yield return new WaitForSeconds(delay);
        }

        foreach (LampController l in lamparas)
        {
            l.ApagarCompleto();
        }
    }
    private void PlaySparks()
    {
        foreach (LampController l in lamparas)
        {
            l.PlaySparks();
        }
    }

    public void EncenderLuces()
    {
        eventoEjecutado = false;

        foreach (LampController l in lamparas)
        {
            l.EncenderCompleto();
        }

        Debug.Log("Luces restauradas");
    }

    private void SetVisual(bool state)
    {
        foreach (LampController l in lamparas)
        {
            l.SetVisual(state);
        }
    }
}