// CameraController.cs
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public float zoomLevel = 5f;
    
    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI nutrientCountText;
    
    private Camera cam;
    
    private void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = zoomLevel;
        
        if (timerText != null) timerText.text = "Time: 5:00";
        if (nutrientCountText != null) nutrientCountText.text = "Nutrients: 0";
    }
    
    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, -10f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}