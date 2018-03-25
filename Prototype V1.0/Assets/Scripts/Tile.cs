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

   
    private void OnTriggerStay2D(Collider2D player)
    {
		if (player.gameObject.CompareTag ("Fill Shape")) 
		{

			if (player.GetComponent<FillShape> ().CanCapture) 
			{
                playerColor = player.GetComponent<SpriteRenderer>().color;
                if(tileImage.color == originalColor)
                {
                    TileManager.instance.TileChanged(playerColor);
                }
                else
                {
                    TileManager.instance.TileChanged(tileImage.color, playerColor);
                }
				tileImage.color = playerColor;
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
