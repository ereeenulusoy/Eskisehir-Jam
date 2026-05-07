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

        // --- YENÝ KURAL ---
        // Eðer oyun Mario (2.5D) modundaysa engeller KESÝNLÝKLE hareket etmez!
        if (playerInput.isMarioMode) return;
        // ------------------

        if (playerInput.canControlCharacterHorizontal) return;
        if (myLevel != playerInput.currentActiveLevel) return;

        float horizontalDirection = playerInput.rawInput.x;

        // EÐER KAMERA TERSSE A VE D TUÞLARININ YÖNÜNÜ TERSÝNE ÇEVÝR
        if (playerInput.isCameraInverted)
        {
            horizontalDirection *= -1f;
        }

        Vector3 newPosition = transform.position + new Vector3(horizontalDirection, 0, 0) * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;
    }
}