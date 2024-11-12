using UnityEngine;
using UnityEngine.UI;

public class Predator : MonoBehaviour {
    public float moveSpeed = 2.25f; 
    public float detectionRadius = 25f;
    public float attackDamage = 10f;
    public float attackInterval = 1f;
    public float health = 100f;
    public Slider healthBar;

    private Transform currentTarget;
    private float attackTimer;
    private float searchTimer = 0f;
    private const float SEARCH_INTERVAL = 2f; 

    private void Start() {
        FindNewTarget();
    }

    private void Update() {
        searchTimer += Time.deltaTime;
        if (currentTarget == null && searchTimer >= SEARCH_INTERVAL) {
            FindNewTarget();
            searchTimer = 0f;
        }
        
        if (currentTarget != null) {
            MoveTowardsTarget();
            TryAttack();
        } else {
            Collider2D[] farObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius * 1.2f);
            foreach (Collider2D obj in farObjects) {
                Consumer consumer = obj.GetComponent<Consumer>();
                if (consumer != null) {
                    currentTarget = consumer.transform;
                    break;
                }
            }
        }

        healthBar.value = health / 100f;

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void FindNewTarget() {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (Collider2D obj in nearbyObjects) {
            Consumer consumer = obj.GetComponent<Consumer>();
            if (consumer != null) {
                currentTarget = consumer.transform;
                return;
            }
        }

        Consumer[] allConsumers = FindObjectsOfType<Consumer>();
        if (allConsumers.Length > 0) {
            float nearestDistance = float.MaxValue;
            Consumer nearestConsumer = null;
            
            foreach (Consumer consumer in allConsumers) {
                float distance = Vector2.Distance(transform.position, consumer.transform.position);
                if (distance < nearestDistance) {
                    nearestDistance = distance;
                    nearestConsumer = consumer;
                }
            }

            if (nearestConsumer != null) {
                currentTarget = nearestConsumer.transform;
            }
        }
    }

    private void MoveTowardsTarget() {
        if (currentTarget != null) {
            Vector2 direction = (currentTarget.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void TryAttack() {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval && currentTarget != null && 
            Vector2.Distance(transform.position, currentTarget.position) < 1f) {
            Consumer consumer = currentTarget.GetComponent<Consumer>();
            if (consumer != null) {
                consumer.TakeDamage(attackDamage);
            }
            attackTimer = 0f;
        }
    }

    public void TakeDamage(float damage) {
        health -= damage;
         if (health <= 0) {
            GameSetup gameSetup = FindObjectOfType<GameSetup>();
            if (gameSetup != null) {
                gameSetup.OnPredatorDestroyed();
            }
            Destroy(gameObject);
        }
    }
}