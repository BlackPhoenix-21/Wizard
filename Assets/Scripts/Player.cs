using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    private Animator animator;
    public GameObject fireballPrefab;
    private bool isAttacking = false;
    private float health = 100f;
    private float maxHealth = 100f;
    private float mana = 100f;
    private float maxMana = 100f;
    public int score = 0;
    public GameObject healthbar;
    public GameObject manabar;
    public GameObject scoreText;

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
            if (mana < 10)
            {
                Debug.Log("Not enough mana!");
                return;
            }
            animator.SetTrigger("Attack");
            Attack();
            mana -= 10;
            isAttacking = true;
            StartCoroutine(ResetAttack());
        }

        healthbar.GetComponent<Image>().fillAmount = health / maxHealth;
        healthbar.GetComponentInChildren<TMP_Text>().text = ((int)health).ToString() + "/" + ((int)maxHealth).ToString();
        manabar.GetComponent<Image>().fillAmount = mana / maxMana;
        manabar.GetComponentInChildren<TMP_Text>().text = ((int)mana).ToString() + "/" + ((int)maxMana).ToString();

        scoreText.GetComponent<TMP_Text>().text = "Score: " + score.ToString();

        if (mana < maxMana)
        {
            mana += Time.deltaTime * 5;
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
        Vector2 direction = Vector2.zero;
        Vector3 positionOffset = Vector3.zero;
        float rotationZ = 0;

        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector2.left;
            positionOffset = new Vector3(-0.5f, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector2.up;
                rotationZ = -45;
                positionOffset += new Vector3(0, 0.5f, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction += Vector2.down;
                rotationZ = 45;
                positionOffset += new Vector3(0, -0.5f, 0);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.right;
            positionOffset = new Vector3(0.5f, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector2.up;
                rotationZ = 45;
                positionOffset += new Vector3(0, 0.5f, 0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction += Vector2.down;
                rotationZ = -45;
                positionOffset += new Vector3(0, -0.5f, 0);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
            rotationZ = 90;
            positionOffset = new Vector3(0, 0.5f, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
            rotationZ = -90;
            positionOffset = new Vector3(0, -0.5f, 0);
        }
        else
        {
            if (GetComponent<SpriteRenderer>().flipX)
            {
                direction = Vector2.left;
                positionOffset = new Vector3(-0.5f, 0, 0);
            }
            else
            {
                direction = Vector2.right;
                positionOffset = new Vector3(0.5f, 0, 0);
            }
        }

        fireball.GetComponent<BallMovment>().direction = direction;
        fireball.transform.position += positionOffset;
        fireball.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

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
