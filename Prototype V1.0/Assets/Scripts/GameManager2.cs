using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour {

    public static GameManager2 Instance
    {
        get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager2>()); }
    }
    private static GameManager2 instance;
    public Dictionary<Color, Shape> players = new Dictionary<Color, Shape>();
  


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPlayer(Color playerColor, Shape player)
    {
        players.Add(playerColor, player);
    }

    public void AddScore(Color playerColor)
    {
        players[playerColor].score++;
    }

    public void MinusScore(Color playerColor)
    {
        players[playerColor].score--;
    }

    //Figure out a way for the ai to shoot stun!!!
}
