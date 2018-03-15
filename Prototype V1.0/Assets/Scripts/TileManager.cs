﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public static TileManager instance { get; private set; }
    //public Dictionary<Color, List<GameObject>> capturedTiles = new Dictionary<Color, List<GameObject>>();
    private Tile[] grid;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        instance = this;
        //grid = gameObject.GetComponentsInChildren<Tile>();
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
    //public void AddColor(Color playerColor)
    //{
    //    capturedTiles.Add(playerColor, new List<GameObject>());
    //}

    public void AddTile(GameObject tile, Color playerColor)
    {
        //capturedTiles[playerColor].Add(tile);
        GameManager2.Instance.AddScore(playerColor);
    }

    //private void minusScore(int playerScore, Color playerColor)
    //{
    //    foreach (GameObject tile in dirtyTiles)
    //    {
    //        if (playerColor == tile.GetComponent<Image>().color)
    //            playerScore--;
    //        else
    //            continue;
    //    }
    //    OnCapture();
    //}

    //private void addScore(int playerScore, Color playerColor)
    //{
        //foreach (GameObject tile in dirtyTiles)
        //{
        //    Debug.Log("This is firing!!!");
        //    var tileColor = tile.GetComponent<Image>().color;
        //    if (tileColor == playerColor)
        //    {
        //        playerScore++;
        //        tileColor = playerColor;
        //    }

        //}
    public void MinusTile(GameObject tile,Color playerColor)
    {
        GameManager2.Instance.MinusScore(playerColor);
        //if (capturedTiles.ContainsKey(playerColor))
        //{
        //    for (int i = 0; 0 < capturedTiles[playerColor].Count; i++)
        //    {
        //        capturedTiles[playerColor].Remove(tile);
        //        GameManager2.Instance.MinusScore(playerColor);
        //        Debug.Log(("score minused!"));
        //    }
        //}
    }


}

