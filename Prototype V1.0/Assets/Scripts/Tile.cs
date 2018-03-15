using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    private int fillShapeIndex = 1 << 9;
    public Color PlayerColor { get { return playerColor; } }
    private Color playerColor;
    private Image tileImage;
    private Color originalColor;
	// Use this for initialization

	void Start () {

        tileImage = gameObject.GetComponent<Image>();
        originalColor = tileImage.color;    
	}

    private void OnMouseDown()
    {
        Debug.Log("I'm hit!");
    }

    // Update is called once per frame
    void Update () {
	}

    private void OnTriggerStay2D(Collider2D player)
    {
		if (player.gameObject.CompareTag ("Fill Shape")) 
		{

			if (player.GetComponent<FillShape> ().CanCapture) 
			{
                if(tileImage.color != originalColor)
                {
                    TileManager.instance.MinusTile(this.gameObject,tileImage.color);
                }
                playerColor = player.GetComponent<SpriteRenderer>().color;
				tileImage.color = playerColor;
                TileManager.instance.AddTile(this.gameObject, playerColor);
			}
		}
            
        }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Player")){
    //        dirtyTiles.Add(this.gameObject);
           
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player")){
    //        dirtyTiles.Remove(this.gameObject);
         
    //    }
    //}
}
