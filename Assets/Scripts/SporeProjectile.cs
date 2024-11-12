// SporeProjectile.cs
using UnityEngine;

public class SporeProjectile : MonoBehaviour 
{
    public float damage = 5f;
    public float speed = 10f; // Add speed for movement
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Set initial velocity in the direction the spore is facing
            rb.velocity = transform.right * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Predator"))
        {
            Predator predator = collision.gameObject.GetComponent<Predator>();
            if (predator != null)
            {
                predator.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}