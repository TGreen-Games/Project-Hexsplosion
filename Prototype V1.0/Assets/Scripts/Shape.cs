using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    public GameObject fillShape;
    public float scaleRate = 0.001f;
    public float scaleSpeed = 0.1f;
    public Color shapeColor;
    protected int score;
    public int Score { get { return score; } }
    private Button[] tiles;
    private TileManager tileManager;
    protected Vector3 scale;

    // Use this for initialization
    protected void Awake()
    {
		DontDestroyOnLoad (this.transform.root.gameObject);
        tileManager = this.gameObject.GetComponent<TileManager>();
		Debug.Log ("This Happened");
    }

    protected void OnEnable()
    {
        Timer.onGameOver += CalculateScore;
		shapeColor = this.GetComponent<SpriteRenderer>().color;

    }
    protected void OnDisable()
    {
        Timer.onGameOver -= CalculateScore;
    }
    protected void Start()
    {
        Debug.Log(fillShape);
        scale = this.transform.localScale;
        tiles = GameObject.FindGameObjectWithTag("Grid").GetComponentsInChildren<Button>();
		Debug.Log ("Yhis Happened!!!!");
    }

    private void CalculateScore()
    {
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Image>().color == shapeColor)
                score++;
        }

    }

 

}

