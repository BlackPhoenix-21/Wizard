using UnityEngine;
using UnityEngine.SceneManagement;

// Der GameManager steuert den Spielzustand und verwaltet die Zauberer-Instanzen
public class GameManager : MonoBehaviour
{
    // M�gliche Spielzust�nde
    public enum GameState
    {
        MainMenu,   // Hauptmen�
        Playing,    // Spiel l�uft
        Paused,     // Spiel pausiert
        GameOver    // Spiel vorbei
    }

    public static GameManager instance; // Singleton-Instanz des GameManagers
    public GameState state;             // Aktueller Spielzustand
    private float timer;                // Timer f�r Zustandswechsel
    public int activeWizardIndex = 0;   // Index des aktiven Zauberers
    public Wizard[] wizard;             // Array der aktiven Zauberer-Instanzen
    public Wizard[] restWizard;         // Array der gespeicherten Zauberer-Referenzen

    // Wird beim Starten des GameObjects aufgerufen
    void Start()
    {
        // Singleton-Pattern: Nur eine Instanz des GameManagers zulassen
        if (instance == null)
        {
            instance = this;
            state = GameState.MainMenu;
            DontDestroyOnLoad(gameObject); // GameManager bleibt beim Szenenwechsel erhalten
        }
        else
        {
            Destroy(gameObject); // Doppelte Instanzen werden zerst�rt
        }
        RestWizard(); // Setzt die Werte des aktiven Zauberers zur�ck
    }

    // Wird einmal pro Frame aufgerufen
    void Update()
    {
        // Wenn das Spiel l�uft, erh�he den Timer
        if (state == GameState.Playing)
        {
            timer += Time.deltaTime;
            // Nach 5 Sekunden: Zur�ck zum Hauptmen�
            if (timer >= 5)
            {
                timer = 0;
                SceneManager.LoadScene("Title", LoadSceneMode.Single);
                state = GameState.MainMenu;
            }
        }
    }

    // Setzt die Werte des aktiven Zauberers auf die gespeicherten Werte zur�ck
    public void RestWizard()
    {
        wizard[activeWizardIndex].health = restWizard[activeWizardIndex].health;
        wizard[activeWizardIndex].mana = restWizard[activeWizardIndex].mana;
        wizard[activeWizardIndex].maxHealth = restWizard[activeWizardIndex].maxHealth;
        wizard[activeWizardIndex].maxMana = restWizard[activeWizardIndex].maxMana;
        wizard[activeWizardIndex].damage = restWizard[activeWizardIndex].damage;
        wizard[activeWizardIndex].attackSpeed = restWizard[activeWizardIndex].attackSpeed;
        wizard[activeWizardIndex].level = restWizard[activeWizardIndex].level;
        wizard[activeWizardIndex].experience = restWizard[activeWizardIndex].experience;
        wizard[activeWizardIndex].statPoints = restWizard[activeWizardIndex].statPoints;
    }
}