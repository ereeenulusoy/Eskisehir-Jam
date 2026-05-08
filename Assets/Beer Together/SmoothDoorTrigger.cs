using UnityEngine;
using System.Collections;

public class SmoothDoorTrigger : MonoBehaviour
{
    [Header("Baðlantýlar")]
    [Tooltip("Açýlacak olan asýl kapý modeli (Pivotu kiriþte/menteþede olmalý!)")]
    public GameObject targetDoor;

    [Header("Smooth Açýlma Ayarlarý")]
    [Tooltip("Kapýnýn tamamen açýlmasý kaç saniye sürsün?")]
    public float openDuration = 0.4f;

    [Tooltip("Kapý Z ekseninde kaç derece açýlsýn? (Ýçeri/Dýþarý durumuna göre 90 veya -90)")]
    public float openAngle = 90f; // -Z'yi hedeflediðimiz için varsayýlaný -90 yaptým

    private bool isOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        // Ýçinden geçen Player ise, kapý baðlýysa ve daha önce açýlmadýysa
        if (other.CompareTag("Player") && !isOpened && targetDoor != null)
        {
            isOpened = true;

            // Bu görünmez tetikleyiciyi hemen kapatýyoruz
            GetComponent<Collider>().enabled = false;

            // 0.4 saniyelik pürüzsüz açýlma hareketini baþlat
            StartCoroutine(RotateDoorSmoothly());
        }
    }

    IEnumerator RotateDoorSmoothly()
    {
        // Baþlangýç rotasyonunu kaydet
        Quaternion startRotation = targetDoor.transform.rotation;

        // --- DEÐÝÞEN KISIM BURASI ---
        // Hedef rotasyonu hesapla (Artýk X ve Y sýfýr, Z ekseni etrafýnda dönüyor)
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, openAngle);

        float elapsedTime = 0f;

        // 0.4 saniye boyunca kapýyý Z ekseninde kaydýrarak aç
        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / openDuration;

            targetDoor.transform.rotation = Quaternion.Slerp(startRotation, endRotation, normalizedTime);

            yield return null;
        }

        // Animasyon bitince milimetrik olarak hedefe oturt
        targetDoor.transform.rotation = endRotation;
    }
}