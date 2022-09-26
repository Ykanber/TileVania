using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] TextMeshProUGUI lifeText; 
    [SerializeField] TextMeshProUGUI scoreText; 

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        DisplayLife();
        DisplayScore();
    }

    void DisplayLife()
    {
        lifeText.text = playerLives.ToString();
    }

    void DisplayScore()
    {
        scoreText.text = score.ToString();
    }

    public void AddToScore(int points)
    {
        score += points;
        DisplayScore();
    }

    public void ProcessPlayerDeath()
    {
        if(playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void TakeLife()
    {
        playerLives -= 1;
        int currentGameScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentGameScene);
        DisplayLife();
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
    }
}
