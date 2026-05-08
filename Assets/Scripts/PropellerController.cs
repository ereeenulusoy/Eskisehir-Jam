using UnityEngine;

public class PropellerController : MonoBehaviour
{
    [Tooltip("Pervanenin dönüþ hýzý (Eksi deðer verirsen tersine döner)")]
    public float rotationSpeed = 200f;

    [Tooltip("Hangi eksende dönecek? (Örn: X ekseninde dönmesi için X'i 1 yap, diðerlerini 0)")]
    public Vector3 rotationAxis = new Vector3(0, 1, 0);

    void Update()
    {
        // Pervaneyi belirlediðimiz eksende ve hýzda sürekli döndür
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}