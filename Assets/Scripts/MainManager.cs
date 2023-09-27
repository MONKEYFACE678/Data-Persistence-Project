using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    static public int[] HighScores = new int[3];
    static public string[] Names = new string[3];

    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;


    void Awake()
    {
        LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        CheckScore();
    }

    public void EnterHighScoreMenu()
    {
        CheckScore();
        SceneManager.LoadScene(0);
        SaveData();
    }

    private void CheckScore()
    {
        if (m_Points > HighScores[0])
        {
            HighScores[2] = HighScores[1];
            HighScores[1] = HighScores[0];
            HighScores[0] = m_Points;
        }
        else if (m_Points > HighScores[1])
        {
            HighScores[2] = HighScores[1];
            HighScores[1] = m_Points;
        }
        else if (m_Points > HighScores[2])
        {
            HighScores[2] = m_Points;
        }
        Debug.Log("1st " + HighScores[0] + " 2nd " + HighScores[1] + " 3rd " + HighScores[2]);
    }

    private class HighScoreData
    {
        public int[] highScoresTemp;
        public string[] namesTemp;
    }

    public void SaveData()
    {
        HighScoreData tempScores = new HighScoreData();
        tempScores.highScoresTemp = HighScores;
        tempScores.namesTemp = Names;

        string save = JsonUtility.ToJson(tempScores);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", save);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string load = File.ReadAllText(path);
            HighScoreData loadData = JsonUtility.FromJson<HighScoreData>(load);
            Names = loadData.namesTemp;
            HighScores = loadData.highScoresTemp;
            Debug.Log(path);
        }
    }

    public void Exit()
    {
        SaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
