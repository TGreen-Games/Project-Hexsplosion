using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

    public void Quit()
	{
		Application.Quit();
	}
	public void Retry()
	{
		
		SceneManager.LoadScene(1);
	}

	public void Menu()
	{
		SceneManager.LoadScene(0);
	}
}
