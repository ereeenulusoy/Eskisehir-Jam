using UnityEngine;

public class FallingTrapTrigger : MonoBehaviour
{
    [Header("Tuzak Ayarlarý")]
    [Tooltip("Tepeden düþecek olan dev þiþe (Prefab)")]
    public GameObject bottlePrefab;

    [Tooltip("Þiþenin tam olarak nerede doðacaðýný belirleyen boþ obje")]
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Sadece Player içinden geçerse tetiklensin
        if (other.CompareTag("Player"))
        {
            if (bottlePrefab != null && spawnPoint != null)
            {
                // Þiþeyi spawn noktasýnda ve o noktanýn açýsýyla yarat
                Instantiate(bottlePrefab, spawnPoint.position, spawnPoint.rotation);
            }

            // Tuzak sadece 1 kere çalýþsýn diye görünmez kapýyý kapat
            gameObject.SetActive(false);
        }
    }
}