using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [Tooltip("Gökyüzünün dönüþ hýzý. Yavaþ ve sinematik olmasý için düþük tutun.")]
    public float rotationSpeed = 1.5f;

    void Update()
    {
        // Sahnedeki Skybox materyalinin "Rotation" deðerini zamanla artýr
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}