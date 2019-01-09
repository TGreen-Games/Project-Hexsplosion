using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Shape
{
	public Text noActionMarker;
	public AudioClip actionMarkerSound;
	public AudioClip expansionSound;
	public AudioClip fillSound;
	public AudioClip touchSound;
	public delegate void ShootAttack(Transform playerHIt, Color attackColor);
	public static event ShootAttack OnAttacking;
	private delegate void OnStateChange();
	private event OnStateChange onStateChange;
	private Collider2D playerCollider;
	private Enums.PlayerStage state;
	private DigitalRubyShared.FingersScript touchScript;
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
		foreach (DigitalRubyShared.GestureTouch touch in touches)
		{
			return touch;
		}
		return new DigitalRubyShared.GestureTouch();
	}

	// Use this for initialization
	private void Awake()
	{
		detectTouch = this.gameObject.GetComponent<DetectTouch>();
	}

	override protected void OnEnable()
	{
		base.OnEnable();
		detectTouch.onTouch += TouchHandler;
		onStateChange += StateHandler;
		Shape_AI.OnShoot += ImHit;
		touchScript = this.gameObject.GetComponent<DigitalRubyShared.FingersScript>();
	}
	private void OnDestroy()
	{
		detectTouch.onTouch -= TouchHandler;
		onStateChange -= StateHandler;
		Shape_AI.OnShoot -= ImHit;
	}

	protected override void Start()
	{
		
		base.Start();
		state = Enums.PlayerStage.Neutral;
		playerCollider = this.gameObject.GetComponent<Collider2D>();
		noActionMarker.enabled = false;
		isPlayer = true;
	}

	private void TouchHandler(RaycastHit2D playerTouch)
	{
		if (State == Enums.PlayerStage.Neutral)
		{
			if (playerTouch)
			{
				noActionMarker.enabled = false;
				var touchLocation = playerTouch.transform.position;
				var detectedPlayer = isPlayerDetected(touchLocation, detectionRadius);
				if (detectedPlayer != null)
				{
					if (canShoot && canMove)
						attackPlayer(detectedPlayer.transform);
					else
						MoveActionMarker("Too close to another player!");
				}
         
                else if(canMove == false)
				{
					MoveActionMarker("You're stunned!");
				}
                
				else if (playerTouch.collider.gameObject.tag == "Player")
				{
					
					if (canShoot && canMove)
					{
						attackPlayer(playerTouch.transform);
					}
					else
					{
						MoveActionMarker("Stun shot still on cooldown");
						StartCoroutine(cooldownTimer(shotCooldown));
					}

				}
				else if (playerTouch.collider.gameObject.tag == "Tile")
				{
					//SoundManager.Instance.EffectsSource.PlayOneShot(touchSound);
					isShapeActive(true);
					this.transform.position = touchLocation;
					State = Enums.PlayerStage.Expand;
				}

			}
			else
			{
				MoveActionMarker("Try aiming for the center of a tile.");
				Debug.Log("you missed!");
			}
		}
		else if (canMove)
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
				this.transform.localScale = startingScale;
				break;
			case Enums.PlayerStage.Fill:
				SoundManager.Instance.EffectsSource.Stop();
				StartCoroutine(Fill());
				break;
			default:
				break;
		}
	}

	private IEnumerator Expand()
	{
		SoundManager.Instance.EffectsSource.clip = expansionSound;
		SoundManager.Instance.EffectsSource.Play();
		while (State == Enums.PlayerStage.Expand)
		{
			this.transform.localScale += new Vector3(scaleRate, scaleRate) * scaleSpeed;
			yield return new WaitForFixedUpdate();
		}

	}

	private IEnumerator Fill()
	{
		SoundManager.Instance.EffectsSource.clip = fillSound;
		SoundManager.Instance.EffectsSource.Play();
		fillShape.SetActive(true);
		while (fillShape.activeSelf)
		{
			yield return null;
		}
		yield return new WaitForFixedUpdate();
		SoundManager.Instance.EffectsSource.Stop();
        IsGreedy = false;
		State = Enums.PlayerStage.Neutral;


	}

	private void attackPlayer(Transform enemyPlayer)
	{
		if (OnAttacking != null)
        {
            OnAttacking(enemyPlayer, shapeColor);
        }
        canShoot = false;
	}

	private void OnCollisionEnter2D(Collision2D player)
	{
		if (player.gameObject.CompareTag("Player"))
		{
			collisionColor.Color = shapeColor;
			Instantiate(collisionStun, this.transform.position, Quaternion.identity);
			Handheld.Vibrate();
			StopAllCoroutines();
			stunText.color = Color.white;
			stunText.enabled = true;
			StartCoroutine(Stun());
		}
	}
    
	private Collider2D isPlayerDetected(Vector3 position, float radius)
	{
		var hitplayer = Physics2D.OverlapCircle(position, radius,1 << playerLayer );
		if (hitplayer != null)
			return hitplayer;
		else
			return null;
	}

	private IEnumerator Stun()
	{
		SoundManager.Instance.EffectsSource.Stop();
		canMove = false;    
		State = Enums.PlayerStage.Neutral;
		this.fillShape.SetActive(false);
		isShapeActive(false);
		yield return new WaitForSeconds(collisionStunTime);
		stunText.enabled = false;
		canMove = true;
	}

	private void MoveActionMarker(string message)
	{
		noActionMarker.text = message;
		noActionMarker.enabled = true;
		SoundManager.Instance.EffectsSource.PlayOneShot(actionMarkerSound);
	}

	private void ImHit(Shape playerHit, Color attackColor)
	{

		if (playerHit == this)
		{
			//var attackingShot = stunShot.main;
			//attackingShot.startColor = new ParticleSystem.MinMaxGradient(attackColor);
			shotColor.Color = attackColor;
			Instantiate(stunShot, this.transform.position, Quaternion.identity);
			StopAllCoroutines();
			Handheld.Vibrate();
			stunText.GetComponent<Text>().color = attackColor;
			stunText.enabled = true;
			StartCoroutine(Stun());
			IsGreedy = false;
		}
	}

	protected override void isPaused(bool isPaused)
	{
		base.isPaused(isPaused);
		if (isPaused)
			this.touchScript.enabled = false;
		else
			this.touchScript.enabled = true;
			
	}

}