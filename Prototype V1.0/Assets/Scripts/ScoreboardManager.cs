using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ScoreboardManager : MonoBehaviour
{

	public GameObject gameOverText;
	public GameObject[] scoreLabels;
	private int[] sortedScores = new int[4];

	private void OnEnable()
	{
		SceneManager.sceneLoaded += onSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= onSceneLoaded;
	}

	private void RetrieveScores()
	{
		
		var i = 0;
		foreach (var player in GameManager2.Instance.players.Values)
		{
			sortedScores[i] = player.score;
			i++;
		}
		Array.Sort(sortedScores);
		Array.Reverse(sortedScores);
	}

	private void DisplayScores()
	{ 
		foreach(Shape player in GameManager2.Instance.players.Values)
		{
			for (int t = 0; t < scoreLabels.Length; t++)
            {
               
				if (sortedScores[0] == player.score && player.isPlayer)
                {
                    gameOverText.GetComponent<Text>().text = "You Won!";
                    gameOverText.GetComponent<Text>().color = player.shapeColor;
                }
				else if (sortedScores[0] == player.score && player.isPlayer == false)
                {
                    gameOverText.GetComponent<Text>().color = player.shapeColor;
                }

				if (sortedScores[t] == player.score && scoreLabels[t].GetComponent<Text>().color == Color.clear)
                {
					scoreLabels[t].GetComponent<Text>().text = sortedScores[t].ToString();
                    scoreLabels[t].GetComponent<Text>().color = player.shapeColor;
                    break;

                }

            }
		}
	}

	private void onSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if(SceneManager.GetActiveScene().buildIndex == (int) Enums.Scenes.ScoreScreen)
		{
			RetrieveScores();
			DisplayScores();
		}
	}
}
