using UnityEngine;

// Erzeugt ein neues Menüelement im Unity Editor für das Anlegen eines Wizard-Charakters
[CreateAssetMenu(menuName = "Wizard/Character")]
public class Wizard : ScriptableObject
{
    // Aktuelle Lebenspunkte des Zauberers
    public float health = 100f;
    // Aktuelle Manapunkte des Zauberers
    public float mana = 100f;
    // Maximale Lebenspunkte
    public float maxHealth = 100f;
    // Maximale Manapunkte
    public float maxMana = 100f;
    // Schaden, den der Zauberer verursacht
    public int damage = 10;
    // Angriffsgeschwindigkeit des Zauberers
    public float attackSpeed = 1f;

    // Aktuelles Level des Zauberers
    public int level = 1;
    // Aktuelle Erfahrungspunkte
    public int experience = 0;
    // Verfügbare Attributpunkte zum Verteilen
    public int statPoints = 0;

    // Berechnet die benötigten Erfahrungspunkte für das angegebene Level
    public int GetXpForLevel(int level)
    {
        return Mathf.RoundToInt(100 * Mathf.Pow(level, 1.5f));
    }

    // Fügt Erfahrungspunkte hinzu und prüft, ob ein Levelaufstieg erfolgt
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

    // Erhöht das Level und gibt Attributpunkte
    public void LevelUP()
    {
        level++;
        statPoints += 5;
    }

    // Erhöht maximale und aktuelle Lebenspunkte, verbraucht einen Attributpunkt
    public void AddHealth()
    {
        maxHealth += 10f;
        health += 10f;
        statPoints--;
    }

    // Erhöht maximale und aktuelle Manapunkte, verbraucht einen Attributpunkt
    public void AddMana()
    {
        maxMana += 10f;
        mana += 10f;
        statPoints--;
    }

    // Erhöht den Schaden, verbraucht einen Attributpunkt
    public void AddDamage()
    {
        damage += 5;
        statPoints--;
    }

    // Erhöht die Angriffsgeschwindigkeit bis zu einem Maximum, verbraucht einen Attributpunkt
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
