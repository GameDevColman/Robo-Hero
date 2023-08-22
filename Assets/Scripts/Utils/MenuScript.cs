using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public Canvas exitMenuCanvas;
    // public Canvas playMenuCanvas;
    public Button playButton;
    public Button exitButton;

    private void Start()
    {
        exitMenuCanvas.enabled = false;
        // playMenuCanvas.enabled = false;
    }

    public void onExitButtonClicked()
    {
        exitMenuCanvas.enabled = true;
        playButton.enabled = false;
        exitButton.enabled = false;
    }

    public void onNoExitConfirmButtonClicked()
    {
        exitMenuCanvas.enabled = false;
        playButton.enabled = true;
        exitButton.enabled = true;
    }

    public void onYesExitConfirmButtonClicked()
    {
        Application.Quit();
    }

    // public void onPlayButtonClicked()
    // {
    //     playMenuCanvas.enabled = true;
    //     playButton.enabled = false;
    //     exitButton.enabled = false;
    // }

    public void onClosePlayMenuButtonClicked()
    {
        // playMenuCanvas.enabled = false;
        playButton.enabled = true;
        exitButton.enabled = true;
    }

    public void StartScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
