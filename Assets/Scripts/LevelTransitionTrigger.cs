using UnityEngine;
using StarterAssets;
using Cinemachine; // Bunu eklemeyi unutma!

public class LevelTransitionTrigger : MonoBehaviour
{
    [Header("Yeni Bölüm Kurallarý")]
    public int switchToLevel = 3;
    public bool newCanJump;
    public bool newCanControlCharacterHorizontal;
    public bool newIsCameraInverted;

    [Header("Kamera Kontrolü")]
    [Tooltip("Bu bölüme geçince aktif olacak kamera")]
    public CinemachineVirtualCamera targetCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StarterAssetsInputs input = other.GetComponent<StarterAssetsInputs>();
            if (input != null)
            {
                // Kurallarý uygula
                input.currentActiveLevel = switchToLevel;
                input.canJump = newCanJump;
                input.canControlCharacterHorizontal = newCanControlCharacterHorizontal;
                input.isCameraInverted = newIsCameraInverted;

                // Kamerayý Deðiþtir
                if (targetCamera != null)
                {
                    // Diðer tüm kameralarýn önceliðini aþmak için yüksek bir deðer ver
                    targetCamera.Priority = 20;
                }

                gameObject.SetActive(false);
            }
        }
    }
}