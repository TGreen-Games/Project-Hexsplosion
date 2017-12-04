using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    public GameObject fillShape;
    public float scaleRate = 0.001f;
    public float scaleSpeed = 0.1f;
    public Color playerColor;
    public int score;
    public int Score { get { return score; } }
    private Button[] tiles;
    private TileManager tileManager;
    private DetectTouch detectTouch;
    private Vector3 scale;
    public delegate int BoardChanged(int playerScore, Color playerColor);
    public static event BoardChanged onCapture;
    private Enums.PlayerStage state;
    private Enums.PlayerStage State
    {
        get { return state; }
        set
        {
            if (state == value) return;
            state = value;
            if(onStateChange != null)
            onStateChange();
        }
    }
    private delegate void OnStateChange();
    private event OnStateChange onStateChange; 
    private DigitalRubyShared.GestureTouch FirstTouch(ICollection<DigitalRubyShared.GestureTouch> touches)
    {
        foreach(DigitalRubyShared.GestureTouch touch in touches)
        {
            return touch;
        }
        return new DigitalRubyShared.GestureTouch();
    }
    private void TouchHandler(Vector2 position)
    {
        if (State == Enums.PlayerStage.Neutral)
        {
            this.transform.position = position;
            State = Enums.PlayerStage.Expand;
        }
        else
            State = Enums.PlayerStage.Fill;
    }
    private void StateHandler()
    {
        switch (State)
        {
            case Enums.PlayerStage.Expand:
                StartCoroutine(Expand());
                break;
            case Enums.PlayerStage.Neutral:
                this.transform.localScale = scale;
                break;
            case Enums.PlayerStage.Fill:
                StartCoroutine(Fill());
                break;
            default:
                break;
        }
    }



    // Use this for initialization
    private void Awake()
    {
		DontDestroyOnLoad (this.transform.root.gameObject);
        detectTouch = this.gameObject.GetComponent<DetectTouch>();
        tileManager = this.gameObject.GetComponent<TileManager>();  
    }

    private void OnEnable()
    {
       
        detectTouch.onTouch += TouchHandler;
        onStateChange += StateHandler;
        Timer.onGameOver += CalculateScore;
    }
    private void OnDisable()
    {
        detectTouch.onTouch -= TouchHandler;
        onStateChange -= StateHandler;
        Timer.onGameOver -= CalculateScore;
    }
    void Start()
    {
        Debug.Log(fillShape);
        state = Enums.PlayerStage.Neutral;
        scale = this.transform.localScale;
        tiles = GameObject.FindGameObjectWithTag("Grid").GetComponentsInChildren<Button>();
        playerColor = this.GetComponent<SpriteRenderer>().color;

    }
  

    // Update is called once per frame
    private void FixedUpdate()
    {
       
    }


    private IEnumerator Expand()
    {

        while (State == Enums.PlayerStage.Expand)
        {
            this.transform.localScale += new Vector3(scaleRate, scaleRate) * scaleSpeed;
            yield return new WaitForFixedUpdate();
        }
        
        
       
    }

    private IEnumerator Fill()
    {
        fillShape.SetActive(true);
        while (fillShape.activeSelf)
        {
            yield return null;
        }
        yield return new WaitForFixedUpdate();
        State = Enums.PlayerStage.Neutral;
        score = onCapture(score, playerColor);
       
    }

    private void CalculateScore()
    {
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Image>().color == playerColor)
                score++;
        }

    }

    private void addScore()
    {
        var dirtyTiles = Tile.dirtyTiles;
        foreach (GameObject tile in dirtyTiles)
        {
            Debug.Log("This is firing!!!");
            var tileColor = tile.GetComponent<Image>().color;
            if (tileColor == playerColor)
            {
                score++;
                tileColor = playerColor;
            }

        }
    }

}

