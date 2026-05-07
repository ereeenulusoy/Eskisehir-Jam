using UnityEngine;
using StarterAssets;

public class RagdollController : MonoBehaviour
{
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

    // Yandan gelen çarpýþmalarý (Triggerlarý) yakalamak için
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

    // Die metoduna artýk engelin bizi ne tarafa ittiði bilgisini (impactNormal) veriyoruz
    public void Die(Vector3 impactNormal)
    {
        SetRagdollState(true);

        // 1. Engellerin okuduðu son tuþ girdilerini (basýlý kalan tuþlarý) zorla SIFIRLA
        if (_input != null)
        {
            _input.isMovementStarted = false;
            _input.rawInput = Vector2.zero; // <--- Engellerin hareketini anýnda keser
            _input.move = Vector2.zero;
        }

        // 2. Oyuncunun klavye/mouse baðlantýsýný tamamen KES (Sanal olarak fiþi çekiyoruz)
        UnityEngine.InputSystem.PlayerInput playerInputComponent = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (playerInputComponent != null)
        {
            playerInputComponent.enabled = false;
        }

        // Doðal fizik hesaplamasý
        foreach (Rigidbody rb in _boneRigidbodies)
        {
            // 1. Karakterin koþudan gelen kendi ileri ivmesi (biraz azaltýlmýþ hali)
            Vector3 runMomentum = transform.forward * 4f;

            // 2. Senin bahsettiðin o doðal "pulse" - engelden karaktere doðru seken güç
            Vector3 bouncePulse = impactNormal * 6f;

            // Ýkisini birleþtirip kemiklere anlýk güç (Impulse) olarak uyguluyoruz
            rb.AddForce(runMomentum + bouncePulse, ForceMode.Impulse);
        }
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

            // Ragdoll aktifse Kinematic kapanýr (fizik motoru devralýr)
            rb.isKinematic = !isRagdollActive;
        }
    }
}