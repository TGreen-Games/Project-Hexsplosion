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

    // Use this for initialization

    protected void OnEnable()
    {
        shapeColor = this.GetComponent<SpriteRenderer>().color;
    }
    protected void OnDisable()
    {
        
    }
    protected void Start()
    {
        Debug.Log(fillShape);
        scale = this.transform.localScale;
        Debug.Log("Yhis Happened!!!!");
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
       



