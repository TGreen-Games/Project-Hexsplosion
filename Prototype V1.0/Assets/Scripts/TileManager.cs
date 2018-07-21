using UnityEngine;

public class TileManager : MonoBehaviour
{
	public static TileManager Instance
	{
		get { return instance ?? (instance = new GameObject("TileManager").AddComponent<TileManager>()); }
	}
	private static TileManager instance;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void TileChanged(Color previousColor, Color newColor)
	{
		GameManager2.Instance.MinusScore(previousColor, 0);
		GameManager2.Instance.AddScore(newColor, 0);
	}

	public void TileChanged(Color newColor)
	{
		GameManager2.Instance.AddScore(newColor, 0);
	}

	public void AddTile(Color playerColor)
	{

		GameManager2.Instance.AddScore(playerColor, 0);
	}

}

