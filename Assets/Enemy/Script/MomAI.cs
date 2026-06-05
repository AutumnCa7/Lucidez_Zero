using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class MomAI : MonoBehaviour
{
    public enum StateMom { Inactive, Cry, Chase, Attack }
    public StateMom stateCurrent = StateMom.Inactive;

    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator anim; 

    [Header("Setting")]
    public float velocityRun = 8f;
    public float distanceAttack = 1.5f;
    public float timePunch = 1.0f;
    bool canAttack = true;
    public float dañoCordura = 25f;

    [Header("Audio Setting")]
    public AudioSource audioSource;
    public AudioClip clipCry;
    public AudioClip clipScream;
    public AudioClip clipAttack;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>(); 
        agent.enabled = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }
    void Start()
    {

        WakeUpMom();
    }

    public void WakeUpMom()
    {
        if (stateCurrent == StateMom.Inactive)
        {
            agent.enabled = true;
            stateCurrent = StateMom.Cry;
            Debug.Log("Mom esta llorando");

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
                Debug.Log("�Te ve y empieza a correr!");

                
                if (anim != null) anim.SetTrigger("Chase");

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

        if (anim != null) anim.SetTrigger("Attack");

        if (audioSource != null && clipAttack != null)
        {
            audioSource.PlayOneShot(clipAttack);
        }

        // ---> CÓDIGO PARA SACAR CORDURA AL PLAYER <---
        // Busca el Manager de cordura en cualquier parte de la escena
        SanitySystem corduraPlayer = FindFirstObjectByType<SanitySystem>();
            
        if (corduraPlayer != null)
        {
            // Le pasamos el daño en NEGATIVO porque ModifySanity suma el valor
            corduraPlayer.ModifySanity(-dañoCordura);
            Debug.Log("¡La Mom te quitó cordura!");
        }
        else
        {
            Debug.LogWarning("¡No se encontró el SanitySystem en la escena!");
        }

        yield return new WaitForSeconds(timePunch);

        if (Vector3.Distance(transform.position, playerTransform.position) > distanceAttack)
        {
            stateCurrent = StateMom.Chase;
            
            if (anim != null) anim.SetTrigger("Chase");
        }
        canAttack = true;
    }
}