
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
            var minutes = time / 60;
            var seconds = time % 60;
            timerLabel.text = string.Format("{0:00} : {1:00}", minutes, seconds);
            yield return new WaitForSeconds(1);
        }
        onGameOver();
        Debug.Log("Scene change activated!!!");
    }
}
