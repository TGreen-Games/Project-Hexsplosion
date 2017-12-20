using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape_AI : Shape
{

    private StateManager_AI actionindicator;
	private List <GameObject> grid;
	public float waitTime = 3f;
    public int bravery = 100;
    public int braveryLimit;
	public delegate void BoardChanged(int aiScore, Color color);
	public static event BoardChanged OnCapture;
    private int timidModifier;
	private int braveryCheck;
    // Use this for initialization
	// make random seed for script
    new private void Awake()
    {
		base.Awake ();
        actionindicator = this.gameObject.GetComponent<StateManager_AI>();
    }
    new private void OnEnable()
    {
		base.OnEnable ();
    }
	new private void Start()
    {
		base.Start ();
		grid = new List<GameObject>();
		grid.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
		actionindicator.AiState = Enums.AiStage.Neutral;
		StartCoroutine (Action());

    }

    // Update is called once per frame
    void Update()
    {
    }
		

//    private void StateChanged()
//    {
//        Move(actionindicator.AiState);
//    }

	private IEnumerator Action(){
		while (true) {
			Move (actionindicator.AiState);
			Fill ();
			yield return new WaitForSeconds (waitTime);
		}
	}

    private void Move(Enums.AiStage state)
    {
        if (state == Enums.AiStage.Attack)
        {
            timidModifier = 1;
            braveryLimit = 50;
            foreach (GameObject tile in grid)
            {
                var tileColor = tile.GetComponent<Image>().color;
                if (tileColor != shapeColor)
                {
					this.transform.position = (Vector2)tile.transform.position;
                    break;
                }
            }
        }
        else if(state == Enums.AiStage.Defence)
        {
			bravery = 150;
			braveryLimit = 50;
            foreach (GameObject tile in grid)
            {
                var tileColor = tile.GetComponent<Image>().color;
                if (tileColor == shapeColor)
                {
					this.transform.position = (Vector2)tile.transform.position;
                    break;
                }
            }

        }
        else
        {
			braveryLimit = 50;
			var randomTile = grid [Random.Range (0, grid.Count)];
			this.transform.position = (Vector2)randomTile.transform.position;
        }

		StartCoroutine (Expand ());
    }

    private IEnumerator Expand()
    {
        do
        {
            this.transform.localScale += new Vector3(scaleRate, scaleRate) * scaleSpeed;
			braveryCheck = Random.Range(0, braveryLimit) + timidModifier;
            timidModifier++;
            yield return new WaitForFixedUpdate();
        } while (bravery > braveryCheck);
		StartCoroutine (Fill ());

    }

    private IEnumerator Fill()
    {
        this.fillShape.SetActive(true);
        while (this.fillShape.activeSelf)
        {
            yield return null;
        }
		this.transform.localScale = scale;
		OnCapture(score, shapeColor);
       
    }


    private void SearchGrid()
    {
        // do this next
    }
}
