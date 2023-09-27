using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Text highScoreTextBox;
    private string highScoreText;
    // Start is called before the first frame update
    void Start()
    {
        highScoreText = ("name: " + MainManager.HighScores[0] + "\nname: " + MainManager.HighScores[1] + "\nname: " + MainManager.HighScores[2]);
        highScoreTextBox.text = highScoreText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
