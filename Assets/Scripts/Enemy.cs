using UnityEngine;

/// <summary>
/// Steuert das Verhalten eines Gegners, inklusive Patrouillieren, Angreifen und Zustandswechsel.
/// </summary>
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rbody2D; // Rigidbody2D-Komponente für Bewegungen
    private GameObject player; // Referenz auf das Spielerobjekt
    public float damage = 10f; // Schaden, den der Gegner verursacht
    public GameObject fireballPrefab; // Prefab für den Feuerball-Angriff
    private Player playerScript; // Referenz auf das Player-Skript
    private float attackSpeed = 0; // Cooldown für Angriffe
    public bool patrol = false; // Gibt an, ob Patrouille aktiviert ist
    private bool isMoving = false; // Gibt an, ob der Gegner sich gerade bewegt
    private bool patroled = false; // Gibt an, ob die Patrouille abgeschlossen ist
    public GameObject patrolTarget; // Zielobjekt für die Patrouille
    private Vector3 targetPosition; // Zielposition für die Bewegung
    public EnemyState enemyState; // Aktueller Zustand des Gegners

    // State-Management
    private float stateTimer = 0f; // Timer für Zustandswechsel
    private float idleDuration = 0f; // Zufällige Dauer im Idle-Zustand

    /// <summary>
    /// Initialisiert Referenzen und setzt den Startzustand.
    /// </summary>
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        rbody2D = GetComponent<Rigidbody2D>();
        enemyState = EnemyState.Idle;
        SetIdleDuration();
    }

    /// <summary>
    /// Haupt-Update-Methode, steuert das Zustandsverhalten des Gegners.
    /// </summary>
    void Update()
    {
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);

        switch (enemyState)
        {
            case EnemyState.Idle:
                stateTimer += Time.deltaTime;
                if (stateTimer >= idleDuration)
                {
                    // Entscheide nächsten State basierend auf Spieler-Distanz
                    if (playerDistance < 5f)
                    {
                        enemyState = EnemyState.Attacking;
                    }
                    else
                    {
                        enemyState = EnemyState.Patrolling;
                    }
                    stateTimer = 0f;
                }
                break;

            case EnemyState.Patrolling:
                Patrol();
                // Wenn Spieler nah genug ist, wechsle zu Attack
                if (playerDistance < 5f)
                {
                    enemyState = EnemyState.Idle;
                    SetIdleDuration();
                    stateTimer = 0f;
                }
                // Nach Patrouille gehe zu Idle
                else if (patroled)
                {
                    patroled = false;
                    enemyState = EnemyState.Idle;
                    SetIdleDuration();
                    stateTimer = 0f;
                }
                break;

            case EnemyState.Attacking:
                RangAttack();
                // Wenn Spieler zu weit weg ist, gehe zu Idle
                if (playerDistance >= 5f)
                {
                    enemyState = EnemyState.Idle;
                    SetIdleDuration();
                    stateTimer = 0f;
                }
                break;
        }
    }

    /// <summary>
    /// Setzt eine zufällige Idle-Dauer.
    /// </summary>
    void SetIdleDuration()
    {
        idleDuration = Random.Range(2f, 3f);
    }

    /// <summary>
    /// Führt die Patrouillenbewegung aus.
    /// </summary>
    void Patrol()
    {
        if (!isMoving)
        {
            // Start moving towards the patrol target
            isMoving = true;
            targetPosition = patrolTarget.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
        }
        else if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Reached patrol target, stop moving
            isMoving = false;
            patroled = true;
        }
        if (isMoving)
        {
            MoveTowards(targetPosition);
        }
    }

    /// <summary>
    /// Führt den Fernangriff auf den Spieler aus.
    /// </summary>
    void RangAttack()
    {
        Vector2 distance = player.transform.position - transform.position;
        if (distance.magnitude < 1f && !playerScript.gotDamage)
        {
            // Wenn der Spieler zu nah ist, stoppe die Bewegung
            rbody2D.velocity = Vector2.zero;
            return;
        }
        MoveTowards(player.transform.position);
        attackSpeed -= Time.deltaTime; // Angriffscooldown verringern
        if (Vector2.Distance(transform.position, player.transform.position) > 1f && attackSpeed <= 0)
        {
            RAttack();
            attackSpeed = Random.Range(1f, 5f); // Angriffscooldown zurücksetzen
        }
    }

    /// <summary>
    /// Bewegt den Gegner auf ein Ziel zu.
    /// </summary>
    /// <param name="target">Zielposition</param>
    void MoveTowards(Vector3 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, 2 * Time.deltaTime);
    }

    /// <summary>
    /// Kollisionsbehandlung mit dem Spieler.
    /// </summary>
    /// <param name="collision">Kollisionsdaten</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !playerScript.gotDamage)
        {
            playerScript.wizard.health -= damage;
            playerScript.gotDamage = true;
            Destroy(gameObject);
            GameObject.Find("Spawner").GetComponent<Spawner>().count--;
        }
    }

    /// <summary>
    /// Instanziiert einen Feuerball und richtet ihn auf den Spieler aus.
    /// </summary>
    private void RAttack()
    {
        // Fireball instanziieren
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        // Richtung zum Spieler berechnen
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Fireball-Spawn-Position leicht vor dem Gegner
        Vector3 spawnPosition = (direction * 2f);
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

/// <summary>
/// Mögliche Zustände des Gegners.
/// </summary>
public enum EnemyState
{
    Idle,
    Patrolling,
    Attacking
}
