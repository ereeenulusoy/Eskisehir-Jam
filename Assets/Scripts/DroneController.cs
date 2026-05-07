using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Tooltip("Drone'un Z eksenindeki hýzý. Karakterden (örn: 5) yüksek olmalý!")]
    public float speed = 7f;

    void Update()
    {
        // Drone'u sürekli olarak dünya Z ekseninde (ileri) hareket ettirir
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}