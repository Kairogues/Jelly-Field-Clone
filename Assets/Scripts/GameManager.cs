using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Level playingLevelPrefab;
    [SerializeField] private List<LevelData> levels;
    [SerializeField] private TransitionScreenUI transitionScreenUI;
    private Level playingLevel;
    private int currentLevelNum;

    public CellGenerator CellGenerator
    {
        get => playingLevel.CellGenerator;
    }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(this);
    }


    private void Start()
    {
        currentLevelNum = 0;
        StartLevel(currentLevelNum);
    }


    private void StartLevel(int levelNum)
    {
        playingLevel = Instantiate(playingLevelPrefab);
        playingLevel.SetUpLevel(levels[levelNum]);
        playingLevel.ClickedRetryButton += ResetLevel;
        playingLevel.ClickedNextLevelButton += GoNextLevel;
    }


    public async void ResetLevel()
    {
        await transitionScreenUI.StartFadeIn(0.5f);
        Destroy(playingLevel.gameObject);
        StartLevel(currentLevelNum);
        await transitionScreenUI.StartStayStill(1.0f);
        await transitionScreenUI.StartFadeOut(0.5f);
    }


    public async void GoNextLevel()
    {
        if ((currentLevelNum + 1) != levels.Count)
        {
            currentLevelNum++;
        }

        await transitionScreenUI.StartFadeIn(0.5f);
        Destroy(playingLevel.gameObject);
        StartLevel(currentLevelNum);
        await transitionScreenUI.StartStayStill(1.0f);
        await transitionScreenUI.StartFadeOut(0.5f);
    }
}
