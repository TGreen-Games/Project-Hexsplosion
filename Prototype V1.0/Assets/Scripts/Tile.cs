using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    private Color playerColor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
            playerColor = player.GetComponent<SpriteRenderer>().color;
            if (player.GetComponent<FillShape>().CanCapture)
            {
                Debug.Log("TRIGGERED!!!!!");
                this.GetComponent<Image>().color = playerColor;
            }
            
        }
    
}
