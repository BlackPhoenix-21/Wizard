using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovment : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    public float speed = 10f;
    [HideInInspector]
    public Vector2 direction = Vector2.right;
    public GameObject targetPrefab;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rbody2D = GetComponent<Rigidbody2D>();
        rbody2D.velocity = direction.normalized * speed;
        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            player.GetComponent<Player>().score++;
            Vector3 spawnPosition;
            do
            {
                spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
            } while (spawnPosition.x == player.transform.position.x || spawnPosition.y == player.transform.position.y
                && spawnPosition.x == collision.transform.position.x || spawnPosition.y == collision.transform.position.y);

            Destroy(collision.gameObject);
            Destroy(gameObject);

            Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
