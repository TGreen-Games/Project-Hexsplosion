using System.Collections.Generic;
using UnityEngine;

public class StateManager_AI : MonoBehaviour
{
	public Enums.AiStage AiState
	{
		get { return aiState; }
		set
		{
			if (aiState == value) return;
			aiState = value;
			Debug.Log("My state chaneged its this " + aiState);
			if (onStateChanged != null)
				onStateChanged(aiState);
		}
	}
	public int place;
	public delegate void UpdateState(Enums.AiStage currentState);
	public event UpdateState onStateChanged;
	private Enums.AiStage aiState = Enums.AiStage.Defence;
	private Dictionary<Color, int> playerScores = new Dictionary<Color, int>();
	private List<int> sortedScores = new List<int>();
	private Color playerColor;


	private void OnEnable()
	{
		GameManager2.NotifyAi += assignPlace;
	}

	private void OnDestroy()
	{
		GameManager2.NotifyAi -= assignPlace;
	}

	// Use this for initialization

	void Start()
	{
		playerColor = this.gameObject.GetComponent<SpriteRenderer>().color;
	}


	private void assignPlace(Color color, int playerScore)
	{

		if (playerColor == color)
		{
			place = 1;
			foreach (Shape player in GameManager2.Instance.players.Values)
			{
				int score = player.score;
				if (playerScore < score)
					place++;
			}
			assignState();

		}

	}

	private void assignState()
	{
		if (place == 1)
			AiState = Enums.AiStage.Defence;
		else if (place == 4)
			AiState = Enums.AiStage.Attack;
		else
			AiState = Enums.AiStage.Neutral;

	}



}

