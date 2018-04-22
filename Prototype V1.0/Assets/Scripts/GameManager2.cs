using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager2 : MonoBehaviour
{

    public static GameManager2 Instance
    {
        get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager2>()); }
    }
    private static GameManager2 instance;
    private Text[] scoreLabels;
    public Dictionary<Color, Shape> players = new Dictionary<Color, Shape>();
    public delegate void SendScoreData(Color color);
    public static event SendScoreData NotifyAi;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	private void OnEnable()
	{
        SceneManager.sceneLoaded += onSceneLoaded;
	}
	// Use this for initialization
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayer(Color playerColor, Shape player)
    {
        players.Add(playerColor, player);
    }

    public void AddScore(Color playerColor)
    {
        players[playerColor].score++;
        NotifyAi(playerColor);
    }

    public void MinusScore(Color playerColor)
    {
        players[playerColor].score--;
        NotifyAi(playerColor);
    }

    //private void assignPlaces(Color playerColor)
    //{
    //    int t = 1;
    //    foreach(Shape player in players.Values)
    //    {
    //        var place = player.place;
    //        if(players[playerColor].score < player.score)
    //        {
    //            t++;

    //        }

    //    }
    //}

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            DisplayScores();
        }

    }

    private void DisplayScores()
    {
        Color winningColor = Color.black;
        var highestScore = 0;
        scoreLabels = FindObjectsOfType<Text>();
        foreach (var player in players.Values)
        {
            var temp = player.score;
            if (temp > highestScore){
                highestScore = temp;
                winningColor = player.shapeColor;

            }
                
            
        }
        scoreLabels[0].color = winningColor;
        scoreLabels[0].text = highestScore.ToString();
    }

}
