using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


    public delegate void SendScoreData(Dictionary <Color,int> playerScores);
    public static event SendScoreData sendToAI;
	private Text[] scoreLabels;
    private GameObject[] players;
    private Dictionary<Color, int> playerScores = new Dictionary<Color, int>();
    private GameObject[] grid;

    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);

    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
        Player.onCapture += UpdatePlayerScore;
        Timer.onGameOver += LoadScoreScreen;
		Shape_AI.OnCapture += UpdateAiScore;
    }
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        grid = GameObject.FindGameObjectsWithTag("Tile");
        AddPlayerScores();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

  //  private void DisplayScores()
  //  {
		//scoreLabels = FindObjectsOfType<Text> ();
       

   //    for(int i = 0; i < players.Length; i++)
   //     {
            
			//scoreLabels [i].text = playerScores [i].ToString ();
    //    }
    //    foreach (GameObject player in players)
    //    {
    //        int i = 0;
    //        if (player.GetComponent<Shape>().Score == playerScores[i])
    //        {
    //            i = 0;
    //            if (playerColors[i] == player.GetComponent<Shape>().playerColor)
				//	scoreLabels[i].color = playerColors[i];
    //            else
    //                i++;
                
    //        }
    //        else
    //        i++;
    //    }
    //}


    void AddPlayerScores(){
		foreach(GameObject player in players){
			var playerColor = player.GetComponent<SpriteRenderer> ().color;
			playerScores.Add (playerColor, 0);
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
            //DisplayScores();
        }

    }

    private int UpdatePlayerScore(int playerScore, Color playerColor)
    {
		playerScore = CountScore (playerScore, playerColor);
        playerScores[playerColor] = playerScore;
        return playerScore;

    }

	private int CountScore(int score, Color color){
		foreach (GameObject tile in grid) {
			var tileColor = tile.GetComponent<Image> ().color;      
			if (tileColor == color)
				score++;
		}
		return score;
	}

	private void UpdateAiScore(int aiScore, Color aiColor){
		aiScore = CountScore (aiScore, aiColor);
		playerScores [aiColor] = aiScore;
		sendToAI (playerScores);
	}


		

	

	


			


    //private List <int> SendScores(List <int> scores){
        //scores = playerScores;
        //return scores;
    }
