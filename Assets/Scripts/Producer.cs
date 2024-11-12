using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Producer : MonoBehaviour
{
    public float nutrientSpawnInterval = 3f;
    public GameObject nutrientPrefab;
    public float nutrientSpawnRadius = 2f;

    private float nutrientTimer;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (gameManager.gameOver) return;

        nutrientTimer += Time.deltaTime;
        if (nutrientTimer >= nutrientSpawnInterval)
        {
            for (int i = 0; i < 3; i++)
            {
                SpawnNutrient();
            }

            nutrientTimer = 0f;
        }
    }

    private void SpawnNutrient()
    {
        Vector2 randomOffset = Random.insideUnitCircle * nutrientSpawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        GameObject nutrient = Instantiate(nutrientPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D rb = nutrient.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(randomOffset.normalized * Random.Range(0.5f, 2f), ForceMode2D.Impulse);
        }
    }
}
