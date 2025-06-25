using UnityEngine;

public class BallMovment : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    public float speed = 10f;
    [HideInInspector]
    public Vector2 direction = Vector2.right;
    public GameObject targetPrefab;
    private GameObject player;
    private float spawnTime;
    private GameObject enemy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        rbody2D = GetComponent<Rigidbody2D>();
        rbody2D.velocity = direction.normalized * speed;
        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        spawnTime = Time.time;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Überprüft, ob das getroffene Objekt ein Target ist
        if (collision.gameObject.CompareTag("Target"))
        {
            float timeTaken = Time.time - spawnTime; // Zeit seit dem Spawn des Balls
            int addXP = CalculateXP(timeTaken); // XP basierend auf der benötigten Zeit berechnen

            player.GetComponent<Player>().score++; // Spielerpunktzahl erhöhen
            player.GetComponent<Player>().wizard.XP(addXP); // XP dem Spieler hinzufügen

            Vector3 spawnPosition;
            // Neue zufällige Position für das nächste Target finden, die nicht auf Spieler- oder aktuelle Target-Position liegt
            do
            {
                spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
            } while ((spawnPosition.x == player.transform.position.x || spawnPosition.y == player.transform.position.y)
                && (spawnPosition.x == collision.transform.position.x || spawnPosition.y == collision.transform.position.y));

            Destroy(collision.gameObject); // Das getroffene Target zerstören
            Destroy(gameObject); // Den Ball zerstören

            Instantiate(targetPrefab, spawnPosition, Quaternion.identity); // Neues Target erzeugen
        }
        // Überprüft, ob das getroffene Objekt ein Enemy ist
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            player.GetComponent<Player>().wizard.XP(50); // Spieler erhält 50 XP
            Destroy(gameObject); // Ball zerstören
            Destroy(collision.gameObject); // Enemy zerstören
            GameObject.Find("Spawner").GetComponent<Spawner>().count--; // Enemy-Zähler im Spawner verringern
        }
        // Überprüft, ob das getroffene Objekt der Spieler ist und dieser noch keinen Schaden bekommen hat
        else if (collision.gameObject.CompareTag("Player") && !player.GetComponent<Player>().gotDamage)
        {
            player.GetComponent<Player>().wizard.health -= enemy.GetComponent<Enemy>().damage; // Spieler erhält Schaden
            player.GetComponent<Player>().gotDamage = true; // Spieler hat Schaden erhalten, um Mehrfachschaden zu verhindern
            Destroy(gameObject); // Ball zerstören
        }
    }

    private int CalculateXP(float timeTaken)
    {
        // Je schneller, desto mehr XP (maximal 150, minimal 25)
        if (timeTaken < 1f)
        {
            return 150;
        }
        if (timeTaken > 3f)
        {
            return 25;
        }
        // Linearer Abfall zwischen 1 und 3 Sekunden
        return Mathf.RoundToInt(150 - ((timeTaken - 1f) / 2f) * (150 - 25));
    }
}
