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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (collision.gameObject.CompareTag("Target"))
        {
            float timeTaken = Time.time - spawnTime;
            int addXP = CalculateXP(timeTaken);

            player.GetComponent<Player>().score++;
            player.GetComponent<Player>().wizard.XP(addXP);

            Vector3 spawnPosition;
            do
            {
                spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
            } while ((spawnPosition.x == player.transform.position.x || spawnPosition.y == player.transform.position.y)
                && (spawnPosition.x == collision.transform.position.x || spawnPosition.y == collision.transform.position.y));

            Destroy(collision.gameObject);
            Destroy(gameObject);

            Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
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
