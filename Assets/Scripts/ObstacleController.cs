using UnityEngine;
using StarterAssets;

public class ObstacleController : MonoBehaviour
{
    public StarterAssetsInputs playerInput;
    public float moveSpeed = 5f;
    public float minX = -5f;
    public float maxX = 5f;

    [Tooltip("Bu engel hangi Level'a ait?")]
    public int myLevel = 1;

    void Update()
    {
        if (playerInput == null) return;
        if (playerInput.isMarioMode) return;

        // --- YENÝ KURAL BURADA ---
        // Eðer engel kontrol þalteri kapalýysa, engeller hiçbir þekilde hareket etmez!
        if (!playerInput.canControlObstacles) return;

        if (myLevel != playerInput.currentActiveLevel) return;

        float horizontalDirection = playerInput.rawInput.x;

        if (playerInput.isCameraInverted)
        {
            horizontalDirection *= -1f;
        }

        Vector3 newPosition = transform.position + new Vector3(horizontalDirection, 0, 0) * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;
    }
}