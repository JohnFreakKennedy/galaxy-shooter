using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabEnemy;
    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private float _enemySpawnTime = 5f;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (!_stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_prefabEnemy, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnTime);
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (!_stopSpawning)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-8f, 8f), 7f, 0);
            int randomPowerUp = Random.Range(0, 3);
            Instantiate(_powerUps[randomPowerUp], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(5, 8));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
