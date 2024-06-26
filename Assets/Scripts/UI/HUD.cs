﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Text currentScoreText;

    [SerializeField]
    private Text bestScoreText;

    private Animator anim;           
    private int slideInHash;       
    private int slideOutHash;

    void Start()
    {
        anim = GetComponent<Animator>();
        slideInHash = Animator.StringToHash("SlideIn");
        slideOutHash = Animator.StringToHash("SlideOut");

        //Nghe hàm cập nhật điểm
        ScoreManager.Instance.CurrentScoreUpdatedEvent.AddListener(OnCurrentScoreUpdated);
        ScoreManager.Instance.BestScoreUpdatedEvent.AddListener(OnBestScoreUpdated);

        //Sinh điểm cao nhất khi bắt đầu game
        bestScoreText.text = ScoreManager.Instance.BestScore.ToString();
    }

    private void OnCurrentScoreUpdated()
    {
        currentScoreText.text = ScoreManager.Instance.CurrentScore.ToString();
    }

    private void OnBestScoreUpdated()
    {
        bestScoreText.text = ScoreManager.Instance.BestScore.ToString();
    }

    public void OnPause()
    {
        if (GameManager.Instance.gameState == GameState.Playing)
        {
            Time.timeScale = (Time.timeScale == 1f) ? 0f : 1f;
        }
    }

    public void SlideOut()
    {
        anim.SetTrigger(slideOutHash);
    }

    public void SlideIn()
    {
        anim.SetTrigger(slideInHash);
    }
}
