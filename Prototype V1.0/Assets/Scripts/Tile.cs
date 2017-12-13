using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    private Color playerColor;
	public static List<GameObject> dirtyTiles;
	// Use this for initialization
	void Start () {
		if (dirtyTiles == null)
			dirtyTiles = new List <GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Fill Shape"))
            playerColor = player.GetComponent<SpriteRenderer>().color;
            if (player.GetComponent<FillShape>().CanCapture)
            {
                Debug.Log("TRIGGERED!!!!!");
			    //dirtyTiles.Add (this.gameObject);
                this.GetComponent<Image>().color = playerColor;
            }
            
        }
    
}
