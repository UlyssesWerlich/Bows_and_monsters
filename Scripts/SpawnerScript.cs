using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{

    [System.Serializable]
    public class Wave {
        public GameObject[] enemies;
        public int enemyCount;
        public float spawnTime;
    }

    public GameScript gameScript;
    public Wave[] wavesEasy;
    public Wave[] wavesNormal;
    public Wave[] wavesHard;
    public Transform[] spawnPoints;
    public float waveTime;

    private Wave currentWave;
    private int currentWaveIndex;
    private Transform player;
    private bool waveEnded;
    private Wave[] waves;

    // Start is called before the first frame update
    void Start()
    {
        SelectDifficulty();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(StartNextWave(currentWaveIndex));
    }

    private void SelectDifficulty()
    {
        switch (PlayerPrefs.GetInt("Difficulty"))
        {
            case 1: waves = wavesEasy; break;
            case 2: waves = wavesNormal; break;
            case 3: waves = wavesHard; break;
        }
    }

    IEnumerator StartNextWave(int index){
        gameScript.UpdateWaveUI(index);
        yield return new WaitForSeconds(waveTime);
        StartCoroutine(SpawnWave(index));
    }

    IEnumerator SpawnWave(int index){
        currentWave = waves[index];

        for (int i = 0; i < currentWave.enemyCount; i++){
            if (player == null ) yield break;
            GameObject randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpot = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(randomEnemy, randomSpot.position, randomSpot.rotation);

            if (i == currentWave.enemyCount -1){
                waveEnded = true;
            } else {
                waveEnded = false;
            }

            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waveEnded == true 
        && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 
        && GameObject.FindGameObjectsWithTag("EnemyRanged").Length == 0 
        && GameObject.FindGameObjectsWithTag("Boss").Length == 0
        && GameObject.FindGameObjectsWithTag("Spider").Length == 0
        && GameObject.FindGameObjectsWithTag("Thing").Length == 0)
        {
            
            waveEnded = false;

            if (currentWaveIndex + 1 < waves.Length){
                currentWaveIndex++;
                StartCoroutine(StartNextWave(currentWaveIndex));

            } else {
                gameScript.Victory();
            }
        }
    }
}
