
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{

	public Color PlayerColor { get { return playerColor; } }
	private Color playerColor;
	private Image tileImage;
	private Color originalColor;

	// Use this for initialization
	void Start()
	{

		tileImage = gameObject.GetComponent<Image>();
		originalColor = tileImage.color;
	}


	private void OnTriggerStay2D(Collider2D player)
	{
		if (player.gameObject.CompareTag("Fill Shape"))
		{

			if (player.GetComponent<FillShape>().CanCapture)
			{
				playerColor = player.GetComponent<SpriteRenderer>().color;
				if (tileImage.color == originalColor)
				{
					TileManager.Instance.TileChanged(playerColor);
				}
				else
				{
					TileManager.Instance.TileChanged(tileImage.color, playerColor);
				}
				tileImage.color = playerColor;
			}
		}

	}

}
