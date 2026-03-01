using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelData
    {
        public string catPrompt;
        public string option1;
        public string option2;
        public string option3;
        public string correctItem;
        public string utilityText;
    }

    [Header("UI Folders")]
    public GameObject menuPanel;
    public GameObject gameUI;

    [Header("Text Boxes")]
    public TextMeshProUGUI catSpeech;
    public TextMeshProUGUI trueFalseText;

    [Header("Stars & Buttons")]
    public Image[] starImages;
    public TextMeshProUGUI[] buttonTexts;
    public GameObject nextButton;
    public GameObject restartButton;

    private List<LevelData> summerDay = new List<LevelData>() {
        new LevelData { catPrompt = "Nous sommes en juillet, l'été au Québec! Le soleil est très fort. Comment Kim pourra protéger sa tête?", option1 = "Tuque", option2 = "Parapluie", option3 = "Casquette", correctItem = "Casquette", utilityText = "La casquette protège la tête du soleil !" },
        new LevelData { catPrompt = "Kim passe la journée à la plage avec sa famille. Qu’est-ce que Kim devra porter ?", option1 = "Maillot de bain", option2 = "Patins", option3 = "Imperméable", correctItem = "Maillot de bain", utilityText = "Le maillot est parfait pour la baignade !" },
        new LevelData { catPrompt = "Pour se rendre à la plage, quel type de chaussures Kim devrait emporter ?", option1 = "Bottes de pluie", option2 = "Bottes de neige", option3 = "Sandales", correctItem = "Sandales", utilityText = "Les sandales sont idéales pour marcher dans le sable chaud." },
        new LevelData { catPrompt = "Il va faire très chaud sur la plage. Qu’est-ce que Kim pourra acheter pour se rafraîchir ?", option1 = "Chocolat chaud", option2 = "Sac de chips", option3 = "Crème glacée", correctItem = "Crème glacée", utilityText = "Miam ! La crème glacée rafraîchit Kim." },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour se protéger du soleil toute la journée ?", option1 = "Collation", option2 = "Crème solaire", option3 = "Salopette de neige", correctItem = "Crème solaire", utilityText = "La crème solaire protège Kim des brûlures." }
    };

    private List<LevelData> winterDay = new List<LevelData>() {
        new LevelData { catPrompt = "C'est l'hiver au Québec! Les rues sont enneigées. Comment Kim pourra protéger sa tête du froid?", option1 = "Tuque", option2 = "Parapluie", option3 = "Casquette", correctItem = "Tuque", utilityText = "La tuque garde les oreilles de Kim bien au chaud !" },
        new LevelData { catPrompt = "Les lacs sont gelés, c'est le temps de patiner! Qu’est-ce que Kim devra porter sur la patinoire ?", option1 = "Maillot de bain", option2 = "Patins", option3 = "Imperméable", correctItem = "Patins", utilityText = "Avec les patins, Kim peut glisser sur la glace." },
        new LevelData { catPrompt = "Pour se rendre à la patinoire, quel type de chaussures Kim devrait porter ?", option1 = "Bottes de pluie", option2 = "Bottes de neige", option3 = "Sandales", correctItem = "Bottes de neige", utilityText = "Les bottes de neige gardent les pieds au sec." },
        new LevelData { catPrompt = "Dehors, il fait très froid. Qu’est-ce que Kim pourra acheter pour se réchauffer ?", option1 = "Chocolat chaud", option2 = "Sac de chips", option3 = "Crème glacée", correctItem = "Chocolat chaud", utilityText = "Un bon chocolat chaud réchauffe Kim immédiatement !" },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour se protéger du froid toute la journée ?", option1 = "Collation", option2 = "Crème solaire", option3 = "Salopette de neige", correctItem = "Salopette de neige", utilityText = "La salopette empêche Kim d'avoir froid dans la neige." }
    };

    private List<LevelData> activeRounds = new List<LevelData>();
    private int currentRoundIndex = 0;
    private int score = 0;
    private bool isRoundOver = false; // THE SAFETY SWITCH

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        menuPanel.SetActive(true);
        gameUI.SetActive(false);
        nextButton.SetActive(false);
        restartButton.SetActive(false);
        trueFalseText.text = "";

        score = 0;
        currentRoundIndex = 0;

        foreach (Image img in starImages)
        {
            if (img != null) img.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }

        catSpeech.text = "Salut ! C'est quelle saison aujourd'hui ?";
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
        gameUI.SetActive(true);
        SetupRound();
    }

    public void SetupRound()
    {
        isRoundOver = false; // Reset the switch for the new level
        nextButton.SetActive(false);
        trueFalseText.text = "";
        LevelData current = activeRounds[currentRoundIndex];
        catSpeech.text = current.catPrompt;

        buttonTexts[0].text = current.option1;
        buttonTexts[1].text = current.option2;
        buttonTexts[2].text = current.option3;
    }

    public void CheckChoice(TextMeshProUGUI clickedText)
    {
        // If the player already chose, stop right here!
        if (isRoundOver) return;

        isRoundOver = true; // Lock the level
        LevelData current = activeRounds[currentRoundIndex];

        if (clickedText.text == current.correctItem)
        {
            if (score < starImages.Length)
            {
                starImages[score].color = Color.white;
            }
            score++;
            trueFalseText.text = "Bravo !";
            trueFalseText.color = Color.green;
        }
        else
        {
            trueFalseText.text = "Beurk...";
            trueFalseText.color = Color.red;
        }

        catSpeech.text = current.utilityText;
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
        catSpeech.text = "Wow, bravo! Grâce à toi, Kim est prêt pour sa journée!\n\nEn connaissant tous ces mots, tu es un super assistant!";
        trueFalseText.text = "";
        nextButton.SetActive(false);
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        ShowMainMenu();
    }
}