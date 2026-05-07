using UnityEngine;
using StarterAssets;
using Cinemachine;

public class LevelTransitionTrigger : MonoBehaviour
{
    [Header("Yeni Bölüm Kurallarý")]
    public int switchToLevel = 3;
    public bool newCanJump;
    public bool newCanControlCharacterHorizontal;
    public bool newIsCameraInverted; // Eski tekli güvenilir þalterimiz
    public bool newCanUseShiftToSlowDown;
    public bool newIsMarioMode;
    public bool newCanControlObstacles;

    [Header("Kamera Kontrolü")]
    public CinemachineVirtualCamera targetCamera;
    public int cameraPriority = 20;

    [Header("Drone Kontrolü")]
    [Tooltip("Bu trigger'a girildiðinde uyanacak Drone (Boþ obje)")]
    public GameObject droneToActivate; // YENÝ EKLENDÝ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

                if (targetCamera != null) targetCamera.Priority = cameraPriority;

                // Drone'u uyandýr!
                if (droneToActivate != null) droneToActivate.SetActive(true);

                gameObject.SetActive(false);
            }
        }
    }
}