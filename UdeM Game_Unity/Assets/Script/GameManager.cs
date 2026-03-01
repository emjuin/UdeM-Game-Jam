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

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip catMeowSound;
    public AudioClip correctSound; // NEW: Drag a success chime here!

    private List<LevelData> summerDay = new List<LevelData>() {
        new LevelData { catPrompt = "Nous sommes en juillet. C'est l'été au Québec! Le soleil est très fort. Comment Kim pourra protéger sa tête?", option1 = "Une tuque", option2 = "Un parapluie", option3 = "Une casquette", correctItem = "Une casquette", utilityText = "La casquette (cap) protège la tête du soleil !" },
        new LevelData { catPrompt = "Kim passe la journée à la plage avec sa famille. Qu’est-ce que Kim devra porter ?", option1 = "Un maillot de bain", option2 = "Des patins", option3 = "Un imperméable", correctItem = "Un maillot de bain", utilityText = "Le maillot (swiw suit) est parfait pour la baignade !" },
        new LevelData { catPrompt = "Pour se rendre à la plage, quel type de chaussures Kim devrait emporter ?", option1 = "Des bottes de pluie", option2 = "Des bottes de neige", option3 = "Des andales", correctItem = "Des sandales", utilityText = "Les sandales (sandals) sont idéales pour marcher dans le sable chaud." },
        new LevelData { catPrompt = "Il va faire très chaud sur la plage. Qu’est-ce que Kim pourra acheter pour se rafraîchir ?", option1 = "Du chocolat chaud", option2 = "Un sac de chips", option3 = "De la crème glacée", correctItem = "De la crème glacée", utilityText = "Miam ! La crème glacée (ice cream) rafraîchit Kim." },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour se protéger du soleil toute la journée ?", option1 = "Une collation", option2 = "De la crème solaire", option3 = "Une salopette de neige", correctItem = "De la crème solaire", utilityText = "La crème solaire (sun screen) protège Kim des brûlures." }
    };

    private List<LevelData> winterDay = new List<LevelData>() {
        new LevelData { catPrompt = "C'est l'hiver au Québec! Les rues sont enneigées. Comment Kim pourra protéger sa tête du froid?", option1 = "Tuque", option2 = "Un parapluie", option3 = "Une casquette", correctItem = "Une tuque", utilityText = "La tuque (beanie) garde les oreilles de Kim bien au chaud !" },
        new LevelData { catPrompt = "Les lacs sont gelés, c'est le temps de patiner! Qu’est-ce que Kim devra porter à la patinoire ?", option1 = "Un maillot de bain", option2 = "Des patins", option3 = "Un imperméable", correctItem = "Des patins", utilityText = "Avec les patins (ice skates), Kim peut glisser sur la glace." },
        new LevelData { catPrompt = "Pour se rendre à la patinoire, quel type de chaussures Kim devrait porter ?", option1 = "Des bottes de pluie", option2 = "Des bottes de neige", option3 = "Des sandales", correctItem = "Des bottes de neige", utilityText = "Les bottes de neige (snow boots) gardent les pieds au sec." },
        new LevelData { catPrompt = "Dehors, il fait très froid. Qu’est-ce que Kim pourra acheter pour se réchauffer ?", option1 = "Du chocolat chaud", option2 = "Un sac de chips", option3 = "De la crème glacée", correctItem = "Du chocolat chaud", utilityText = "Un bon chocolat chaud (hot chocolate) réchauffe Kim immédiatement !" },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour se protéger du froid toute la journée ?", option1 = "Une collation", option2 = "De la crème solaire", option3 = "Une salopette de neige", correctItem = "Une salopette de neige", utilityText = "La salopette (snow pants) empêche Kim d'avoir froid dans la neige." }
    };

    private List<LevelData> activeRounds = new List<LevelData>();
    private int currentRoundIndex = 0;
    private int score = 0;
    private bool isRoundOver = false;

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
        foreach (Image img in starImages) { if (img != null) img.color = new Color(0.2f, 0.2f, 0.2f, 1f); }
        catSpeech.text = "Salut ! Je suis le chat de Kim, un enfant récemment arrivé au Québec! Kim dort. Aide-moi à préparer ce dont Kim aura besoin pour la journée qui l’attend. \n\n Choisis la saison que tu préfères, puis clique sur la meilleure option à chaque choix .";
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
        isRoundOver = false;
        nextButton.SetActive(false);
        trueFalseText.text = "";
        LevelData current = activeRounds[currentRoundIndex];
        catSpeech.text = current.catPrompt;

        if (sfxSource != null && catMeowSound != null)
        {
            sfxSource.PlayOneShot(catMeowSound);
        }

        buttonTexts[0].text = current.option1;
        buttonTexts[1].text = current.option2;
        buttonTexts[2].text = current.option3;
    }

    public void CheckChoice(TextMeshProUGUI clickedText)
    {
        if (isRoundOver) return;
        isRoundOver = true;
        LevelData current = activeRounds[currentRoundIndex];

        if (clickedText.text == current.correctItem)
        {
            if (score < starImages.Length) starImages[score].color = Color.white;
            score++;
            trueFalseText.text = "Bravo !";
            trueFalseText.color = Color.green;

            // Play Correct Chime
            if (sfxSource != null && correctSound != null) sfxSource.PlayOneShot(correctSound);
        }
        else
        {
            trueFalseText.text = "Ce n'est pas tout à fait ça, mais bien essayé! C'est en essayant qu'on apprend!";
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
        catSpeech.text = "Wow, bravo! Grâce à toi, Kim est prêt pour sa journée!\n\nTu es un super assistant!";
        trueFalseText.text = "";
        nextButton.SetActive(false);
        restartButton.SetActive(true);
    }

    public void RestartGame()
    {
        ShowMainMenu();
    }
}