using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovment : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    public float speed = 10f;
    [HideInInspector]
    public Vector2 direction = Vector2.right;

    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Collider());
        rbody2D.velocity = direction * speed;
        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    IEnumerator Collider()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<Collider2D>().enabled = true;
    }
}
