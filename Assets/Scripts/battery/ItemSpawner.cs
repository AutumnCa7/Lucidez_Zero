using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Setting Battery")]
    [Tooltip("prefabbattery")]
    public GameObject batteryPrefab;

    [Header("Points Aparitions")]
    [Tooltip("PointsSpawn")]
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnBatteryPointsAleatory();

        
    }

   

    
    public void SpawnBatteryPointsAleatory()
    {
        if (batteryPrefab == null || spawnPoints.Length == 0) return;

        int indexAleatory = Random.Range(0, spawnPoints.Length);
        Transform pointElected = spawnPoints[indexAleatory];

        Instantiate(batteryPrefab, pointElected.position, pointElected.rotation);
    }
}