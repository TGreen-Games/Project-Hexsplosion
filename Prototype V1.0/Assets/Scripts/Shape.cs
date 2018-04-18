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
    public ParticleSystem stunprefab;
    public ParticleSystem stunShot;
    //public ParticleSystem.EmissionModule module;
    public Color shapeColor;
    public int score;
    public int place;
    protected bool canMove = true;
    protected bool isStunned = false;
    protected Vector3 scale;
    protected List<GameObject> capturedTiles;

    // Use this for initialization

    protected void OnEnable()
    {
        shapeColor = this.GetComponent<SpriteRenderer>().color;
        GameManager2.Instance.AddPlayer(shapeColor, this);
        Debug.Log("We runnning now");
    }
    protected void OnDisable()
    {
        
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


}
       



