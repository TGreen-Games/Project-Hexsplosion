using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shape : MonoBehaviour
{
    public GameObject fillShape;
    public float scaleRate = 0.001f;
    public float scaleSpeed = 0.1f;
    public float collisionStunTime = 5.0f;
    public float shotCooldown = 5.0f;
    public ParticleSystem stunprefab;
    public ParticleSystem stunShot;
    //public ParticleSystem.EmissionModule module;
    public Color shapeColor;
    public int score;
    public int place;
    protected bool canShoot = true;
    protected bool canMove = true;
    protected bool isStunned = false;
    protected Vector3 scale;
    protected List<GameObject> capturedTiles;

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
            Debug.Log("Sorry cant shoot yet. Heres how much time you have left " + shotCooldown);
            if (shotCooldown <= 0){
                canShoot = true;
                shotCooldown = 5.0f;
            }
               
           
                
        }
	}
	protected void Start()
    {
        scale = this.transform.localScale;
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
        shotCooldown = 5;

    }


}
       



