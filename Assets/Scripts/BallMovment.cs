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
        // �berpr�ft, ob das getroffene Objekt ein Target ist
        if (collision.gameObject.CompareTag("Target"))
        {
            float timeTaken = Time.time - spawnTime; // Zeit seit dem Spawn des Balls
            int addXP = CalculateXP(timeTaken); // XP basierend auf der ben�tigten Zeit berechnen

            player.GetComponent<Player>().score++; // Spielerpunktzahl erh�hen
            player.GetComponent<Player>().wizard.XP(addXP); // XP dem Spieler hinzuf�gen

            Vector3 spawnPosition;
            // Neue zuf�llige Position f�r das n�chste Target finden, die nicht auf Spieler- oder aktuelle Target-Position liegt
            do
            {
                spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
            } while ((spawnPosition.x == player.transform.position.x || spawnPosition.y == player.transform.position.y)
                && (spawnPosition.x == collision.transform.position.x || spawnPosition.y == collision.transform.position.y));

            Destroy(collision.gameObject); // Das getroffene Target zerst�ren
            Destroy(gameObject); // Den Ball zerst�ren

            Instantiate(targetPrefab, spawnPosition, Quaternion.identity); // Neues Target erzeugen
        }
        // �berpr�ft, ob das getroffene Objekt ein Enemy ist
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            player.GetComponent<Player>().wizard.XP(50); // Spieler erh�lt 50 XP
            Destroy(gameObject); // Ball zerst�ren
            Destroy(collision.gameObject); // Enemy zerst�ren
            GameObject.Find("Spawner").GetComponent<Spawner>().count--; // Enemy-Z�hler im Spawner verringern
        }
        // �berpr�ft, ob das getroffene Objekt der Spieler ist und dieser noch keinen Schaden bekommen hat
        else if (collision.gameObject.CompareTag("Player") && !player.GetComponent<Player>().gotDamage)
        {
            player.GetComponent<Player>().wizard.health -= enemy.GetComponent<Enemy>().damage; // Spieler erh�lt Schaden
            player.GetComponent<Player>().gotDamage = true; // Spieler hat Schaden erhalten, um Mehrfachschaden zu verhindern
            Destroy(gameObject); // Ball zerst�ren
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
