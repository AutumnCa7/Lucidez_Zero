using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips; // Arreglo para poner varios sonidos de pasos

    [Header("Ritmo de Pasos")]
    [SerializeField] private float stepInterval = 0.5f; // Tiempo en segundos entre cada paso

    private float stepTimer;

    private void Update()
    {
        // Detectar si el jugador está presionando teclas de movimiento
        bool isMoving = Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        if (isMoving)
        {
            // El temporizador va restando el tiempo
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval; // Reinicia el tiempo para el proximo paso
            }
        }
        else
        {
            
            stepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        // Verificamos que haya sonidos cargados
        if (footstepClips.Length > 0)
        {
            
            int randomIndex = Random.Range(0, footstepClips.Length);

            // Modifico ligeramente el Pitch para que no suene robotico
            audioSource.pitch = Random.Range(0.4f, 0.6f);

            //Modificar  el Volumen para realismo
            audioSource.volume = Random.Range(0.3f, 0.4f);

            
            audioSource.PlayOneShot(footstepClips[randomIndex]);
        }
    }
}