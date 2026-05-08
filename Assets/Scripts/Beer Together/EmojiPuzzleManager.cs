using UnityEngine;
using System.Collections.Generic;

// OYUNDAKÝ GÜNCEL EMOJÝ ÇEÞÝTLERÝMÝZ (Kýzgýn iptal!)
public enum EmojiType { Mutlu, Uzgun, Normal }

public class EmojiPuzzleManager : MonoBehaviour
{
    [Header("Bilmecenin Þifresi")]
    public List<EmojiType> currentPuzzleSequence = new List<EmojiType>();
    public int currentStep = 0; // Oyuncu þu an kaçýncý doðru kapýdan geçti?

    void Start()
    {
        GenerateRandomPuzzle();
    }

    void GenerateRandomPuzzle()
    {
        currentPuzzleSequence.Clear();
        currentStep = 0;

        // 3 aþamalý (3 kapýlýk) rastgele bir þifre oluþtur
        for (int i = 0; i < 3; i++)
        {
            // 0, 1 veya 2 sayýlarýndan birini rastgele seçer (3 dahil deðil)
            EmojiType randomEmoji = (EmojiType)Random.Range(0, 3);
            currentPuzzleSequence.Add(randomEmoji);
        }

        // Test için þifreyi konsola yazdýr (Oyun baþlarken þifreyi buradan kopya çekebilirsin)
        Debug.Log("Þifre: " + currentPuzzleSequence[0] + " - " + currentPuzzleSequence[1] + " - " + currentPuzzleSequence[2]);
    }

    // Kapýlarýn tetiklendiðinde beyne soracaðý soru:
    public bool CheckDoor(EmojiType selectedEmoji)
    {
        // Eðer oyuncu tüm kapýlarý çoktan geçtiyse
        if (currentStep >= currentPuzzleSequence.Count) return false;

        // Girdiði kapýnýn emojisi, þifredeki sýradaki emojiyle ayný mý?
        if (selectedEmoji == currentPuzzleSequence[currentStep])
        {
            currentStep++; // Doðru! Bir sonraki kapý aþamasýna geç
            return true;
        }
        else
        {
            return false; // Yanlýþ kapý!
        }
    }
}