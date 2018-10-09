using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

	public AudioClip btnSound;

    public void Quit()
	{
		SoundManager.Instance.EffectsSource.PlayOneShot(btnSound);
		Application.Quit();

	}
	public void Retry()
	{
		SoundManager.Instance.EffectsSource.PlayOneShot(btnSound);
		SceneManager.LoadScene((int)Enums.Scenes.Game);
	}

	public void Menu()
	{
		SoundManager.Instance.EffectsSource.PlayOneShot(btnSound);
		SceneManager.LoadScene((int)Enums.Scenes.Start);
	}

    public void Tutorial()
	{
		SoundManager.Instance.EffectsSource.PlayOneShot(btnSound);
		SceneManager.LoadScene((int)Enums.Scenes.Tutorial);
	}
}
