using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public GameObject UI;
    public GameObject HP;
    public GameObject Mana;
    public GameObject AttackSpeed;
    public GameObject Damage;
    public GameObject StatsPoints;
    private Wizard wizard;

    private void Start()
    {
        wizard = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().wizard;
        UI.SetActive(false);
    }

    private void Update()
    {
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
