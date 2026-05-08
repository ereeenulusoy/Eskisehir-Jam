using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement; // Sahne yenileme iþlemleri için eklendi

public class RagdollController : MonoBehaviour
{
    // --- CHECKPOINT HAFIZASI (Static: Sahne yenilense bile veriler silinmez) ---
    public static Vector3 lastCheckpointPosition;
    public static bool hasCheckpoint = false;
    // --------------------------------------------------------------------------

    private Animator _animator;
    private CharacterController _characterController;
    private ThirdPersonController _tpController;
    private StarterAssetsInputs _input;

    // Mahluk'un içindeki tüm kemik fizikleri
    private Rigidbody[] _boneRigidbodies;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _tpController = GetComponent<ThirdPersonController>();
        _input = GetComponent<StarterAssetsInputs>();

        // Karakterin altýndaki (kemiklerdeki) tüm Rigidbody'leri bul
        _boneRigidbodies = GetComponentsInChildren<Rigidbody>();

        // Oyun baþlarken Ragdoll'u kapalý tut
        SetRagdollState(false);

        // --- CHECKPOINT KONTROLÜ ---
        // Eðer oyuncu daha önce bir checkpoint aldýysa ve sahne yeniden yüklendiyse
        if (hasCheckpoint)
        {
            // CharacterController açýkken Unity transform.position ile ýþýnlamaya izin vermez.
            // Bu yüzden önce kapatýp, ýþýnlayýp, sonra tekrar açýyoruz.
            _characterController.enabled = false;
            transform.position = lastCheckpointPosition;
            _characterController.enabled = true;

            Debug.Log("Karakter Checkpoint noktasýnda doðdu: " + lastCheckpointPosition);
        }
    }

    // Karakter KATIK BÝR CÝSME fiziksel olarak çarptýðýnda Unity bu metodu çaðýrýr
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Çarptýðýmýz þeyin etiketi Obstacle ise
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            // hit.normal = Çarptýðýmýz yüzeyin bize doðru olan dik açýsý (geri tepme yönü)
            Die(hit.normal);
        }
    }

    // Yandan gelen çarpýþmalarý (Görünmez Triggerlarý) yakalamak için
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            // Yandan çarpmalarda engelin bize vurma yönünü (sahte normal) manuel hesaplýyoruz
            Vector3 fakeNormal = (transform.position - other.transform.position).normalized;

            // Havaya doðru sekmesi için Y eksenini biraz yukarý kaldýrýyoruz
            fakeNormal.y = 0.5f;

            Die(fakeNormal.normalized);
        }
    }

    // Ölüm ve Ragdoll tetikleyicisi
    public void Die(Vector3 impactNormal)
    {
        SetRagdollState(true);

        // 1. Engellerin okuduðu son tuþ girdilerini (basýlý kalan tuþlarý) zorla SIFIRLA
        if (_input != null)
        {
            _input.isMovementStarted = false;
            _input.rawInput = Vector2.zero; // Engellerin hareketini anýnda keser
            _input.move = Vector2.zero;
        }

        // 2. Oyuncunun klavye/mouse baðlantýsýný tamamen KES (Sanal olarak fiþi çekiyoruz)
        UnityEngine.InputSystem.PlayerInput playerInputComponent = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (playerInputComponent != null)
        {
            playerInputComponent.enabled = false;
        }

        // 3. EÐER SAHNEDE UÇAN BÝR DRONE VARSA MOTORUNU ANINDA DURDUR!
        DroneController activeDrone = FindObjectOfType<DroneController>();
        if (activeDrone != null)
        {
            activeDrone.enabled = false;
        }

        // 4. Doðal fizik hesaplamasý (Çarpma Hissiyatý)
        foreach (Rigidbody rb in _boneRigidbodies)
        {
            // Karakterin koþudan gelen kendi ileri ivmesi (biraz azaltýlmýþ hali)
            Vector3 runMomentum = transform.forward * 4f;

            // Engelden karaktere doðru seken güç (Pulse)
            Vector3 bouncePulse = impactNormal * 6f;

            // Ýkisini birleþtirip kemiklere anlýk güç (Impulse) olarak uyguluyoruz
            rb.AddForce(runMomentum + bouncePulse, ForceMode.Impulse);
        }

        // 5. Ölüm animasyonunu izletip sahneyi yenilemek için sayacý baþlat
        StartCoroutine(RestartSceneRoutine());
    }

    // Sahne yenileme sayacý
    private System.Collections.IEnumerator RestartSceneRoutine()
    {
        // Oyuncuya karakterinin yere yapýþmasýný izlemesi için 2.5 saniye ver
        yield return new WaitForSeconds(2f);

        // Mevcut sahneyi en baþtan yükle (Tüm kýrýlan/düþen engeller, dronlar, þiþeler sýfýrlanýr)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetRagdollState(bool isRagdollActive)
    {
        // 1. Eðer Ragdoll aktifse, Standart Kontrolleri ve Animasyonu KAPAT
        if (_animator != null) _animator.enabled = !isRagdollActive;
        if (_characterController != null) _characterController.enabled = !isRagdollActive;
        if (_tpController != null) _tpController.enabled = !isRagdollActive;

        // 2. Kemik fiziklerini yönet
        foreach (Rigidbody rb in _boneRigidbodies)
        {
            // Kendi ana objemizde bir Rigidbody varsa onu atla
            if (rb.gameObject == gameObject) continue;

            // Ragdoll aktifse Kinematic kapanýr (fizik motoru devralýr), kapalýysa Kinematic açýlýr
            rb.isKinematic = !isRagdollActive;
        }
    }
}