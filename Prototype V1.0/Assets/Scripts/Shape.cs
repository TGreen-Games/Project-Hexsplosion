using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
	public GameObject fillShape;
	public float scaleRate = 0.002f;
	public float scaleSpeed = 0.2f;
	public float collisionStunTime = 5.0f;
	public float shotCooldown = 12.0f;
	public GameObject collisionStun;
	protected RFX4_EffectSettingColor collisionColor;
	//public ParticleSystem stunShot;
	public GameObject stunShot;
	protected RFX4_EffectSettingColor shotColor;
	public Color shapeColor;
	public int score;
	public Text coolDown;
	public Text stunText;
	public int place;
	public bool isPlayer = false;
	public float detectionRadius = 1;
	public bool stunDisabled = false;
	protected int playerLayer = 10;
	protected Vector2 sizeLimiter = new Vector2(0.1608f, 0.1546f);
	protected delegate void ImGreedy(Shape greedyPlayer, bool isGreedy);
	protected static event ImGreedy OnGreed;
	protected bool canShoot = true;
	public bool canMove = true;
	//public bool isStunned = false;
	protected float greedLimiter = 0.2f;
	protected Vector3 startingScale;
	protected List<GameObject> capturedTiles;
	protected bool isGamePaused = false;
	protected bool isGreedy = false;
	protected bool IsGreedy
	{
		get { return isGreedy; }
		set
		{
			if (isGreedy == value) return;
			isGreedy = value;
			if (OnGreed != null)
				OnGreed(this, isGreedy);
		}
	}
	private CircleCollider2D shapeCollision;
	public SpriteRenderer shapeSprite;
    
	// Use this for initialization

	protected virtual void OnEnable()
	{
		shapeColor = this.GetComponent<SpriteRenderer>().color;
		GameManager2.Instance.AddPlayer(shapeColor, this);
		Timer.IsGamePaused += isPaused;
	}

	protected virtual void OnDisable()
	{
		Timer.IsGamePaused -= isPaused;
	}

	protected virtual void Update()
	{
		if (isGamePaused)
			return;
		if (canShoot == false)
		{

			shotCooldown -= Time.deltaTime;
			if (shotCooldown <= 0)
			{
				canShoot = true;
				shotCooldown = 12.0f;
			}

		}

		if (this.transform.localScale.x > greedLimiter && isGreedy == false)
		{
			IsGreedy = true;
		}
		if (shotCooldown == 12.0)
			coolDown.text = "Stun Ready";
		else if (stunDisabled)
		{
			shotCooldown = 100f;
			coolDown.text = "Stun Disabled";
		}
		else
			coolDown.text = Mathf.RoundToInt(shotCooldown).ToString();
	}
	

	protected virtual void Start()
	{
		startingScale = this.transform.localScale;
		capturedTiles = new List<GameObject>();
		shotColor = stunShot.GetComponent<RFX4_EffectSettingColor>();
		collisionColor = collisionStun.GetComponent<RFX4_EffectSettingColor>();
		coolDown.color = shapeColor;
		stunText.enabled = false;
		shapeCollision = this.gameObject.GetComponent<CircleCollider2D>();
		shapeSprite = this.gameObject.GetComponent<SpriteRenderer>();

	}

	protected IEnumerator cooldownTimer(float cooldown)
	{
		yield return new WaitUntil(() => canShoot == true);
		shotCooldown = 12.0f;

	}

	protected virtual void isPaused(bool isPaused)
	{
		isGamePaused = isPaused;
	}

	protected void isShapeActive(bool active)
	{
		shapeSprite.enabled = active;
		shapeCollision.enabled = active;
	}
}




