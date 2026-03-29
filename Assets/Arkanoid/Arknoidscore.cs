using UnityEngine;
using TMPro;
using System;

public class Arknoidscore : MonoBehaviour
{
    private int score;
    private int blocksdestroyed;

    [SerializeField]
    private TMP_Text seenscore;
    [SerializeField]
    private TMP_Text blockline;
    [SerializeField]
    private TMP_Text gameover; 
    [SerializeField]
    private int newblockline = 15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        blocksdestroyed = 0;
        UpdateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // get the score 
    public int GetScore()
    {
        return score;
    }

    // add points to score
    public void addScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // subtract points from score
    public void subtractScore(int points)
    {
        if (score - points < 0)
        {
            score = 0;
            UpdateScoreText();
            Debug.Log("Game Over!");
        }
        else
        {
            score -= points;
            UpdateScoreText();
        }
        
    }

    // block was destroyed
    public void adddestroyedBlock()
    {
        blocksdestroyed++;
        UpdateScoreText();
    }

    // reset destroyedblock
    public void setzerodestroyed()
    {
        blocksdestroyed = 0;
        UpdateScoreText();
    }

    // boolean for new blockline spawn
    public Boolean newBlockline()
    {
        return blocksdestroyed >= newblockline;
    }

    // rewrite the score
    void UpdateScoreText()
    {
        seenscore.text = "Score :" + score.ToString();
        blockline.text = "next Line:" + blocksdestroyed.ToString() + "/10";
    }

    // game over
    public void Gameover()
    {
        gameover.text = "Game Over" + "\nEndscore: " + score.ToString();
        seenscore.text = "";
        blockline.text = "";
    }

    // winning
    public void winning()
    {
        gameover.text = "You WIN" + "\nEndscore: " + score.ToString();
        seenscore.text = "";
        blockline.text = "";
    }
}
