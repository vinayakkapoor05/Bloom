// using UnityEngine;
// using TMPro;

// public class GameManager : MonoBehaviour
// {
//     public static GameManager Instance { get; private set; }

//     public float gameTimer = 300f; // 5 minutes
//     public bool gameOver = false;

//     [Header("UI References")]
//     public TextMeshProUGUI timerText;
//     public TextMeshProUGUI nutrientCountText;

//     private void Awake()
//     {
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//     }

//     private void Update()
//     {
//         if (!gameOver)
//         {
//             gameTimer -= Time.deltaTime;
//             UpdateTimerUI();
//             if (gameTimer <= 0)
//             {
//                 WinGame();
//             }
//         }
//     }

//     private void UpdateTimerUI()
//     {
//         int minutes = Mathf.FloorToInt(gameTimer / 60f);
//         int seconds = Mathf.FloorToInt(gameTimer % 60f);
//         if (timerText != null)
//         {
//             timerText.text = $"Time: {minutes:00}:{seconds:00}";
//         }
//     }

//     public void UpdateNutrientUI(int count)
//     {
//         if (nutrientCountText != null)
//         {
//             nutrientCountText.text = $"Nutrients: {count}";
//         }
//     }

//     public void WinGame()
//     {
//         gameOver = true;
//         if (timerText != null)
//             timerText.text = "You Won!";
//     }

//     public void LoseGame()
//     {
//         gameOver = true;
//         if (timerText != null)
//             timerText.text = "Game Over!";
//     }
// }

using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float gameTimer = 300f;  
    public bool gameOver = false;
    public float worldRadius = 12.4f;  

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nutrientCountText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Initialize the boundary trigger
        CreateBoundaryTrigger();
    }

    private void Update()
    {
        if (!gameOver)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerUI();
            if (gameTimer <= 0)
            {
                WinGame();
            }
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60f);
        int seconds = Mathf.FloorToInt(gameTimer % 60f);
        if (timerText != null)
        {
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    public void UpdateNutrientUI(int count)
    {
        if (nutrientCountText != null)
        {
            nutrientCountText.text = $"Nutrients: {count}";
        }
    }

    public void WinGame()
    {
        gameOver = true;
        if (timerText != null)
            timerText.text = "You Won!";
    }

    public void LoseGame()
    {
        gameOver = true;
        if (timerText != null)
            timerText.text = "Game Over!";
    }

    private void CreateBoundaryTrigger()
    {
        GameObject boundaryObject = new GameObject("BoundaryTrigger");
        BoundaryTrigger boundaryTrigger = boundaryObject.AddComponent<BoundaryTrigger>();
        boundaryTrigger.SetWorldRadius(worldRadius);
    }
}
