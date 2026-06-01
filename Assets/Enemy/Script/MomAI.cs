using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MomAI : MonoBehaviour
{
    public enum StateMom { Inactive, Cry, Chase, Attack }
    public StateMom stateCurrent = StateMom.Inactive;

    private Transform playerTransform;
    private NavMeshAgent agent;

    [Header("Setting")]
    public float velocityRun = 8f;
    public float distanceAttack = 1.5f;
    public float timePunch = 1.0f;
    bool canAttack = true;

    [Header("Audio Setting")]
    public AudioSource audioSource;
    public AudioClip clipCry;
    public AudioClip clipScream;
    public AudioClip clipAttack;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    public void WakeUpMom()
    {
        if (stateCurrent == StateMom.Inactive)
        {
            agent.enabled = true;
            stateCurrent = StateMom.Cry;
            Debug.Log("Mom esta llorando");

            // el llanto en loop
            if (audioSource != null && clipCry != null)
            {
                audioSource.clip = clipCry;
                audioSource.loop = true;
                audioSource.Play();
            }

            StartCoroutine(CheckDistance());
        }
    }

    void Update()
    {
        if (playerTransform == null || !agent.enabled) return;

        switch (stateCurrent)
        {
            case StateMom.Cry:
                agent.isStopped = true;
                break;

            case StateMom.Chase:
                agent.isStopped = false;
                agent.speed = velocityRun;
                agent.SetDestination(playerTransform.position);

                if (Vector3.Distance(transform.position, playerTransform.position) <= distanceAttack)
                {
                    stateCurrent = StateMom.Attack;
                }
                break;

            case StateMom.Attack:
                agent.isStopped = true;
                if (canAttack) StartCoroutine(Hit());
                break;
        }
    }

    IEnumerator CheckDistance()
    {
        while (stateCurrent == StateMom.Cry)
        {
            
            if (Vector3.Distance(transform.position, playerTransform.position) < 10f)
            {
                FindFirstObjectByType<DynamicAudioController>().TriggerFinalBattleAudio();
                stateCurrent = StateMom.Chase;
                Debug.Log("¡Te ve y empieza a correr!");

                // Cortamos el llanto y dispara el grito de susto
                if (audioSource != null && clipScream != null)
                {
                    audioSource.Stop();
                    audioSource.loop = false;
                    audioSource.PlayOneShot(clipScream);
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Hit()
    {
        canAttack = false;
        Debug.Log("¡Te golpeo!");

        // Reproduce sonido de ataque
        if (audioSource != null && clipAttack != null)
        {
            audioSource.PlayOneShot(clipAttack);
        }

        // ACA SE PONE EL CODIGO PARA SACAR VIDA AL PLAYER STEF

        yield return new WaitForSeconds(timePunch);

        if (Vector3.Distance(transform.position, playerTransform.position) > distanceAttack)
        {
            stateCurrent = StateMom.Chase;
        }
        canAttack = true;
    }
}