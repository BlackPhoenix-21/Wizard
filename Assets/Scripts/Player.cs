using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    private Animator animator;
    public GameObject fireballPrefab;
    private bool isAttacking = false;
    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = 0, y = 0;
        animator.SetBool("IsMoving", false);
        Movment(x, y);
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            animator.SetTrigger("Attack");
            Attack();
            isAttacking = true;
            StartCoroutine(ResetAttack());
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    private void Attack()
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        if (Input.GetKey(KeyCode.A))
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.left;
            fireball.transform.position += new Vector3(-0.5f, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                fireball.GetComponent<BallMovment>().direction = Vector2.up + Vector2.left;
                fireball.transform.rotation = Quaternion.Euler(0, 0, -45);
                fireball.transform.position += new Vector3(0, 0.5f, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                fireball.GetComponent<BallMovment>().direction = Vector2.down + Vector2.left;
                fireball.transform.rotation = Quaternion.Euler(0, 0, 45);
                fireball.transform.position += new Vector3(0, -0.5f, 0);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.right;
            fireball.transform.position += new Vector3(0.5f, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                fireball.GetComponent<BallMovment>().direction = Vector2.up + Vector2.right;
                fireball.transform.rotation = Quaternion.Euler(0, 0, 45);
                fireball.transform.position += new Vector3(0, 0.5f, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                fireball.GetComponent<BallMovment>().direction = Vector2.down + Vector2.right;
                fireball.transform.rotation = Quaternion.Euler(0, 0, -45);
                fireball.transform.position += new Vector3(0, -0.5f, 0);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.up;
            fireball.transform.rotation = Quaternion.Euler(0, 0, 90);
            fireball.transform.position += new Vector3(0, 0.5f, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.down;
            fireball.transform.rotation = Quaternion.Euler(0, 0, -90);
            fireball.transform.position += new Vector3(0, -0.5f, 0);
        }
        else if (GetComponent<SpriteRenderer>().flipX)
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.left;
            fireball.transform.position += new Vector3(-0.5f, 0, 0);
        }
        else if (!GetComponent<SpriteRenderer>().flipX)
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.right;
            fireball.transform.position += new Vector3(0.5f, 0, 0);
        }

        Destroy(fireball, 5f);
    }

    private void Movment(float x, float y)
    {
        if (Input.GetKey(KeyCode.W))
        {
            y += 1;
            animator.SetBool("IsMoving", true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            y -= 1;
            animator.SetBool("IsMoving", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            x -= 1;
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("IsMoving", true);
        }
        if (Input.GetKey(KeyCode.D))
        {
            x += 1;
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool("IsMoving", true);
        }
        if (x != 0 && y != 0)
        {
            x *= 0.7f;
            y *= 0.7f;
        }
        transform.position += 4 * Time.deltaTime * (Vector3.up * y + Vector3.right * x);
    }
}
