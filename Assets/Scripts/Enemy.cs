using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    private GameObject player;
    public float damage = 10f;
    public GameObject fireballPrefab;
    public float attackSpeed = 2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 2 * Time.deltaTime);
        attackSpeed -= Time.deltaTime; // Decrease attack speed cooldown
        if (Vector2.Distance(transform.position, player.transform.position) > 1f && attackSpeed <= 0)
        {
            Attack();
            attackSpeed = 2f; // Reset attack speed cooldown
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Player>().wizard.health -= damage;
            Destroy(gameObject);
            GameObject.Find("Spawner").GetComponent<Spawner>().count--;
        }
    }

    private void Attack()
    {
        // Fireball instanziieren
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        // Richtung zum Spieler berechnen
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Fireball-Spawn-Position leicht vor dem Gegner
        Vector3 spawnPosition = (direction * 2f);
        Debug.Log("Spawn Position: " + spawnPosition);
        // Fireball-Rotation in Flugrichtung setzen
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0, 0, rotationZ - 180f);

        // Richtung an BallMovment-Komponente übergeben
        BallMovment ballMovment = fireball.GetComponent<BallMovment>();
        ballMovment.direction = direction;
        fireball.transform.position += spawnPosition;

        Destroy(fireball, 5f);
    }
}
