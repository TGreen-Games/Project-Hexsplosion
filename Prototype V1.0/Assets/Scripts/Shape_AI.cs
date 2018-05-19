using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape_AI : Shape
{
	public Shape attackMe;
    private StateManager_AI actionindicator;
    private List<GameObject> grid;
    public float detectionRadius = 1;
    public float waitTime = 3f;
    public int bravery = 100;
    public int braveryLimit = 50;
    //public int seed;
    public delegate void ShootAttack(Shape playerHit, Color attackingColor);
    public static event ShootAttack OnShoot;
    //public delegate void BoardChanged(int aiScore, Color color);
    //public static event BoardChanged OnCapture;
    private int timidModifier;
    private int braveryCheck;
    private Color tileColor;
    private Vector2 tilePosition;
    private StunShot shotScript;
    public System.Random generateRandomNum;
    // Use this for initialization
    // make random seed for script
    private void Awake()
    {
        actionindicator = this.gameObject.GetComponent<StateManager_AI>();
        shotScript = this.GetComponent<StunShot>();
    }
    new private void OnEnable()
    {
        base.OnEnable();
        Player.OnAttacking += imHit;
        Shape_AI.OnShoot += imAttacking;
        //seed = System.Environment.TickCount + this.gameObject.GetHashCode();

    }
    new private void Start()
    {
        base.Start();
        generateRandomNum = new System.Random(System.Environment.TickCount + this.gameObject.GetHashCode());
        grid = new List<GameObject>();
        grid.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
        actionindicator.AiState = Enums.AiStage.Neutral;
        StartCoroutine(Action());
        //TileManager.instance.AddColor(shapeColor);




    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    //private void UpdateState(Enums.AiStage currentState){

    //}
    private IEnumerator Action()
    {
        while (true)
        {
            waitTime = generateRandomNum.Next(1, 3);
			if (shotScript.isAttacking() && canShoot)

            {
                var hitPlayer = shotScript.FindTarget();
				OnShoot(hitPlayer, shapeColor);
				canShoot = false;
                Debug.Log("I just shot! My color is: " + shapeColor.ToString());
            }
            else if (canMove)
            {
                canMove = false;
                Move(actionindicator.AiState);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void Move(Enums.AiStage state)
    {
        if (state == Enums.AiStage.Attack)
        {
            foreach (GameObject tile in grid)
            {
                GetTileInfo(tile);
                if (tileColor != shapeColor && isPlayerDetected(tilePosition, detectionRadius) == null)
                {
                    this.transform.position = (Vector2)tile.transform.position;
                    break;
                }
            }
        }
        else if (state == Enums.AiStage.Defence)
        {
            bravery = 150;
            foreach (GameObject tile in grid)
            {
                GetTileInfo(tile);
                if (tileColor == shapeColor && isPlayerDetected(tilePosition, detectionRadius) == null)
                {
                    this.transform.position = (Vector2)tile.transform.position;
                    break;
                }
            }

        }
        else
        {
            while (FoundRandomTile() == false) { }
        }

        StartCoroutine(Expand());
    }

    private IEnumerator Expand()
    {
        timidModifier = 0;
        do
        {
            this.transform.localScale += new Vector3(scaleRate, scaleRate) * scaleSpeed;
            var randomNum = generateRandomNum.Next(0, braveryLimit);
            braveryCheck = randomNum + timidModifier;
            timidModifier++;
            yield return new WaitForFixedUpdate();
        } while (bravery > braveryCheck);
        StartCoroutine(Fill());

    }

    private IEnumerator Fill()
    {
        this.fillShape.SetActive(true);
        while (this.fillShape.activeSelf)
        {
            yield return null;
        }
        this.transform.localScale = scale;
        //OnCapture(score, shapeColor);
        canMove = true;



    }

    private void GetTileInfo(GameObject tile)
    {
        tileColor = tile.GetComponent<Image>().color;
        tilePosition = tile.transform.position;
    }

    private bool FoundRandomTile()
    {
        var randomNum = generateRandomNum.Next(0, grid.Count);
        GameObject randomTile = grid[randomNum];
        GetTileInfo(randomTile);
        if (isPlayerDetected(tilePosition, detectionRadius) == false)
        {
            this.transform.position = (Vector2)randomTile.transform.position;
            return true;
        }
        else
            return false;
    }

    //rework this next
    private Collider2D isPlayerDetected(Vector2 position, float radius)
    {
        Collider2D[] hitPLayers = Physics2D.OverlapCircleAll(position, radius);
        foreach (Collider2D player in hitPLayers)
        {
            if (player.gameObject.CompareTag("Player") && player.gameObject != this.gameObject)
                return player;
        }
        return null;
    }

    private void OnCollisionEnter2D(Collision2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            Instantiate(stunprefab, this.transform.position, Quaternion.identity);
            StopAllCoroutines();
            StartCoroutine(Stun());
        }

    }

    public IEnumerator Stun()
    {
        canMove = false;
        this.transform.localScale = scale;
        this.fillShape.SetActive(false);
        yield return new WaitForSeconds(collisionStunTime);
        canMove = true;
        StartCoroutine(Action());
    }

    private void imHit(RaycastHit2D playerHit, Color attackColor)
    {
        if (playerHit.collider.gameObject == this.gameObject)
        {
            var attackingShot = stunShot.main;
            attackingShot.startColor = new ParticleSystem.MinMaxGradient(attackColor);
            Instantiate(stunShot, this.transform.position, Quaternion.identity);
            StopAllCoroutines();
            StartCoroutine(Stun());
        }
    }

    private void imAttacking(Shape hitPlayer, Color attackColor)
    {
        if (hitPlayer == this && canShoot)
        {
            var attackingShot = stunShot.main;
            attackingShot.startColor = new ParticleSystem.MinMaxGradient(attackColor);
            Instantiate(stunShot, this.transform.position, Quaternion.identity);
            StopAllCoroutines();
            canShoot = false;
            StartCoroutine(Stun());

        }

        StartCoroutine(cooldownTimer(shotCooldown));
    }

}
