using UnityEngine;
using StarterAssets;
using Cinemachine;

public class LevelTransitionTrigger : MonoBehaviour
{
    [Header("Yeni Bölüm Kurallarý")]
    public int switchToLevel = 3;
    public bool newCanJump;
    public bool newCanControlCharacterHorizontal;
    public bool newIsCameraInverted;
    public bool newCanUseShiftToSlowDown;

    [Header("Kamera Kontrolü")]
    [Tooltip("Bu bölüme geçince aktif olacak kamera")]
    public CinemachineVirtualCamera targetCamera;

    [Tooltip("Bu kameranýn öncelik gücü. Bir önceki leveldan DAHA YÜKSEK olmalý.")]
    public int cameraPriority = 20; // <--- YENÝ EKLENDÝ

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
                input.canUseShiftToSlowDown = newCanUseShiftToSlowDown;

                // Kamerayý Deðiþtir
                if (targetCamera != null)
                {
                    // Artýk önceliði Inspector'dan belirlediðimiz sayýya çekiyoruz
                    targetCamera.Priority = cameraPriority;
                }

                gameObject.SetActive(false);
            }
        }
    }
}