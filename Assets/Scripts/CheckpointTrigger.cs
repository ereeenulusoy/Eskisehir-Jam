using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Karakterin o anki tam pozisyonunu hafýzaya yaz
            RagdollController.lastCheckpointPosition = transform.position;
            RagdollController.hasCheckpoint = true;

            Debug.Log("Checkpoint Alýndý: " + transform.position);

            // Üst üste tekrar alýnmasýn diye bu görünmez trigger'ý kapat
            gameObject.SetActive(false);
        }
    }
}