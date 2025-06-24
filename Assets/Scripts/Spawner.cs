using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Zeitintervall bis zum nächsten Spawn
    public float time = 5f;

    // Array der möglichen Prefabs, die gespawnt werden können
    public GameObject[] prefabToSpawn;

    // Gewichtung der Gegner: niedrigere Indizes sind wahrscheinlicher
    public int count = 1;

    // Referenz auf das Spieler-GameObject
    private GameObject player;

    void Start()
    {
        // Spieler-Objekt anhand des Tags "Player" finden
        player = GameObject.FindGameObjectWithTag("Player");
        count = 1; // Startzähler auf 1 setzen
    }

    void Update()
    {
        // Zeit bis zum nächsten Spawn herunterzählen
        time -= Time.deltaTime;

        // Wenn die Zeit abgelaufen ist, neuen Gegner spawnen
        if (time <= 0f && count <= 10)
        {
            // Spawn-Intervall abhängig vom Level des Zauberers anpassen
            time = 10f / player.GetComponent<Player>().wizard.level;
            SpawnEnemy();
        }
    }

    // Spawnt einen zufälligen Gegner basierend auf Gewichtung und erhöht den Zähler
    void SpawnEnemy()
    {
        int index = GetWeightedRandomIndex(prefabToSpawn.Length);
        Instantiate(prefabToSpawn[index], transform.position, Quaternion.identity);
        count++;
    }

    // Gibt einen zufälligen Index zurück, wobei niedrigere Indizes wahrscheinlicher sind
    int GetWeightedRandomIndex(int length)
    {
        // Gewichtung: 1, 1/2, 1/3, ..., 1/n
        float totalWeight = 0f;
        float[] weights = new float[length];
        for (int i = 0; i < length; i++)
        {
            weights[i] = 1f / (i + 1);
            totalWeight += weights[i];
        }

        // Zufallswert im Bereich der Gesamtgewichtung
        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;
        for (int i = 0; i < length; i++)
        {
            cumulative += weights[i];
            if (randomValue <= cumulative)
                return i;
        }
        return length - 1; // Fallback, falls kein Index gefunden wurde
    }
}
