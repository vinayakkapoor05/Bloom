// GameSetup.cs
using UnityEngine;
using TMPro; // Add this for TextMeshPro

public class GameSetup : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject predatorPrefab;
    public GameObject consumerPrefab;
    public GameObject producerPrefab;

    [Header("Spawn Settings")]
    public int initialConsumers = 4;
    public int initialProducers = 4;
    public float spawnEdgeOffset = 1f;
    
    [Header("Wave Settings")]
    public float timeBetweenWaves = 30f;
    public int predatorsPerWave = 2;
    public float initialDelay = 10f;
    public float waveTextDisplayTime = 3f; // How long to show the wave text

    [Header("UI")]
    public TextMeshProUGUI waveText; // Reference to UI text component
    public TextMeshProUGUI nextWaveText; // Optional countdown text
    public TextMeshProUGUI predatorKillsText;
    private int predatorsKilled = 0; 
    private float timeUntilNextWave;
    private int currentWave = 0;
    private float worldRadius;
    private int predatorCount = 0;
    private int consumerCount = 0;
    private float waveTextTimer = 0f;

    private void Start() {
        worldRadius = GameManager.Instance.worldRadius;
        timeUntilNextWave = initialDelay;
        SpawnInitialEntities();
        
        // Initialize wave text
        if (waveText != null) {
            waveText.gameObject.SetActive(false);
        }
        UpdateNextWaveCountdown();
        UpdateKillCounter(); 
    }

    private void Update() {
        if (!GameManager.Instance.gameOver) {
            timeUntilNextWave -= Time.deltaTime;
            
            // Update countdown text
            UpdateNextWaveCountdown();

            if (timeUntilNextWave <= 0) {
                SpawnWave();
                timeUntilNextWave = timeBetweenWaves;
            }

            // Handle wave announcement text fade
            if (waveTextTimer > 0) {
                waveTextTimer -= Time.deltaTime;
                if (waveTextTimer <= 0 && waveText != null) {
                    waveText.gameObject.SetActive(false);
                }
            }

            CheckGameState();
        }
    }

    private void UpdateNextWaveCountdown() {
        if (nextWaveText != null) {
            if (currentWave == 0) {
                nextWaveText.text = $"First Wave in: {Mathf.Ceil(timeUntilNextWave)}s";
            } else {
                nextWaveText.text = $"Next Wave in: {Mathf.Ceil(timeUntilNextWave)}s";
            }
        }
    }

    private void SpawnWave() {
        currentWave++;
        int predatorsToSpawn = predatorsPerWave * currentWave;
        
        // Show wave announcement
        if (waveText != null) {
            waveText.gameObject.SetActive(true);
            waveText.text = $"Wave {currentWave}\n{predatorsToSpawn} Predators Incoming!";
            waveTextTimer = waveTextDisplayTime;
        }

        for (int i = 0; i < predatorsToSpawn; i++) {
            SpawnPredator();
        }
    }

    // Rest of the code remains the same...
    private void SpawnInitialEntities() {
        for (int i = 0; i < initialConsumers; i++) {
            Vector2 randomPos = Random.insideUnitCircle * (worldRadius * 0.5f);
            GameObject consumer = Instantiate(consumerPrefab, randomPos, Quaternion.identity);
            consumerCount++;
        }

        for (int i = 0; i < initialProducers; i++) {
            Vector2 randomPos = Random.insideUnitCircle * (worldRadius * 0.5f);
            Instantiate(producerPrefab, randomPos, Quaternion.identity);
        }
    }

    private void SpawnPredator() {
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        
        float x = Mathf.Cos(randomAngle) * (worldRadius - spawnEdgeOffset);
        float y = Mathf.Sin(randomAngle) * (worldRadius - spawnEdgeOffset);
        Vector2 spawnPos = new Vector2(x, y);
        
        float rotationAngle = Mathf.Atan2(-y, -x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);
        
        GameObject predator = Instantiate(predatorPrefab, spawnPos, rotation);
        predatorCount++;
    }

    public void HandleConsumerDestroyed() {
        consumerCount--;
        CheckGameState();
    }

    public void OnPredatorDestroyed() {
        predatorCount--;
        predatorsKilled++;
        UpdateKillCounter();
    }
     private void UpdateKillCounter() {
        if (predatorKillsText != null) {
            predatorKillsText.text = $"Predators Killed: {predatorsKilled}";
        }
    }

    private void CheckGameState() {
        Consumer[] consumers = FindObjectsOfType<Consumer>();
        consumerCount = consumers.Length;

        if (consumerCount == 0) {
            GameManager.Instance.LoseGame();
        }
    }
}