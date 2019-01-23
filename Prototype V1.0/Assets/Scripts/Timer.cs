
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
	public AudioClip cdwnTimerSound;
	public AudioClip testSound;
	public delegate void GameOver();
	public static event GameOver onGameOver;
	public Text timerLabel;
	public float time;
	public GameObject countDownObject;
	private Color[] colors;
	private Text countDownText;
	public delegate void PauseGame(bool isPaused);
    public static event PauseGame IsGamePaused;

	// Use this for initialization
	void Start()
	{
		countDownText = countDownObject.GetComponent<Text>();
		StartCoroutine(countdownTimer());
        
	}

	IEnumerator timer()
	{

		while (time > 0)
		{
			time--;
			int minutes = (int)time / 60;
			if (time <= 60)
				minutes = 0;
			var seconds = time % 60;
			if (minutes < 1)
				timerLabel.text = seconds.ToString();
			timerLabel.text = string.Format("{0:00} : {1:00}", minutes, seconds);
			yield return new WaitForSeconds(1);
		}
		SceneManager.LoadScene(2);
	}

	private IEnumerator countdownTimer()
    {
        IsGamePaused(true);
        var i = 3;
        float seconds = 1.0f;
		countDownObject.SetActive(true);
  
		SoundManager.Instance.EffectsSource.PlayOneShot(testSound);
		foreach (Color playerColor in GameManager2.Instance.players.Keys)
        {
            countDownText.color = playerColor;
            if (i == 0)
            {
                countDownText.text = "GO!";
                Debug.Log("this is went off");
            }
            else
            {
                countDownText.text = i.ToString();
				//SoundManager.Instance.EffectsSource.PlayOneShot(cdwnTimerSound);
                Debug.Log("countdown active");
            }
            i--;
            yield return new WaitForSeconds(seconds);

        }
        countDownObject.SetActive(false);
		Debug.Log("we here");
		IsGamePaused(false);
		StartCoroutine(timer());

    }
}
