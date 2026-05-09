using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        // 1. Eðer sahnede halihazýrda çalan bir müzik yöneticisi varsa...
        if (instance != null)
        {
            // Bu yeni oluþan kopyayý anýnda yok et (Müziklerin üst üste binmesini engeller)
            Destroy(gameObject);
            return;
        }

        // 2. Eðer yoksa, bunu ana yönetici ilan et
        instance = this;

        // 3. Sahneler deðiþse bile bu objeyi asla yok etme!
        DontDestroyOnLoad(gameObject);
    }
}