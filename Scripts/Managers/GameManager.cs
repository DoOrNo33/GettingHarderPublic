using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool isPaused;
    private bool isInitialized = false;

    // 이벤트 모음
    public event Action<bool> OnPause;
    public event Action OnGameOver;
    public event Action OnSleep;

    [Header("플레이어 죽음")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    
    public float deathAnimationDuration = 2.0f;

    public ScreenFade screenFade;

    [Header("일시정지")]
    public GameObject pausePanel;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeGameData();

        OnGameOver += EndGame;
        OnPause += GetPauseStatus;
    }

    public void OnGameOverEvent()
    {
        OnGameOver?.Invoke();
    }

    public void OnPauseEvent(bool pause)
    {
        OnPause?.Invoke(pause);
    }

    public void EndGame()
    {
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        gameOverText.text = "YOU DIE";
        gameOverText.gameObject.SetActive(true);

        screenFade.gameObject.SetActive(true);
        screenFade.FadeToBlack();

        // 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(deathAnimationDuration);

        gameOverText.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public bool IsPaused
    {
        get { return isPaused; }
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }

    private void InitializeGameData()
    {
        Time.timeScale = 1f;
        EnvironmentManager.Instance.GenerateForest();
    }

    private void GetPauseStatus(bool pause)
    {
        isPaused = pause;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void SceneLoadBtn()
    {
        EnvironmentManager.Instance.ClearEnvironment();
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public void SceneLoadBtn2()
    {
        EnvironmentManager.Instance.ClearEnvironment();
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
