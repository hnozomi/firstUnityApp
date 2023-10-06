using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;
    [SerializeField] Text scoreText;

    [SerializeField] AudioClip gameClearSE;
    [SerializeField] AudioClip gameOverSE;
    AudioSource audioSource;


    const int MAX_SCORE = 9999;
    int score = 0;

    void Start()
    {
        scoreText.text = score.ToString();
        audioSource = GetComponent<AudioSource>();
            }

    public void AddScore(int val)

    {
        Debug.Log(val);
        score += val; 
        if (score > MAX_SCORE)
        {
            score = MAX_SCORE;
                }

        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        audioSource.PlayOneShot(gameOverSE);
        Invoke("RestartScene", 1.5f);
    }

    public void GameClear()
    {
        gameClearText.SetActive(true);
        audioSource.PlayOneShot(gameClearSE);

        Invoke("RestartScene", 1.5f);
    }

    public void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);

}

}
