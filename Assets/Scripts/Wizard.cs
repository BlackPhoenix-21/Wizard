using UnityEngine;

[CreateAssetMenu(menuName = "Wizard/Character")]
public class Wizard : ScriptableObject
{
    public float health = 100f;
    public float mana = 100f;
    public float maxHealth = 100f;
    public float maxMana = 100f;
    public int damage = 10;
    public float attackSpeed = 1f;

    public int level = 1;
    public int experience = 0;
    public int statPoints = 0;

    public int GetXpForLevel(int level)
    {
        return Mathf.RoundToInt(100 * Mathf.Pow(level, 1.5f));
    }

    public void XP(int newXP)
    {
        experience += newXP;
        do
        {
            if (experience >= GetXpForLevel(level))
            {
                experience -= GetXpForLevel(level);
                LevelUP();
            }
        } while (experience >= GetXpForLevel(level));
    }

    public void LevelUP()
    {
        level++;
        statPoints += 5;
    }

    public void AddHealth()
    {
        maxHealth += 10f;
        health += 10f;
        statPoints--;
    }

    public void AddMana()
    {
        maxMana += 10f;
        mana += 10f;
        statPoints--;
    }

    public void AddDamage()
    {
        damage += 5;
        statPoints--;
    }

    public void AddAttackSpeed()
    {
        if (attackSpeed < 2f)
        {
            attackSpeed += 0.1f;
            statPoints--;
            if (attackSpeed > 2f)
            {
                attackSpeed = 2f;
            }
        }
        else
        {
            attackSpeed = 2f;
        }
    }
}
