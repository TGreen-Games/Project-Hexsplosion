
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver onGameOver;
    public Text timerLabel;
    public float time;
    
    // Use this for initialization
    void Start()
    {
        
        StartCoroutine(timer());
        
    }

    // Update is called once per frame
    void Update()
    {

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
        SceneManager.LoadScene(1);
        Debug.Log("Scene change activated!!!");
    }
}
