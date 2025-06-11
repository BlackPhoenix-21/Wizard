using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public static GameManager instance;
    public GameState state;
    private float timer;
    public Wizard[] wizard;
    public Wizard[] restWizard;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            state = GameState.MainMenu;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (state == GameState.Playing)
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                timer = 0;
                SceneManager.LoadScene("Title", LoadSceneMode.Single);
                state = GameState.MainMenu;
            }
        }
    }

    public void ResteValues()
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().RestWizard();
    }
}