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
    public bool newIsMarioMode;
    public bool newCanControlObstacles;

    [Header("UI Ayarlarý")]
    [TextArea] // Inspector'da uzun uzun kural yazabilmen için büyük bir kutu açar
    public string thisLevelRules;

    [Header("Kamera Kontrolü")]
    public CinemachineVirtualCamera targetCamera;

    // Artýk her trigger'da 20, 30 yazmana GEREK YOK!
    // Tüm trigger'lar en yüksek önceliðin 20 olduðunu bilecek.
    private int activePriority = 20;
    private int standbyPriority = 10;

    // STATIC deðiþken: Sahnede çalýþan tüm trigger'lar bu deðiþkeni ORTAK kullanýr.
    // Yani en son hangi kameranýn aktif olduðunu hepsi bilir.
    private static CinemachineVirtualCamera currentActiveCamera;

    [Header("Drone Kontrolü")]
    public GameObject droneToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ekranda kural yazýsýný güncelle
            if (InGameUIManager.instance != null)
            {
                InGameUIManager.instance.UpdateRules(thisLevelRules);
            }

            StarterAssetsInputs input = other.GetComponent<StarterAssetsInputs>();
            if (input != null)
            {
                input.currentActiveLevel = switchToLevel;
                input.canJump = newCanJump;
                input.canControlCharacterHorizontal = newCanControlCharacterHorizontal;
                input.isCameraInverted = newIsCameraInverted;
                input.canUseShiftToSlowDown = newCanUseShiftToSlowDown;
                input.isMarioMode = newIsMarioMode;
                input.canControlObstacles = newCanControlObstacles;

                // --- KAMERA GEÇÝÞ BÜYÜSÜ BURADA ---
                if (targetCamera != null)
                {
                    // 1. Eðer halihazýrda aktif bir kamera varsa, onu uyku moduna (10) al.
                    if (currentActiveCamera != null)
                    {
                        currentActiveCamera.Priority = standbyPriority;
                    }

                    // 2. Girdiðimiz kapýnýn kamerasýný Kral (20) yap.
                    targetCamera.Priority = activePriority;

                    // 3. Yeni aktif kamerayý sisteme kaydet ki bir sonraki kapý bilsin.
                    currentActiveCamera = targetCamera;
                }
                // ----------------------------------

                // Drone'u uyandýr!
                if (droneToActivate != null) droneToActivate.SetActive(true);

                gameObject.SetActive(false);
            }
        }
    }
}