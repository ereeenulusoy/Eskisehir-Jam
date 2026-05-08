using UnityEngine;
using TMPro; // TextMeshPro kullanmak için þart

public class InGameUIManager : MonoBehaviour
{
    // Singleton mantýðý: Sahnedeki diðer tüm scriptler bu UI'a anýnda ulaþabilsin diye
    public static InGameUIManager instance;

    [Tooltip("Tomarýn içindeki TextMeshPro objesini buraya sürükle")]
    public TextMeshProUGUI ruleText;

    private void Awake()
    {
        // Oyun baþlarken kendini sisteme kaydet
        instance = this;
    }

    // Bu metodu tetikleyicilerimizden (Trigger) çaðýracaðýz
    public void UpdateRules(string newRules)
    {
        if (ruleText != null)
        {
            ruleText.text = newRules;
        }
    }
}