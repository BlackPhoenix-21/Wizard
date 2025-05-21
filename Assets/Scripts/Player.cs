using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    private Animator animator;
    public GameObject fireballPrefab;
    private bool isAttacking = false;
    public int score = 0;
    public GameObject healthbar;
    public GameObject manabar;
    public GameObject scoreText;
    public GameObject levelbar;
    public Wizard wizard;
    public Wizard restWizard;
    public bool UIActive = false;

    void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (UIActive)
        {
            return;
        }

        float x = 0, y = 0;
        animator.SetBool("IsMoving", false);
        Movment(x, y);
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            if (wizard.mana < 10)
            {
                Debug.Log("Not enough mana!");
                return;
            }
            animator.SetFloat("AttackSpeed", wizard.attackSpeed);
            animator.SetTrigger("Attack");
            wizard.mana -= 10;
            isAttacking = true;
        }

        healthbar.GetComponent<Image>().fillAmount = wizard.health / wizard.maxHealth;
        healthbar.GetComponentInChildren<TMP_Text>().text = ((int)wizard.health).ToString() + "/" + ((int)wizard.maxHealth).ToString();
        manabar.GetComponent<Image>().fillAmount = wizard.mana / wizard.maxMana;
        manabar.GetComponentInChildren<TMP_Text>().text = ((int)wizard.mana).ToString() + "/" + ((int)wizard.maxMana).ToString();

        levelbar.GetComponent<Image>().fillAmount = (float)wizard.experience / wizard.GetXpForLevel(wizard.level);
        levelbar.GetComponentInChildren<TMP_Text>().text = wizard.level.ToString() + " (" + wizard.experience.ToString() + "/" + wizard.GetXpForLevel(wizard.level).ToString() + ")";

        scoreText.GetComponent<TMP_Text>().text = "Score: " + score.ToString();

        if (wizard.mana < wizard.maxMana)
        {
            wizard.mana += Time.deltaTime * 5;
        }
    }

    public void RestWizard()
    {
        wizard.health = restWizard.health;
        wizard.mana = restWizard.mana;
        wizard.maxHealth = restWizard.maxHealth;
        wizard.maxMana = restWizard.maxMana;
        wizard.damage = restWizard.damage;
        wizard.attackSpeed = restWizard.attackSpeed;
        wizard.level = restWizard.level;
        wizard.experience = restWizard.experience;
        wizard.statPoints = restWizard.statPoints;
    }

    public void ResteAttack()
    {
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
        Vector2 moveDirection = new Vector2(x, y).normalized;
        rbody2D.velocity = moveDirection * 4f;
    }
}
