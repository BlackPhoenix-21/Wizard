using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    // Wird aufgerufen, wenn ein Button im Titelbildschirm gedr�ckt wird
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
                // Starte das Spiel, setze den Spielstatus auf "Playing" und setze den Zauberer zur�ck
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                GameManager.instance.state = GameManager.GameState.Playing;
                GameManager.instance.RestWizard();
                break;
            case 2:
                // Beende die Anwendung
                Application.Quit();
                break;
            default:
                // Unbekannter Button wurde gedr�ckt
                Debug.Log("Unknown Button Pressed");
                break;
        }
    }
}
