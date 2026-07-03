using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AudioSource))]
public class GhostController : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject playerFlashlight;

    [Header("Flashlight Steal Event")]
    [SerializeField] private GameObject droppedFlashlightPrefab;
    [SerializeField] private Transform[] flashlightDropPoints;
    [SerializeField] private float stealDistance = 2f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip fastFootstepsClip;
    [SerializeField] private AudioClip stealSoundClip;
    [SerializeField] private AudioClip laughSoundClip; 

    private NavMeshAgent agent;
    private AudioSource audioSource;
    private bool isCrossingHallway = false;
    private bool isChasingPlayer = false;

    public bool hasStolenFlashlight = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
            // Este código lee la velocidad del cubo. Si se mueve rápido, corre. Si se frena, se queda quieta.
            Animator anim = GetComponent<Animator>();
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (anim != null && agent != null)
            {
                // Si la velocidad es mayor a 0.1, el bool se vuelve true y arranca a correr
                anim.SetBool("Corriendo", agent.velocity.magnitude > 0.1f);
            }
        
        if (isChasingPlayer && playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }

        if (!hasStolenFlashlight && Vector3.Distance(transform.position, playerTransform.position) <= stealDistance)
        {
            StealFlashlight();
        }

        if (isCrossingHallway && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vanish();
        }
    }

    private void OnBecameInvisible()
    {
        if (!isCrossingHallway && !isChasingPlayer)
        {
            Vanish();
        }
    }

    public void Vanish()
    {
        FindFirstObjectByType<DynamicAudioController>().ReturnToNormalAudio();
        isCrossingHallway = false;
        isChasingPlayer = false;
        gameObject.SetActive(false);
    }

    private void StealFlashlight()
    {
        hasStolenFlashlight = true;

        if (playerFlashlight != null) playerFlashlight.SetActive(false);
        if (stealSoundClip != null) audioSource.PlayOneShot(stealSoundClip);

        if (flashlightDropPoints.Length > 0 && droppedFlashlightPrefab != null)
        {
            int randomIndex = Random.Range(0, flashlightDropPoints.Length);
            Instantiate(droppedFlashlightPrefab, flashlightDropPoints[randomIndex].position, Quaternion.identity);
        }

        Vanish();
    }

    public void TriggerHallwayCross(Transform destinationPoint)
    {
        gameObject.SetActive(true);
        isCrossingHallway = true;
        isChasingPlayer = false;

        agent.SetDestination(destinationPoint.position);

        PlayFootsteps();
    }

    public void TriggerStealChase()
    {
        if (hasStolenFlashlight) return;

        FindFirstObjectByType<DynamicAudioController>().TriggerPanicAudio();
        gameObject.SetActive(true);
        isChasingPlayer = true;
        isCrossingHallway = false;

        agent.speed = 7f;

        // Reproducir la risita justo en el frame que aparece
        if (laughSoundClip != null)
        {
            // PlayOneShot para que no interrumpa los pasos 
            audioSource.PlayOneShot(laughSoundClip);
        }

        PlayFootsteps();
    }

    private void PlayFootsteps()
    {
        if (fastFootstepsClip != null)
        {
            audioSource.clip = fastFootstepsClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}