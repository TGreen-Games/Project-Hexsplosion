using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape_AI : Shape
{

    private StateManager_AI actionindicator;
	private List <GameObject> grid;
    public int bravery = 100;
    public int braveryLimit;

    private int timidModifier;
    private int braveryCheck;
    // Use this for initialization

    new private void Awake()
    {
		base.Awake ();
        actionindicator = this.gameObject.GetComponent<StateManager_AI>();
    }
    new private void OnEnable()
    {
		base.OnEnable ();
        actionindicator.onStateChanged += StateChanged;
    }
	new private void Start()
    {
		base.Start ();
		grid = new List<GameObject>();
		grid.AddRange(GameObject.FindGameObjectsWithTag("Tile"));

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StateChanged()
    {
        Move(actionindicator.AiState);
        Expand();
        Fill();
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
                    this.transform.position = tile.transform.position;
                    break;
                }
            }
        }
        else if(state == Enums.AiStage.Defence)
        {
            bravery = 20;
            foreach (GameObject tile in grid)
            {
                var tileColor = tile.GetComponent<Image>().color;
                if (tileColor == shapeColor)
                {
					this.transform.position = tile.transform.localPosition;
                    break;
                }
            }

        }
        else
        {
			var randomTile = grid [Random.Range (0, grid.Count)];
			this.transform.position = new Vector3 (randomTile.transform.localPosition.x, randomTile.transform.localPosition.y,0);
        }
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

    }

    private IEnumerator Fill()
    {
        fillShape.SetActive(true);
        while (fillShape.activeSelf)
        {
            yield return null;
        }
        yield return new WaitForFixedUpdate();
    }


    private void SearchGrid()
    {
        // do this next
    }
}
