using UnityEngine;
using TMPro; // Fixed: Added the 'ro' back!
using UnityEngine.UI;
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

    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject tutorialImage;
    public GameObject gameUI;

    [Header("UI References")]
    public TextMeshProUGUI catSpeech;
    public Image[] starImages;       // Drag your 5 Black Star objects here
    public Sprite goldStarSprite;    // Drag your Golden Star Sprite asset here
    public TextMeshProUGUI[] buttonTexts;
    public GameObject nextButton;
    public GameObject restartButton;

    // --- LEVELS ---
    private List<LevelData> summerDay = new List<LevelData>() {
        new LevelData { catPrompt = "Le soleil pique mes yeux !", correctItem = "Lunettes", utilityText = "Les lunettes protègent tes yeux." },
        new LevelData { catPrompt = "Il commence à pleuvoir...", correctItem = "Imperméable", utilityText = "L'imperméable te garde au sec." },
        new LevelData { catPrompt = "Le sable est très chaud !", correctItem = "Sandales", utilityText = "Les sandales évitent de se brûler." },
        new LevelData { catPrompt = "C'est la canicule !", correctItem = "Creme Solaire", utilityText = "La crème protège ta peau." },
        new LevelData { catPrompt = "Ma tête chauffe trop !", correctItem = "Casquette", utilityText = "La casquette te garde à l'ombre." }
    };

    private List<LevelData> winterDay = new List<LevelData>() {
        new LevelData { catPrompt = "Mes oreilles sont gelées !", correctItem = "Tuque", utilityText = "La tuque garde tes oreilles au chaud." },
        new LevelData { catPrompt = "On va glisser sur la glace !", correctItem = "Patins", utilityText = "Les patins sont faits pour la glace." },
        new LevelData { catPrompt = "Il y a une tempête !", correctItem = "Salopette", utilityText = "La salopette te protège de la neige." },
        new LevelData { catPrompt = "Mes mains sont froides.", correctItem = "Gants", utilityText = "Les gants gardent tes doigts au chaud." },
        new LevelData { catPrompt = "Je veux jouer dehors !", correctItem = "Manteau", utilityText = "Le manteau est indispensable en hiver." }
    };

    private List<LevelData> activeRounds = new List<LevelData>();
    private int currentRoundIndex = 0;
    private int score = 0;
    private string[] allItems = { "Tuque", "Manteau", "Bottes de neige", "Salopette", "Gants", "Patins", "Casquette", "Lunettes", "Sandales", "Maillot", "Creme Solaire", "Parapluie", "Imperméable", "Bottes de pluie" };

    void Start()
    {
        menuPanel.SetActive(true);
        if (tutorialImage != null) tutorialImage.SetActive(true);
        gameUI.SetActive(false);
        nextButton.SetActive(false);
        restartButton.SetActive(false);
        catSpeech.text = "Chat : Salut ! C'est quelle saison aujourd'hui ?";
    }

    public void ChooseSummer()
    {
        activeRounds = new List<LevelData>(summerDay);
        StartGame();
    }

    public void ChooseWinter()
    {
        activeRounds = new List<LevelData>(winterDay);
        StartGame();
    }

    void StartGame()
    {
        menuPanel.SetActive(false);
        if (tutorialImage != null) tutorialImage.SetActive(false);
        gameUI.SetActive(true);
        SetupRound();
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