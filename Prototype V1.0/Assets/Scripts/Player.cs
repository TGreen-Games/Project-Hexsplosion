using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Shape {
    private delegate void OnStateChange();
    private event OnStateChange onStateChange;
    private Enums.PlayerStage state;
    private Enums.PlayerStage State
    {
        get { return state; }
        set
        {
            if (state == value) return;
            state = value;
            if (onStateChange != null)
                onStateChange();
        }
    }
    private DetectTouch detectTouch;

	private DigitalRubyShared.GestureTouch FirstTouch(ICollection<DigitalRubyShared.GestureTouch> touches)
	{
		foreach(DigitalRubyShared.GestureTouch touch in touches)
		{
			return touch;
		}
		return new DigitalRubyShared.GestureTouch();
	}
    public delegate int BoardChanged(int playerScore, Color playerColor);
    public static event BoardChanged onCapture;


    // Use this for initialization
    new private void Awake()
    {
		base.Awake();
        detectTouch = this.gameObject.GetComponent<DetectTouch>();
    }
    new private void OnEnable()
    {
		base.OnEnable();
        detectTouch.onTouch += TouchHandler;
        onStateChange += StateHandler;
    }
   new private void OnDisable()
    {
		base.OnDisable();
        detectTouch.onTouch -= TouchHandler;
        onStateChange -= StateHandler;
    }
    new void Start () {
		base.Start();
		state = Enums.PlayerStage.Neutral;
	}
	
	// Update is called once per frame
	void Update () {
		
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
        score = onCapture(score, shapeColor);

    }
}
