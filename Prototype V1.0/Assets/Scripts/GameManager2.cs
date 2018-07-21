
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager2 : MonoBehaviour
{
	public Dictionary<Color, Shape> players = new Dictionary<Color, Shape>();
	public delegate void SendScoreData(Color color, int playerScore);
	public static event SendScoreData NotifyAi;
	public static GameManager2 Instance
	{
		get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager2>()); }
	}
	private static GameManager2 instance;
	private Text[] scoreLabels;


	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{

		SceneManager.sceneLoaded += onSceneLoaded;
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
		if (SceneManager.GetActiveScene().buildIndex == 2)
		{
			DisplayScores();
			players.Clear();
		}

	}

	private void DisplayScores()
	{
		Color winningColor = Color.black;
		scoreLabels = FindObjectsOfType<Text>();
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
			scoreLabels[k].text = sortedScores[k].ToString();
		foreach (Shape player in players.Values)
		{
			var playerScore = player.score;
			for (int t = 0; t < scoreLabels.Length; t++)
			{
				if (sortedScores[t] == playerScore && scoreLabels[t].color != player.shapeColor)
					scoreLabels[t].color = player.shapeColor;
			}

		}
	}

}
