using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager_AI : MonoBehaviour {

    private Enums.AiStage aiState;
    private Dictionary<Color, int> playerScores = new Dictionary<Color, int>();
    private int playerScore;
    private Color playerColor;

	// Use this for initialization
	void Start () {
        playerColor = this.gameObject.GetComponent<SpriteRenderer>().color;
        GameManager.sendToAI += RecievePlayerScores;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void RecievePlayerScores(Dictionary <Color, int> updatedScores)
    {
        playerScores = updatedScores;
    }

    private void assignState()
    {
       

    }
}
