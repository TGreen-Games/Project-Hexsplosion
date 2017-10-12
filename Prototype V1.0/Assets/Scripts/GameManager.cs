using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public List<Text> labelScores;
    private GameObject[] players;
    private List<int> playerScores = new List<int>(4);
    private List<Color> playerColors = new List<Color>(4);
    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        Timer.onGameOver += LoadScoreScreen;
        GetPlayerColors();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void DisplayScores()
    {
        for(int i = 0; i < players.Length; i++)
        {
            playerScores.Add(players[i].GetComponent<Shape>().Score);
        }

        playerScores.Sort();

       for(int i = 0; i < players.Length; i++)
        {
            
            labelScores[i].text = playerScores[i].ToString();
        }
        foreach (GameObject player in players)
        {
            int i = 0;
            if (player.GetComponent<Shape>().Score == playerScores[i])
            {
                i = 0;
                if (playerColors[i] == player.GetComponent<Shape>().playerColor)
                    labelScores[i].color = playerColors[i];
                else
                    i++;
                
            }
            else
            i++;
        }
    }

    void GetPlayerColors()
    {
        for(int i = 0; i < players.Length; i++)
        {
            playerColors.Add(players[i].GetComponent<Shape>().playerColor);
        }
    }

    private void LoadScoreScreen()
    {
        SceneManager.LoadScene(1);
    }

    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            Debug.Log("This is running!!!");
            DisplayScores();
        }

    }
}
