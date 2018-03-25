using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunShot : MonoBehaviour {
    public int attackThreshold = 50;
    private List<GameObject> capturedTiles = new List<GameObject>();
    private Shape player;
    private int chanceModifier = 0;
    private StateManager_AI currentState;
    private System.Random generateRandomNum;


	// Use this for initialization
	void Start () {
        player = this.GetComponent<Shape>();
        currentState = this.GetComponent<StateManager_AI>();
        generateRandomNum = new System.Random(System.Environment.TickCount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //private void GetCapturedTiles()
    //{
    //    foreach (GameObject tile in Tile.dirtyTiles)
    //    {
    //        var tileColor = tile.GetComponent<Tile>().PlayerColor;
    //        if (tileColor == playerColor)
    //        {
    //            chanceModifier++;
    //            capturedTiles.Add(tile);
    //        }
    //    }
       
    //}

    //public bool isAttacking(){
    //    GetCapturedTiles();
    //    AssignVariables();
    //    var randomNum = generateRandomNum.Next(0, 50);
    //    if (randomNum + chanceModifier > attackThreshold)
    //        return true;
    //    else
    //        return false;

    //}

   
    public GameObject SelectTile(){
        if (capturedTiles != null)
        {
            var tile = capturedTiles[generateRandomNum.Next(0, capturedTiles.Count - 1)];
            capturedTiles.Clear();
            chanceModifier = 0;
            return tile;
        }
        else return null;

       
    }

    private void AssignVariables(){
        if (currentState.AiState == Enums.AiStage.Attack)
        {
            chanceModifier = 0;
        }
        else if (currentState.AiState == Enums.AiStage.Defence)
        {
            chanceModifier = 10;
        }
        else
            chanceModifier = 5;
    }

    private Shape FindTarget(){
        Shape[] possibleTargets = { };
        int i = 0;
        foreach(var enemy in GameManager2.Instance.players.Values)
        {
            if (enemy.place >= player.place)
            {
                possibleTargets[i] = enemy;
                i++;
            }
            else
                continue;
        }
        return possibleTargets[generateRandomNum.Next(0, possibleTargets.Length - 1)];
    }

}
