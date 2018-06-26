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
    public ParticleSystem stunprefab;
    public ParticleSystem stunShot;
    public Color shapeColor;
    public int score;
    public int place;
	protected Vector2 sizeLimiter = new Vector2(0.1608f, 0.1546f);
	protected delegate void ImGreedy( Shape greedyPlayer, bool isGreedy);
	protected static event ImGreedy OnGreed;
    protected bool canShoot = true;
    protected bool canMove = true;
    protected bool isStunned = false;
    protected Vector3 startingScale;
    protected List<GameObject> capturedTiles;
	protected bool isGreedy = false;
	protected bool IsGreedy
    {
		get { return isGreedy; }
        set
        {
			if (isGreedy == value) return;
			isGreedy = value;
			if (OnGreed != null)
				OnGreed(this,isGreedy);
        }
    }

    // Use this for initialization

    protected void OnEnable()
    {
        shapeColor = this.GetComponent<SpriteRenderer>().color;
        GameManager2.Instance.AddPlayer(shapeColor, this);
    }
    protected void OnDisable()
    {
        
    }
	protected void Update()
	{
        if(canShoot == false)
        {
            
            shotCooldown -= Time.deltaTime;
			//Debug.Log( this.transform.parent.name + " Sorry cant shoot yet. Heres how much time you have left " + shotCooldown);
            if (shotCooldown <= 0){
                canShoot = true;
				shotCooldown = 12.0f;
            }          
                
        }

		if(this.transform.localScale.x > 0.2 && isGreedy == false)
		{
			IsGreedy = true;
		}


	}
	protected void Start()
    {
        startingScale = this.transform.localScale;
        capturedTiles = new List<GameObject>();
     
    }


    //protected IEnumerator Stun()
    //{
    //    canMove = false;
    //    this.transform.localScale = scale;
    //    this.fillShape.SetActive(false);
    //    yield return new WaitForSeconds(collisionStunTime);
    //    canMove = true;
    //}
    
    protected IEnumerator cooldownTimer ( float cooldown)
    {
        yield return new WaitUntil(() => canShoot == true);
        shotCooldown = 12.0f;

    }


}
       



