// EnzymeBomb.cs
using UnityEngine;
using System.Collections;

public class EnzymeBomb : MonoBehaviour 
{
    public float explosionRadius = 10f;
    public float damagePerSecond = 20f; 
    public float explosionDuration = 30f; 
    public ParticleSystem explosionEffect;

    private bool isExploding = false;
    private CircleCollider2D damageArea;
    private ParticleSystem.ShapeModule particleShape;

    private void Start() 
    {
        damageArea = gameObject.AddComponent<CircleCollider2D>(); 
        damageArea.isTrigger = true;
        damageArea.radius = explosionRadius;

        if (explosionEffect != null) 
        {
            particleShape = explosionEffect.shape;
            particleShape.radius = explosionRadius;

            var main = explosionEffect.main;
            main.duration = explosionDuration; 
            main.startLifetime = 2.0f;  
        }

        Explode();
    }

    private void Explode() 
    {
        if (!isExploding) 
        {
            isExploding = true;
            StartCoroutine(ExplodeCoroutine());
        }
    }

    private IEnumerator ExplodeCoroutine() 
    {
        if (explosionEffect != null) 
        {
            explosionEffect.Play();  
        }

        float elapsedTime = 0f;

        while (elapsedTime < explosionDuration && isExploding) 
        {
            Collider2D[] currentNearbyObjects = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            foreach (Collider2D obj in currentNearbyObjects) 
            {
                Predator predator = obj.GetComponent<Predator>(); 
                if (predator != null) 
                {
                    predator.TakeDamage(damagePerSecond * Time.deltaTime);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (explosionEffect != null) 
        {
            explosionEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            yield return new WaitForSeconds(2.0f);  
        }

        Destroy(gameObject);
    }

    private void OnDestroy() 
    {
        isExploding = false;
        StopAllCoroutines();
    }
}
