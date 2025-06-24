using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
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
                GameManager.instance.RestWizard();
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
