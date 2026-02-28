using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for Image!
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelData
    {
        public string catPrompt;
        public string correctItem;
        public string utilityText;
    }

    [Header("UI References")]
    public TextMeshProUGUI catSpeech;
    public Image[] starImages; // DRAG YOUR 5 STAR IMAGES HERE
    public Sprite goldStarSprite; // DRAG YOUR SHINY STAR SPRITE HERE
    public TextMeshProUGUI[] buttonTexts;
    public GameObject nextButton;
    public GameObject restartButton;

    // --- LEVELS ---
    private List<LevelData> summerDay = new List<LevelData>() {
        new LevelData { catPrompt = "Le soleil pique mes yeux !", correctItem = "Lunettes", utilityText = "Les lunettes protègent tes yeux du soleil." },
        new LevelData { catPrompt = "Il commence à pleuvoir un peu...", correctItem = "Imperméable", utilityText = "L'imperméable te garde au sec sous la pluie." },
        new LevelData { catPrompt = "Le sable est très chaud !", correctItem = "Sandales", utilityText = "Les sandales évitent de se brûler les pieds." },
        new LevelData { catPrompt = "Je ne veux pas attraper de coup de soleil.", correctItem = "Creme Solaire", utilityText = "La crème protège ta peau des rayons." },
        new LevelData { catPrompt = "Ma tête chauffe trop !", correctItem = "Casquette", utilityText = "La casquette garde ta tête à l'ombre." }
    };

    private List<LevelData> winterDay = new List<LevelData>() {
        new LevelData { catPrompt = "Mes oreilles sont gelées !", correctItem = "Tuque", utilityText = "La tuque garde tes oreilles bien au chaud." },
        new LevelData { catPrompt = "On va glisser sur la glace !", correctItem = "Patins", utilityText = "Les patins permettent de glisser sur la patinoire." },
        new LevelData { catPrompt = "Il y a une tempête de neige !", correctItem = "Salopette", utilityText = "La salopette empêche la neige d'entrer." },
        new LevelData { catPrompt = "Mes mains sont toutes froides.", correctItem = "Gants", utilityText = "Les gants protègent tes doigts du gel." },
        new LevelData { catPrompt = "Je veux faire un bonhomme de neige !", correctItem = "Manteau", utilityText = "Le manteau est essentiel pour rester dehors." }
    };

    private List<LevelData> activeRounds = new List<LevelData>();
    private int currentRoundIndex = 0;
    private int score = 0;

    private string[] allItems = { "Tuque", "Manteau", "Bottes de neige", "Salopette", "Gants", "Patins", "Casquette", "Lunettes", "Sandales", "Maillot", "Creme Solaire", "Parapluie", "Imperméable", "Bottes de pluie" };

    void Start()
    {
        if (restartButton != null) restartButton.SetActive(false);
        PickDayTheme();
        SetupRound();
    }

    void PickDayTheme()
    {
        activeRounds = (Random.value > 0.5f) ? new List<LevelData>(summerDay) : new List<LevelData>(winterDay);
    }

    public void SetupRound()
    {
        nextButton.SetActive(false);
        LevelData current = activeRounds[currentRoundIndex];
        catSpeech.text = "Chat : " + current.catPrompt;

        List<string> choices = new List<string> { current.correctItem };
        List<string> losers = new List<string>(allItems);
        losers.Remove(current.correctItem);

        for (int i = 0; i < 2; i++)
        {
            int r = Random.Range(0, losers.Count);
            choices.Add(losers[r]);
            losers.RemoveAt(r);
        }

        for (int i = 0; i < choices.Count; i++)
        {
            int rnd = Random.Range(i, choices.Count);
            string temp = choices[i];
            choices[i] = choices[rnd];
            choices[rnd] = temp;
            buttonTexts[i].text = choices[i];
        }
    }

    public void CheckChoice(TextMeshProUGUI clickedText)
    {
        LevelData current = activeRounds[currentRoundIndex];

        if (clickedText.text == current.correctItem)
        {
            // Update the star image at the current score index
            if (score < starImages.Length)
            {
                starImages[score].sprite = goldStarSprite;
            }
            score++;
            catSpeech.text = "<b>Bravo !</b>\n" + current.utilityText;
        }
        else
        {
            catSpeech.text = "<b>Beurk...</b>\n" + current.utilityText;
        }

        nextButton.SetActive(true);
    }

    public void GoToNextRound()
    {
        currentRoundIndex++;
        if (currentRoundIndex < 5) SetupRound();
        else EndGame();
    }

    void EndGame()
    {
        catSpeech.text = "Partie terminée ! Score : " + score + "/5";
        nextButton.SetActive(false);
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}