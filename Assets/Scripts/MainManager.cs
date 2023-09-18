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
    public Text bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_BestPoint;
    
    private bool m_GameOver = false;

    public string playerName;
    public string bestPlayerName;


    private void Awake()
    {
        LoadDataPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerName = GameManager.Instance.palyerName;

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

        bestScoreText.text = "Best Score : " + " " + bestPlayerName + " : " + m_BestPoint;
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

        if(m_Points >= m_BestPoint)
        {
            m_BestPoint= m_Points;
            bestPlayerName = playerName;
        }
    }

    public void GameOver()
    {
        bestScoreText.text = "Best Score : " + " "+ bestPlayerName + " : " + m_BestPoint;

        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveDataPlayer();
    }

    [System.Serializable]
    class SaveData
    {
        public int bestScoreSave;
        public string bestPlayer;
    }

    public void SaveDataPlayer()
    {
        SaveData data1 = new SaveData();
        data1.bestScoreSave = m_BestPoint;
        data1.bestPlayer = bestPlayerName;


        string json = JsonUtility.ToJson(data1);

        File.WriteAllText(Application.persistentDataPath + "/data1.json", json);
    }

    public void LoadDataPlayer()
    {
        string path = Application.persistentDataPath + "/data1.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data1 = JsonUtility.FromJson<SaveData>(json);

            m_BestPoint = data1.bestScoreSave;
            bestPlayerName = data1.bestPlayer;
        }
    }
}
