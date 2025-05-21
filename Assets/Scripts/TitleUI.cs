using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    private GameObject player;
    private float moveSpeed = 2f;
    private float minX = -4f;
    private float maxX = 4f;
    private int moveDirection = 1;
    private float pauseTimer = 0f;
    private bool isPaused = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Player>().UIActive = true;
    }

    private void Update()
    {
        if (isPaused)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0f)
            {
                isPaused = false;
                moveDirection *= -1;
            }
            player.GetComponent<Animator>().SetBool("IsMoving", false);
            return;
        }

        Vector3 pos = player.transform.position;
        pos.x += moveSpeed * moveDirection * Time.deltaTime;
        player.transform.position = pos;


        player.GetComponent<Animator>().SetBool("IsMoving", true);
        player.GetComponent<SpriteRenderer>().flipX = moveDirection < 0;

        if ((moveDirection == 1 && pos.x >= maxX) || (moveDirection == -1 && pos.x <= minX))
        {
            isPaused = true;
            pauseTimer = 2f;
            // Clamp position to boundary
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            player.transform.position = pos;

            player.GetComponent<Animator>().SetBool("IsMoving", false);

        }
    }

    public void PressedButton(int id)
    {
        switch (id)
        {
            case 0:
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                GameManager.instance.state = GameManager.GameState.Playing;
                break;
            case 1:
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                GameManager.instance.state = GameManager.GameState.Playing;
                GameManager.instance.ResteValues();
                break;
            case 2:
                Application.Quit();
                break;
            default:
                Debug.Log("Unknown Button Pressed");
                break;
        }
    }
}
