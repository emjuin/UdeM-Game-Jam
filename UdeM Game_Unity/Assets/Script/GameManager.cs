using UnityEngine;
using TMPro;
using System.Collections.Generic; // Needed for Lists

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI catSpeech;
    public TextMeshProUGUI[] buttonTexts; // Drag your 3 Button Text(TMP) objects here
    public GameObject nextButton;

    [Header("Settings")]
    public string currentMode = "Summer";

    // The Full Database of 10 items
    private string[] allClothes = {
        "Tuque", "Habit de neige", "Maillot", "Casquette", "Imperméable",
        "Bottes", "Sandales", "Manteau", "Shorts", "Gants"
    };

    void Start()
    {
        SetupRound();
    }

    public void SetupRound()
    {
        nextButton.SetActive(false);
        string winner = GetWinnerForMode();

        // 1. Create a list of "Losers" (everything except the winner)
        List<string> losers = new List<string>();
        foreach (string item in allClothes)
        {
            if (item != winner) losers.Add(item);
        }

        // 2. Pick 2 random losers
        string fake1 = losers[Random.Range(0, losers.Count)];
        losers.Remove(fake1); // Don't pick the same one twice
        string fake2 = losers[Random.Range(0, losers.Count)];

        // 3. Put them in a list and Shuffle
        List<string> choices = new List<string> { winner, fake1, fake2 };
        for (int i = 0; i < choices.Count; i++)
        {
            string temp = choices[i];
            int randomIndex = Random.Range(i, choices.Count);
            choices[i] = choices[randomIndex];
            choices[randomIndex] = temp;
        }

        // 4. Push the text to the actual buttons
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            buttonTexts[i].text = choices[i];
        }

        catSpeech.text = "Chat : Trouve ce qu'il me faut pour " + GetSeasonName() + " !";
    }

    public void CheckChoice(TextMeshProUGUI clickedText)
    {
        if (clickedText.text == GetWinnerForMode())
        {
            catSpeech.text = "Chat : Bravo ! Le " + clickedText.text + " est parfait !";
            nextButton.SetActive(true);
        }
        else
        {
            catSpeech.text = "Chat : Beurk ! Pas de " + clickedText.text + " !";
        }
    }

    // (Keep the GetWinnerForMode and GetSeasonName functions from the previous script here)
    private string GetWinnerForMode()
    {
        if (currentMode == "Summer") return "Maillot";
        if (currentMode == "SpecialSummer") return "Casquette";
        if (currentMode == "Winter") return "Tuque";
        if (currentMode == "SpecialWinter") return "Habit de neige";
        return "Imperméable";
    }

    private string GetSeasonName()
    {
        if (currentMode == "Summer") return "l'Été";
        if (currentMode == "SpecialSummer") return "la Canicule";
        return "cette saison";
    }
}