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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (GameManager.instance.state == GameManager.GameState.Playing ||
            GameManager.instance.state == GameManager.GameState.Paused))
        {
            if (pauseMenu.activeSelf)
            {
                PauseMenu(1); // Close pause menu
                GameManager.instance.state = GameManager.GameState.Playing;

            }
            else
            {
                PauseMenu(0); // Open pause menu
                GameManager.instance.state = GameManager.GameState.Paused;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UI.SetActive(!UI.activeSelf);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UIActive = UI.activeSelf;
        }
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
