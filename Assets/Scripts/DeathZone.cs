using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Ýçinden geçen objenin Player olup olmadýðýný kontrol et
        if (other.CompareTag("Player"))
        {
            RagdollController ragdoll = other.GetComponent<RagdollController>();

            if (ragdoll != null)
            {
                // Boþluða düþüþte belirli bir çarpma yönü olmadýðý için, 
                // Vector3.up (yukarý) vererek havada ufak bir sekme efektiyle ragdoll'a sokuyoruz.
                ragdoll.Die(Vector3.up);
                Debug.Log("Oyuncu parkurdan aþaðý düþtü, Checkpoint'e dönülüyor!");
            }
        }
    }
}