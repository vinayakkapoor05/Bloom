using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int nutrientCount = 0;
    public GameObject enzymeBombPrefab;
    public GameObject sporeProjectilePrefab;

    public const int ENZYME_BOMB_COST = 10;
    public float projectileSpeed = 10f;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private GameManager gameManager;

    public AudioClip shootSporeClip;
    public AudioClip collectNutrientClip;
    public AudioClip dropEnzymeBombClip;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        rb.velocity = movement * moveSpeed;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetKeyDown(KeyCode.Space) && nutrientCount >= ENZYME_BOMB_COST)
        {
            LaunchEnzymeBomb();
        }

        if (Input.GetMouseButtonDown(0) && nutrientCount > 0)
        {
            ShootSpore();
        }
    }

    private void LaunchEnzymeBomb()
    {
        nutrientCount -= ENZYME_BOMB_COST;
        Instantiate(enzymeBombPrefab, transform.position, Quaternion.identity);
        gameManager.UpdateNutrientUI(nutrientCount);
        
        PlaySound(dropEnzymeBombClip);
    }

    private void ShootSpore()
    {
        if (nutrientCount <= 0) return;

        nutrientCount--;
        GameObject spore = Instantiate(sporeProjectilePrefab, transform.position, transform.rotation);
        Rigidbody2D sporeRb = spore.GetComponent<Rigidbody2D>();
        sporeRb.velocity = transform.right * projectileSpeed;
        gameManager.UpdateNutrientUI(nutrientCount);

        PlaySound(shootSporeClip);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nutrient"))
        {
            nutrientCount++;
            Destroy(other.gameObject);
            gameManager.UpdateNutrientUI(nutrientCount);

            PlaySound(collectNutrientClip);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
