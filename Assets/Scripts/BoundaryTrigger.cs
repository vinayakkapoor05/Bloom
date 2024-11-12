// BoundaryTrigger.cs
using UnityEngine;

public class BoundaryTrigger : MonoBehaviour {
    private float worldRadius;

    public void SetWorldRadius(float radius) {
        worldRadius = radius;
        CreateBoundaryCollider();
    }

    private void CreateBoundaryCollider() {
        CircleCollider2D boundaryCollider = gameObject.AddComponent<CircleCollider2D>();
        boundaryCollider.radius = worldRadius;
        boundaryCollider.isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Nutrient")) {
            Destroy(other.gameObject);
            return;
        }
         if (other.CompareTag("Projectile")) {
            Destroy(other.gameObject);
            return;
        }

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Vector2 position = other.transform.position;
        
        position = position.normalized * worldRadius;
        other.transform.position = position;

        if (rb != null) {
            rb.velocity = -rb.velocity;
        }
    }
}