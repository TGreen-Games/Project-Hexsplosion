using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillShape : MonoBehaviour {
    public float fillRate = 1f;
    private float fillSpeed;
    private Vector3 originalScale;
    private Vector3 currentParentScale;
    public GameObject outlineShape;
    private Color playerColor;
    private bool canCapture;
    public bool CanCapture
    {
        set { canCapture = value; }
        get { return canCapture; }
    }


    private void Awake()
    {
        fillSpeed = fillRate / 1000;
        Debug.Log(outlineShape.name);
        originalScale = this.transform.localScale;     
        this.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerColor = outlineShape.GetComponent<SpriteRenderer>().color;
        this.GetComponent<SpriteRenderer>().color = playerColor;
        canCapture = false;
    }

    // Update is called once per frame
    private void Update()
    {
       StartCoroutine(Fill());
        
    }

    private void OnEnable()
    {

        currentParentScale = outlineShape.transform.localScale;
        this.transform.localScale = originalScale;
        this.transform.position = outlineShape.transform.position;
        canCapture = false;

    }

    IEnumerator Fill()
    {
        while (this.transform.localScale.magnitude.Equals(currentParentScale.magnitude) == false)
        {
            this.transform.localScale = Vector3.MoveTowards(transform.localScale, currentParentScale, fillSpeed * Time.deltaTime);
            yield return null;
        }
        canCapture = true;
        yield return new WaitForFixedUpdate();
        this.gameObject.SetActive(false);
    }

   


}
