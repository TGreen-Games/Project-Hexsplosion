using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager2 : MonoBehaviour
{
	public AudioClip bgm;
	public Dictionary<Color, Shape> players = new Dictionary<Color, Shape>();
	public delegate void SendScoreData(Color color, int playerScore);
	public static event SendScoreData NotifyAi;
	public static GameManager2 Instance
	{
		get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager2>()); }
	}
	private static GameManager2 instance;
	private GameObject[] scoreLabels;


	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
        
		SceneManager.sceneLoaded += onSceneLoaded;
	}

	private void Start()
	{
		//SoundManager.Instance.MusicSource.clip = bgm;
		//SoundManager.Instance.MusicSource.Play();
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= onSceneLoaded;
	}

	public void AddPlayer(Color playerColor, Shape player)
	{
		players.Add(playerColor, player);
	}

	public void AddScore(Color playerColor, int playerScore)
	{
		players[playerColor].score++;
		playerScore = players[playerColor].score;
		NotifyAi(playerColor, playerScore);
	}

	public void MinusScore(Color playerColor, int playerScore)
	{
		players[playerColor].score--;
		playerScore = players[playerColor].score;
		NotifyAi(playerColor, playerScore);
	}

	private void onSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene().buildIndex == (int)Enums.Scenes.ScoreScreen)
		{
			SoundManager.Instance.EffectsSource.Stop();
			//DisplayScores();
		}
		else if (SceneManager.GetActiveScene().buildIndex == (int)Enums.Scenes.Start)
			players.Clear();
		

	}

	private void DisplayScores()
	{
		Color winningColor = Color.black;
		scoreLabels = GameObject.FindGameObjectsWithTag("scorelabels");
		int[] sortedScores = new int[4];
		var i = 0;
		foreach (var player in players.Values)
		{
			sortedScores[i] = player.score;
			i++;
		}                               
		Array.Sort(sortedScores);
		Array.Reverse(sortedScores);
		for (int k = 0; k < sortedScores.Length; k++)
		{
			for (int g = 0; g < scoreLabels.Length; g++)
			{
				var scoreText = scoreLabels[g].GetComponent<Text>();
				if (scoreLabels[g].GetComponent<Text>().text == k.ToString())
					scoreLabels[g].GetComponent<Text>().text = sortedScores[k].ToString();
											
			}
		}
		var gameOverText = GameObject.FindWithTag("Title Text");
		foreach (Shape player in players.Values)
		{
			var playerScore = player.score;
			for (int t = 0; t < scoreLabels.Length; t++)
			{
				Debug.Log(playerScore + " " + player.isPlayer);
				if(sortedScores[0] == playerScore && player.isPlayer)
				{
					gameOverText.GetComponent<Text>().text = "You Won!";
					gameOverText.GetComponent<Text>().color = player.shapeColor;
				}
				else if(sortedScores[0] == playerScore && player.isPlayer == false)
				{
					gameOverText.GetComponent<Text>().color = player.shapeColor;
				}

				if (sortedScores[t] == playerScore && scoreLabels[t].GetComponent<Text>().color == Color.clear)
                {
                    scoreLabels[t].GetComponent<Text>().color = player.shapeColor;
					break;
                    
                }
					
			}

		}
       
	}

}
