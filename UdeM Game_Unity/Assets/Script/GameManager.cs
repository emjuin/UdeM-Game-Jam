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
        public string option1, option2, option3;
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
    public Button[] optionButtons;
    public TextMeshProUGUI[] buttonTexts;
    public GameObject nextButton;
    public GameObject restartButton;
    public GameObject quitButton;

    [Header("Cat Visuals")]
    public GameObject catNormal;
    public GameObject catWrong;
    public GameObject catHappy;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip catMeowSound;
    public AudioClip correctSound;

    private List<LevelData> summerDay = new List<LevelData>() {
        new LevelData { catPrompt = "Le soleil est très fort. Comment Kim pourra protéger sa tête?", option1 = "Une tuque", option2 = "Un parapluie", option3 = "Une casquette", correctItem = "Une casquette", utilityText = "Une casquette protège la tête du soleil !" },
        new LevelData { catPrompt = "Kim passe la journée à la plage. Qu’est-ce que Kim devra porter ?", option1 = "Un maillot de bain", option2 = "Des patins", option3 = "Un imperméable", correctItem = "Un maillot de bain", utilityText = "Un maillot de bain est parfait pour la baignade !" },
        new LevelData { catPrompt = "Quelles chaussures Kim devrait emporter à la plage ?", option1 = "Des bottes de pluie", option2 = "Des bottes de neige", option3 = "Des sandales", correctItem = "Des sandales", utilityText = "Des sandales sont idéales pour le sable chaud." },
        new LevelData { catPrompt = "Il va faire très chaud ! Qu’est-ce que Kim pourra acheter pour se rafraîchir ?", option1 = "Un chocolat chaud", option2 = "Un sac de chips", option3 = "Une crème glacée", correctItem = "Une crème glacée", utilityText = "Miam ! Une crème glacée rafraîchit Kim." },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour se protéger du soleil toute la journée ?", option1 = "Une collation", option2 = "De la crème solaire", option3 = "Une salopette de neige", correctItem = "De la crème solaire", utilityText = "La crème solaire protège Kim des brûlures." }
    };

    private List<LevelData> winterDay = new List<LevelData>() {
        new LevelData { catPrompt = "C'est l'hiver ! Comment Kim pourra protéger sa tête du froid ?", option1 = "Une tuque", option2 = "Un parapluie", option3 = "Une casquette", correctItem = "Une tuque", utilityText = "Une tuque garde les oreilles au chaud !" },
        new LevelData { catPrompt = "Qu’est-ce que Kim devra porter à la patinoire ?", option1 = "Un maillot de bain", option2 = "Des patins", option3 = "Un imperméable", correctItem = "Des patins", utilityText = "Avec des patins, Kim peut glisser sur la glace." },
        new LevelData { catPrompt = "Pour se rendre à la patinoire, quel type de chaussures Kim devrait porter ?", option1 = "Des bottes de pluie", option2 = "Des bottes de neige", option3 = "Des sandales", correctItem = "Des bottes de neige", utilityText = "Des bottes de neige gardent les pieds au sec." },
        new LevelData { catPrompt = "Dehors, il fait très froid. Qu’est-ce que Kim pourra acheter pour se réchauffer ?", option1 = "Un chocolat chaud", option2 = "Un sac de chips", option3 = "Une crème glacée", correctItem = "Un chocolat chaud", utilityText = "Un bon chocolat chaud réchauffe Kim immédiatement !" },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour se protéger du froid toute la journée ?", option1 = "Une collation", option2 = "De la crème solaire", option3 = "Une salopette de neige", correctItem = "Une salopette de neige", utilityText = "Une salopette empêche Kim d'avoir froid dans la neige." }
    };

    private List<LevelData> activeRounds = new List<LevelData>();
    private int currentRoundIndex = 0;
    private int score = 0;
    private bool isRoundOver = false;

    void Start() { ShowMainMenu(); }

    public void ShowMainMenu()
    {
        menuPanel.SetActive(true);
        gameUI.SetActive(false);
        nextButton.SetActive(false);
        restartButton.SetActive(false);
        if (quitButton != null) quitButton.SetActive(true);
        SetCatFace("normal");
        trueFalseText.text = ""; score = 0; currentRoundIndex = 0;
        foreach (Image img in starImages) { if (img != null) img.color = new Color(0.2f, 0.2f, 0.2f, 1f); }
        catSpeech.text = "Salut, je suis le chat de Kim, un enfant récemment arrivé au Québec! Kim dort. Aide moi à préparer sa journée en attendant.\n\nChoisis la saison que tu préfères, puis clique sur la meilleure option à chaque choix. !";
    }

    void SetCatFace(string type)
    {
        catNormal.SetActive(type == "normal");
        catWrong.SetActive(type == "wrong");
        catHappy.SetActive(type == "happy");
    }

    public void ChooseSummer() { activeRounds = new List<LevelData>(summerDay); StartGame(); }
    public void ChooseWinter() { activeRounds = new List<LevelData>(winterDay); StartGame(); }

    void StartGame()
    {
        menuPanel.SetActive(false);
        gameUI.SetActive(true);
        SetupRound();
    }

    public void SetupRound()
    {
        isRoundOver = false; nextButton.SetActive(false); trueFalseText.text = "";
        SetCatFace("normal");
        LevelData current = activeRounds[currentRoundIndex];
        catSpeech.text = current.catPrompt;
        if (sfxSource != null && catMeowSound != null) sfxSource.PlayOneShot(catMeowSound);
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].image.color = Color.white;
            buttonTexts[i].text = (i == 0) ? current.option1 : (i == 1) ? current.option2 : current.option3;
        }
    }

    public void CheckChoice(TextMeshProUGUI clickedText)
    {
        if (isRoundOver) return;
        isRoundOver = true;
        LevelData current = activeRounds[currentRoundIndex];
        foreach (Button btn in optionButtons) { btn.image.color = new Color(0.5f, 0.5f, 0.5f, 1f); }
        clickedText.transform.parent.GetComponent<Image>().color = Color.white;

        if (clickedText.text.Trim().ToLower() == current.correctItem.Trim().ToLower())
        {
            if (score < starImages.Length) starImages[score].color = Color.white;
            score++;
            trueFalseText.text = "Bravo ! C'est la bonne réponse !";
            trueFalseText.color = Color.green;
            SetCatFace("happy");
            if (sfxSource != null && correctSound != null) sfxSource.PlayOneShot(correctSound);
        }
        else
        {
            trueFalseText.text = "Ce n'est pas tout à fait ça, mais bien essayé !";
            trueFalseText.color = Color.red;
            SetCatFace("wrong");
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
        if (score > 0)
        {
            catSpeech.text = "Bravo ! Grâce à toi, Kim est prêt pour sa journée !";
        }
        else
        {
            catSpeech.text = "Oups ! Kim n'a pas tout ce qu'il lui faut. Veux-tu réessayer ?";
        }
        SetCatFace("happy");
        trueFalseText.text = "";
        nextButton.SetActive(false);
        restartButton.SetActive(true);
    }

    public void RestartGame() { ShowMainMenu(); }
    public void QuitGame()
    {
        Debug.Log("Quit Requested");
        Application.Quit();
    }
}