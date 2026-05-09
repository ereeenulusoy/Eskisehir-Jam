using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using StarterAssets;
using Cinemachine;

public class GameEndTrigger : MonoBehaviour
{
    [Header("Bitiþ Kamerasý")]
    [Tooltip("Sahnede þiþeleri gören o havalý Virtual Camera")]
    public CinemachineVirtualCamera winCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tetikleyiciyi hemen kapat ki karakter içindeyken sürekli çalýþmasýn
            GetComponent<Collider>().enabled = false;

            // 1. BÝLEÞENLERÝ YAKALA
            StarterAssetsInputs input = other.GetComponent<StarterAssetsInputs>();
            ThirdPersonController controller = other.GetComponent<ThirdPersonController>();
            Animator animator = other.GetComponent<Animator>();
            UnityEngine.InputSystem.PlayerInput playerInput = other.GetComponent<UnityEngine.InputSystem.PlayerInput>();

            // 2. HAREKETÝ VE KONTROLÜ TAMAMEN KES
            if (input != null)
            {
                input.move = Vector2.zero;
                input.sprint = false;
            }

            // Controller scriptini direkt kapatýyoruz, böylece momentum hesaplayamaz
            if (controller != null) controller.enabled = false;

            // Klavye/Fare baðlantýsýný kopar
            if (playerInput != null) playerInput.enabled = false;

            // 3. ÝDLE ANÝMASYONUNA ZORLA
            if (animator != null)
            {
                animator.SetFloat("Speed", 0f);
                animator.SetFloat("MotionSpeed", 0f);
            }

            // 4. ZAFER KAMERASINA DOÐRU DÖN
            if (winCamera != null)
            {
                // Karakterin sadece Y ekseninde (saða-sola) dönmesi için hedef pozisyonu ayarla
                Vector3 targetPosition = winCamera.transform.position;
                targetPosition.y = other.transform.position.y;
                other.transform.LookAt(targetPosition);
            }

            // 5. DRONE VARSA MOTORUNU DURDUR
            DroneController activeDrone = FindObjectOfType<DroneController>();
            if (activeDrone != null) activeDrone.enabled = false;

            // 6. ZAFER KAMERASINI KRAL YAP
            if (winCamera != null)
            {
                winCamera.Priority = 100; // Statik sistemdeki 20 deðerini ezmesi için 100 veriyoruz
                winCamera.gameObject.SetActive(true);
            }

            StartCoroutine(EndGameRoutine());
        }
    }

    IEnumerator EndGameRoutine()
    {
        // 4 saniye zafer anýný izlet
        yield return new WaitForSeconds(4f);

        // Ana Menü'ye dön (Build Settings'te 0. sýrada olduðundan emin ol)
        SceneManager.LoadScene(0);
    }
}