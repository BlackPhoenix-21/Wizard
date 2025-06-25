using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    // Wird aufgerufen, wenn ein Button im Titelbildschirm gedrückt wird
    public void PressedButton(int id)
    {
        switch (id)
        {
            case 0:
                // Starte das Spiel und setze den Spielstatus auf "Playing"
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                GameManager.instance.state = GameManager.GameState.Playing;
                break;
            case 1:
                // Starte das Spiel, setze den Spielstatus auf "Playing" und setze den Zauberer zurück
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                GameManager.instance.state = GameManager.GameState.Playing;
                GameManager.instance.RestWizard();
                break;
            case 2:
                // Beende die Anwendung
                Application.Quit();
                break;
            default:
                // Unbekannter Button wurde gedrückt
                Debug.Log("Unknown Button Pressed");
                break;
        }
    }
}
