using UnityEngine;

public class MomSpawnManager : MonoBehaviour
{
    public GameObject MomPrefab;
    public Transform pointMom;

    // Simula si agarre la llave o no
    public bool getKey = false;
    private bool isActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            if (getKey)
            {
                isActive = true;

                // Crea mama en el punto indicado
                GameObject nuevaWitch = Instantiate(MomPrefab, pointMom.position, pointMom.rotation);

                // Activa su estado de llanto
                nuevaWitch.GetComponent<MomAI>().WakeUpMom();
            }
        }
    }
}
