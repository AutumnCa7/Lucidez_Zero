using UnityEngine;
using UnityEngine.Audio;

public class DynamicAudioController : MonoBehaviour
{
    [Header("Audio Mixer Setup")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerSnapshot explorationSnapshot;
    [SerializeField] private AudioMixerSnapshot panicSnapshot;
    [SerializeField] private AudioMixerSnapshot finalBattleSnapshot; 

    [Header("Heartbeat (Para la Nena)")]
    [SerializeField] private AudioSource heartbeatSource;

    [Header("Parameter Settings")]
    [SerializeField] private string lowpassParameter = "MasterLowpass";
    [SerializeField] private float normalCutoff = 22000f;
    [SerializeField] private float panicCutoff = 1500f;

    private string currentState = "Exploration";

    // Ataque de la nena fantasma (Latidos y sordera)
    public void TriggerPanicAudio()
    {
        if (currentState == "Panic" || currentState == "FinalBattle") return;
        currentState = "Panic";

        panicSnapshot.TransitionTo(0.5f);
        mainMixer.SetFloat(lowpassParameter, panicCutoff);
        if (heartbeatSource != null) heartbeatSource.Play();
    }

    // batalla final con la Mama (Violencia pura,solo SFX
    public void TriggerFinalBattleAudio()
    {
        if (currentState == "FinalBattle") return;
        currentState = "FinalBattle";

        // Transicion rpida (0.1s)"Jump Scare"
        finalBattleSnapshot.TransitionTo(0.1f);

        
        mainMixer.SetFloat(lowpassParameter, normalCutoff);

        // Apago latido porque importa el grito y la persecucion real
        if (heartbeatSource != null) heartbeatSource.Stop();
    }

    // Vuelta a la normalidad
    public void ReturnToNormalAudio()
    {
        if (currentState == "Exploration") return;
        currentState = "Exploration";

        explorationSnapshot.TransitionTo(3f);
        mainMixer.SetFloat(lowpassParameter, normalCutoff);
        if (heartbeatSource != null) heartbeatSource.Stop();
    }
}