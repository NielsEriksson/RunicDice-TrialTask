using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int spawnDelay;
    public void Start()
    {
        StartCoroutine(EnemySpawnCO());
    }
    public IEnumerator EnemySpawnCO()
    {
        while(true)
        {
            Instantiate(enemyPrefab, transform.position,Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
