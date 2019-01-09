using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape_AI : Shape
{
	public StateManager_AI actionindicator;

	public int expansionLimit = 100;
	public int braveryModifier = 50;
	public delegate void ShootAttack(Shape playerHit, Color attackingColor);
	public static event ShootAttack OnShoot;
	public System.Random generateRandomNum;
	protected List<GameObject> grid;
    private float waitTime = 3f;
	protected int timidModifier;
	protected int braveryCheck;
	protected Color tileColor;
    protected Vector2 tilePosition;
    private StunShot shotScript;
	private bool actionStarted;
	protected Shape priorityTarget;

	private void Awake()
	{
		actionindicator = this.gameObject.GetComponent<StateManager_AI>();
		shotScript = this.GetComponent<StunShot>();
	}
	new private void OnEnable()
	{
		base.OnEnable();
		Player.OnAttacking += humanPlayerHitMe;
		Shape_AI.OnShoot += aiPLayerHitMe;
		Shape.OnGreed += priorityShot;

	}

	private void OnDestroy()
	{
		Player.OnAttacking -= humanPlayerHitMe;
		Shape_AI.OnShoot -= aiPLayerHitMe;
		Shape.OnGreed -= priorityShot;
	}
	protected override void Start()
	{
		base.Start();
		generateRandomNum = new System.Random(System.Environment.TickCount + this.gameObject.GetHashCode());
		grid = new List<GameObject>();
		grid.AddRange(GameObject.FindGameObjectsWithTag("Tile"));
		actionindicator.AiState = Enums.AiStage.Neutral;

	}

	protected override void Update()
	{
		base.Update();
		if (isGamePaused)
			return;
		if(actionStarted == false)
			StartCoroutine(Action());
			
		
	}
	private IEnumerator Action()
	{
		actionStarted = true;
		while (true)
		{
			waitTime = generateRandomNum.Next(1, 6);
			if (priorityTarget != null && canShoot && canMove )
			{
				BeingGreedy(priorityTarget);
				StartCoroutine(cooldownTimer(shotCooldown));
				canShoot = false;
				priorityTarget = null;
			}
			else if (shotScript.isAttacking() && canShoot && score > 10 && canMove)
			{
				var hitPlayer = shotScript.FindTarget();
				if (hitPlayer.shapeSprite.enabled == true)
				{
					if (OnShoot != null)
					{
						OnShoot(hitPlayer, shapeColor);
						canShoot = false;
                        StartCoroutine(cooldownTimer(shotCooldown));
                        priorityTarget = null;
					}

				}							
			}
			else if (canMove)
			{
				canMove = false;
				isShapeActive(true);
				Move(actionindicator.AiState);
			}
			yield return new WaitForSeconds(waitTime);
		}
	}

	private void Move(Enums.AiStage state)
	{
		if (state == Enums.AiStage.Attack)
		{
			expansionLimit = 200;
			greedLimiter = 0.4f;
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
			expansionLimit = 150;
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

	protected virtual IEnumerator Expand()
	{
		timidModifier = 0;
		do
		{
			this.transform.localScale += new Vector3(scaleRate, scaleRate) * scaleSpeed;
			var bravery = generateRandomNum.Next(0, braveryModifier);
			braveryCheck = bravery + timidModifier;
			timidModifier++;
			yield return new WaitForFixedUpdate();
		} while (expansionLimit > braveryCheck);
		StartCoroutine(Fill());

	}

	protected virtual IEnumerator Fill()
	{
		this.fillShape.SetActive(true);
		while (this.fillShape.activeSelf)
		{
			yield return null;
		}
		this.transform.localScale = startingScale;
		canMove = true;
		IsGreedy = false;

	}


	protected void priorityShot(Shape greedyPlayer, bool isPlayerBeingGreedy)
	{
		if (isPlayerBeingGreedy && greedyPlayer != this)
			priorityTarget = greedyPlayer;
		else
			priorityTarget = null;
	}


	protected void GetTileInfo(GameObject tile)
	{
		tileColor = tile.GetComponent<Image>().color;
		tilePosition = tile.transform.position;
	}

	protected bool FoundRandomTile()
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

    
	protected Collider2D isPlayerDetected(Vector2 position, float radius)
	{
		var hitPlayer = Physics2D.OverlapCircle(position, radius, 1 << playerLayer);
		if (hitPlayer != null)
			return hitPlayer;
		else
		    return null;
	}

	private void OnCollisionEnter2D(Collision2D player)
	{
		if (player.gameObject.CompareTag("Player"))
		{
			collisionColor.Color = shapeColor;
			Instantiate(collisionStun, this.transform.position, Quaternion.identity);
			StopAllCoroutines();
			StartCoroutine(Stun());
		}

	}

	protected virtual IEnumerator Stun()
	{
		canMove = false;
		this.transform.localScale = startingScale;
		this.fillShape.SetActive(false);
		isShapeActive(false);
		yield return new WaitForSeconds(collisionStunTime);
		canMove = true;
		actionStarted = false;
	}

	private void humanPlayerHitMe(Transform playerHit, Color attackColor)
	{
		if (playerHit.gameObject == this.gameObject)
		{
			//var attackingShot = stunShot.main;
			//attackingShot.startColor = new ParticleSystem.MinMaxGradient(attackColor);
			shotColor.Color = attackColor;
			Instantiate(stunShot, this.transform.position, Quaternion.identity);
			StopAllCoroutines();
			StartCoroutine(Stun());
			IsGreedy = false;
		}
		//this only activates when human player attacks
	}

	private void aiPLayerHitMe(Shape hitPlayer, Color attackColor)
	{
		if (hitPlayer == this)
		{
			//var attackingShot = stunShot.main;
			//attackingShot.startColor = new ParticleSystem.MinMaxGradient(attackColor);
			shotColor.Color = attackColor;
			Instantiate(stunShot, this.transform.position, Quaternion.identity);
			StopAllCoroutines();
			StartCoroutine(Stun());
			IsGreedy = false;

		}

	}

    //Activated when player expands too big
	protected virtual void BeingGreedy(Shape hitPlayer)
	{
		if (OnShoot != null)
        {
            OnShoot(hitPlayer, shapeColor);
        }
	}

}
