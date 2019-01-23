using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager2 : MonoBehaviour
{
	public AudioClip bgm;
	public GameObject countDownObject;
	private Text countDownText;
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
		
        
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= onSceneLoaded;
	}

	public void AddPlayer(Color playerColor, Shape player)
	{
		players.Add(playerColor, player);
	}

	public void RemovePlayer(Color playerColor)
	{
		players.Remove(playerColor);
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
		{
			players.Clear();
		}
		

	}
       

}
