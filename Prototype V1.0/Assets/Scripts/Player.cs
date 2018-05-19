using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Shape
{
    //public Text stunText;


    public delegate void ShootAttack(RaycastHit2D playerHIt, Color attackColor);
    public static event ShootAttack OnAttacking;
    private delegate void OnStateChange();
    private event OnStateChange onStateChange;
    private Collider2D playerCollider;
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
        foreach (DigitalRubyShared.GestureTouch touch in touches)
        {
            return touch;
        }
        return new DigitalRubyShared.GestureTouch();
    }
    //public delegate int BoardChanged(int playerScore, Color playerColor);
    //public static event BoardChanged onCapture;


    // Use this for initialization
     private void Awake()
    {
        detectTouch = this.gameObject.GetComponent<DetectTouch>();
    }
    new private void OnEnable()
    {
        base.OnEnable();
        detectTouch.onTouch += TouchHandler;
        onStateChange += StateHandler;
        Shape_AI.OnShoot += ImHit;
    }
    new private void OnDisable()
    {
        base.OnDisable();
        detectTouch.onTouch -= TouchHandler;
        onStateChange -= StateHandler;
    }
    new void Start()
    {
        base.Start();
        state = Enums.PlayerStage.Neutral;
        playerCollider = this.gameObject.GetComponent<Collider2D>();
        //TileManager.instance.AddColor(this.shapeColor);


    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void TouchHandler(RaycastHit2D playerTouch)
    {
        if (State == Enums.PlayerStage.Neutral && canMove)
        {
            if (playerTouch)
            {

            if (playerTouch.collider.gameObject.tag == "Player")
                {
                    if (canShoot)
                    {
                        OnAttacking(playerTouch, shapeColor);
                        Debug.Log(playerTouch);
                        Shape_AI enemyPlayer = playerTouch.collider.GetComponent<Shape_AI>();
                        Instantiate(stunShot, playerTouch.transform.position, Quaternion.identity);
                        canShoot = false;
                        //enemyPlayer.Stun();
                    }
                    else
                    {
                        
                        StartCoroutine(cooldownTimer(shotCooldown));
                    }
                        
                }
                else if (playerTouch.collider.gameObject.tag == "Tile")
                {
                    this.transform.position = playerTouch.transform.position;
                    State = Enums.PlayerStage.Expand;
                }

            }
            else
            {
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
        //score = onCapture(score, shapeColor);

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

    private IEnumerator Stun()
    {
        canMove = false;
        State = Enums.PlayerStage.Neutral;
        this.fillShape.SetActive(false);
        yield return new WaitForSeconds(collisionStunTime);
        canMove = true;
    }
    
    private void ImHit(Shape playerHit, Color attackColor)
    {

        if (playerHit == playerCollider)
        {
            var attackingShot = stunShot.main;
            attackingShot.startColor = new ParticleSystem.MinMaxGradient(attackColor);
			Instantiate(stunShot, this.transform.position, Quaternion.identity);
            StopAllCoroutines();
            StartCoroutine(Stun());
        }
    }
} 