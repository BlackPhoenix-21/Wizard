using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsUI : MonoBehaviour
{
    public GameObject UI;
    public GameObject HP;
    public GameObject Mana;
    public GameObject AttackSpeed;
    public GameObject Damage;
    public GameObject StatsPoints;
    private Wizard wizard;
    public GameObject pauseMenu;

    private void Start()
    {
        wizard = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().wizard;
        UI.SetActive(false);
    }

    // Wird einmal pro Frame aufgerufen
    private void Update()
    {
        // Überprüft, ob die Escape-Taste gedrückt wurde und das Spiel entweder im Spiel- oder Pausenmodus ist
        if (Input.GetKeyDown(KeyCode.Escape) && (GameManager.instance.state == GameManager.GameState.Playing ||
            GameManager.instance.state == GameManager.GameState.Paused))
        {
            // Wenn das Pausenmenü aktiv ist, wird es geschlossen und das Spiel fortgesetzt
            if (pauseMenu.activeSelf)
            {
                PauseMenu(1); // Pausenmenü schließen
                GameManager.instance.state = GameManager.GameState.Playing;
            }
            // Andernfalls wird das Pausenmenü geöffnet und das Spiel pausiert
            else
            {
                PauseMenu(0); // Pausenmenü öffnen
                GameManager.instance.state = GameManager.GameState.Paused;
            }
        }
        // Überprüft, ob die Tab-Taste gedrückt wurde, um das Stats-UI ein- oder auszublenden
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UI.SetActive(!UI.activeSelf);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UIActive = UI.activeSelf;
        }
        // Wenn das Stats-UI aktiv ist, werden die aktuellen Werte angezeigt
        if (UI.activeSelf)
        {
            HP.GetComponent<TMP_Text>().text = "HP: " + (int)wizard.health + "/" + wizard.maxHealth;
            Mana.GetComponent<TMP_Text>().text = "Mana: " + (int)wizard.mana + "/" + wizard.maxMana;
            AttackSpeed.GetComponent<TMP_Text>().text = "Attack Speed: " + Mathf.Floor((2.1f - wizard.attackSpeed) * 10) / 10;
            Damage.GetComponent<TMP_Text>().text = "Damage: " + wizard.damage;
            StatsPoints.GetComponent<TMP_Text>().text = "Stat Points: " + wizard.statPoints;
        }
    }

    public void PauseMenu(int index)
    {
        switch (index)
        {
            case 0:
                Time.timeScale = 0f; // Pause game time
                pauseMenu.SetActive(true);
                break;
            case 1:
                Time.timeScale = 1f; // Resume game time
                pauseMenu.SetActive(false);
                break;
            case 2:
                Time.timeScale = 1f; // Resume game time
                GameManager.instance.state = GameManager.GameState.MainMenu;
                SceneManager.LoadScene("Title", LoadSceneMode.Single);
                break;
        }
    }

    public void AddHealth()
    {
        if (wizard.statPoints > 0)
        {
            wizard.AddHealth();
        }
    }

    public void AddMana()
    {
        if (wizard.statPoints > 0)
        {
            wizard.AddMana();
        }
    }

    public void AddDamage()
    {
        if (wizard.statPoints > 0)
        {
            wizard.AddDamage();
        }
    }

    public void AddAttackSpeed()
    {
        if (wizard.statPoints > 0)
        {
            wizard.AddAttackSpeed();
        }
    }
}
