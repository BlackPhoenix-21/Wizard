using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public static GameManager instance;
    public GameState state;
    private float timer;
    public int activeWizardIndex = 0;
    public Wizard[] wizard;
    public Wizard[] restWizard;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            state = GameState.MainMenu;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        RestWizard();
    }

    void Update()
    {
        if (state == GameState.Playing)
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                timer = 0;
                SceneManager.LoadScene("Title", LoadSceneMode.Single);
                state = GameState.MainMenu;
            }
        }
    }

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