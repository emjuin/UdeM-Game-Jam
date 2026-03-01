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
    public TextMeshProUGUI catSpeech; // Glisse 'CatSpeech_Text' ici
    public TextMeshProUGUI trueFalseText; // Glisse 'True_False' ici

    [Header("Stars & Buttons")]
    public Image[] starImages;
    public Sprite goldStarSprite;
    public Sprite blackStarSprite;
    public TextMeshProUGUI[] buttonTexts; // Les 3 textes de tes boutons Option 1, 2, 3
    public GameObject nextButton;
    public GameObject restartButton;

    // --- TES SCÉNARIOS FIXES ---
    private List<LevelData> summerDay = new List<LevelData>() {
        new LevelData { catPrompt = "Le soleil est très fort aujourd'hui. Comment protéger la tête de Kim ?", option1 = "Tuque", option2 = "Parapluie", option3 = "Casquette", correctItem = "Casquette", utilityText = "La casquette protège la tête du soleil !" },
        new LevelData { catPrompt = "Kim passe la journée à la plage. Qu’est-ce que Kim devra porter ?", option1 = "Maillot de bain", option2 = "Patins", option3 = "Imperméable", correctItem = "Maillot de bain", utilityText = "Le maillot est parfait pour la baignade !" },
        new LevelData { catPrompt = "Pour se rendre à la plage, quelles chaussures choisir ?", option1 = "Bottes de pluie", option2 = "Bottes de neige", option3 = "Sandales", correctItem = "Sandales", utilityText = "Les sandales sont idéales pour le sable chaud." },
        new LevelData { catPrompt = "Il fait très chaud ! Qu’est-ce que Kim peut acheter pour se rafraîchir ?", option1 = "Chocolat chaud", option2 = "Sac de chips", option3 = "Crème glacée", correctItem = "Crème glacée", utilityText = "Miam ! La crème glacée rafraîchit tout le monde." },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour passer la journée au soleil ?", option1 = "Collation", option2 = "Crème solaire", option3 = "Salopette de neige", correctItem = "Crème solaire", utilityText = "La crème solaire protège la peau des brûlures." }
    };

    private List<LevelData> winterDay = new List<LevelData>() {
        new LevelData { catPrompt = "Les rues sont enneigées. Comment protéger la tête de Kim du froid ?", option1 = "Tuque", option2 = "Parapluie", option3 = "Casquette", correctItem = "Tuque", utilityText = "La tuque garde les oreilles bien au chaud !" },
        new LevelData { catPrompt = "Les lacs sont gelés ! Qu’est-ce que Kim porte pour patiner ?", option1 = "Maillot de bain", option2 = "Patins", option3 = "Imperméable", correctItem = "Patins", utilityText = "Avec les patins, Kim peut glisser sur la glace." },
        new LevelData { catPrompt = "Pour se rendre à la patinoire, quelles chaussures porter ?", option1 = "Bottes de pluie", option2 = "Bottes de neige", option3 = "Sandales", correctItem = "Bottes de neige", utilityText = "Les bottes de neige gardent les pieds au sec." },
        new LevelData { catPrompt = "Il fait très froid. Qu’est-ce que Kim peut acheter pour se réchauffer ?", option1 = "Chocolat chaud", option2 = "Sac de chips", option3 = "Crème glacée", correctItem = "Chocolat chaud", utilityText = "Un bon chocolat chaud, ça réchauffe le cœur !" },
        new LevelData { catPrompt = "Qu’est-ce qui est essentiel pour rester dehors toute la journée ?", option1 = "Collation", option2 = "Crème solaire", option3 = "Salopette de neige", correctItem = "Salopette de neige", utilityText = "La salopette empêche d'avoir froid dans la neige." }
    };

    private List<LevelData> activeRounds = new List<LevelData>();
    private int currentRoundIndex = 0;
    private int score = 0;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        menuPanel.SetActive(true);