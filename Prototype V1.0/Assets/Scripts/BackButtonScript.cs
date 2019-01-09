using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackButtonScript : MonoBehaviour
{

	public Canvas backButtonCanvas;
	public Text boxMessage;

	// Use this for initialization
	void Start()
	{
		backButtonCanvas.enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			BackButtonPressed();
		}
	}

	private void BackButtonPressed()
	{
		Time.timeScale = 0;
		backButtonCanvas.enabled = true;
		if (SceneManager.GetActiveScene().buildIndex == (int)(Enums.Scenes.Start))
		{
			boxMessage.text = "Do you wish to quit the game?";
		}
		else
		{
			boxMessage.text = "Do you wish to go back to the main menu?";
		}
	}

	public void YesButton()
	{
		if (SceneManager.GetActiveScene().buildIndex == (int)(Enums.Scenes.Start))
		{
			Application.Quit();
		}
		else
			SceneManager.LoadScene((int)Enums.Scenes.Start);
	}

    public void NoButton()
	{		
		backButtonCanvas.enabled = false;
		Time.timeScale = 1;
	}
}

