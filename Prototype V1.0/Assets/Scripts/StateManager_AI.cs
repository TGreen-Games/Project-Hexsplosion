using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_AI : MonoBehaviour {

	private Enums.AiStage aiState = Enums.AiStage.Defence;
    public Enums.AiStage AiState{ 
        get { return aiState; } 
        set{
            if (aiState == value) return;
            aiState = value;
            if (onStateChanged != null)
                onStateChanged(aiState);
        }
    }
    private Dictionary<Color, int> playerScores = new Dictionary<Color, int>();
    private int playerScore;
    public int place;
    private List<int> sortedScores = new List<int>();
    private Color playerColor;
    public delegate void UpdateState(Enums.AiStage currentState);
    public event UpdateState onStateChanged;

	private void Awake(){
	}

	private void OnEnable(){
        GameManager2.NotifyAi += assignPlace;
	}

	// Use this for initialization

	void Start () {
        playerColor = this.gameObject.GetComponent<SpriteRenderer>().color;     
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  //  public void RecievePlayerScores(Dictionary <Color, int> updatedScores)
  //  {
		//playerScores = updatedScores;
		//assignPlace();
    //    assignState();
    //}

    private void assignPlace(Color color){

        if(playerColor == color){
            place = 1;
            foreach (Shape player in GameManager2.Instance.players.Values)
            {
                int score = player.score;
                if (playerScore < score)
                    place++;
            }

            this.GetComponent<Shape>().place = place;
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

