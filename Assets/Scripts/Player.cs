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
    private int face = 2; // 1 = up, -1 = down, 2 = right, -2 = left
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
        if (GetComponent<SpriteRenderer>().flipX)
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.left;
        }
        else
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.right;
        }
        if (face == 1)
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.up;
            fireball.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (face == -1)
        {
            fireball.GetComponent<BallMovment>().direction = Vector2.down;
            fireball.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        Destroy(fireball, 5f);
    }

    private void Movment(float x, float y)
    {
        if (Input.GetKey(KeyCode.W))
        {
            y += 1;
            animator.SetBool("IsMoving", true);
            face = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            y -= 1;
            animator.SetBool("IsMoving", true);
            face = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            x -= 1;
            GetComponent<SpriteRenderer>().flipX = true;
            animator.SetBool("IsMoving", true);
            face = -2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            x += 1;
            GetComponent<SpriteRenderer>().flipX = false;
            animator.SetBool("IsMoving", true);
            face = 2;
        }
        if (x != 0 && y != 0)
        {
            x *= 0.7f;
            y *= 0.7f;
        }
        transform.position += 4 * Time.deltaTime * (Vector3.up * y + Vector3.right * x);
    }
}
