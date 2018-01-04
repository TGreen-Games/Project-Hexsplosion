using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{


    private static List<GameObject> dirtyTiles;
    private Shape player;
    public delegate void CaptureTiles();
    public event CaptureTiles OnCapture;


    private void Awake()
    {
        player = this.GetComponent<Shape>();
    }
    private void OnEnable()
    {
       
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void minusScore(int playerScore, Color playerColor)
    {
        foreach (GameObject tile in dirtyTiles)
        {
            if (playerColor == tile.GetComponent<Image>().color)
                playerScore--;
            else
                continue;
        }
        OnCapture();
    }

    private void addScore(int playerScore, Color playerColor)
    {
        foreach (GameObject tile in dirtyTiles)
        {
            Debug.Log("This is firing!!!");
            var tileColor = tile.GetComponent<Image>().color;
            if (tileColor == playerColor)
            {
                playerScore++;
                tileColor = playerColor;
            }

        }
    }
}

