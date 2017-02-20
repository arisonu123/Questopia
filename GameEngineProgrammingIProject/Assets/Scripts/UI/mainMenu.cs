using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class mainMenu : MonoBehaviour {


    [SerializeField]
    private string gameScene = "gameScene";
    /// <summary>
    /// Starts the game
    /// </summary>
	public void startGame()
    {
        Toolbox.loadManager.load(gameScene);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void quitGame()
    {
       Application.Quit();
       #if UNITY_EDITOR
       UnityEditor.EditorApplication.isPlaying = false;
       #endif
    }

    /// <summary>
    /// Calls the playButtonSound in the UIManager, used when the UIManager is not initially in the scene, but will be at the time of calling this function
    /// </summary>
    public void playButtonSound()
    {
        Toolbox.UI.playButtonSound();
    }
}
