using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Level playingLevelPrefab;
    [SerializeField] private List<LevelData> levels;
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


    public void ResetLevel()
    {
        Destroy(playingLevel.gameObject);
        StartLevel(currentLevelNum);
    }


    private void StartLevel(int levelNum)
    {
        playingLevel = Instantiate(playingLevelPrefab);
        playingLevel.SetUpLevel(levels[levelNum]);
        playingLevel.Reset += ResetLevel;
    }


    public void GoNextLevel()
    {
        if ((currentLevelNum + 1) != levels.Count)
        {
            currentLevelNum++;
        }

        Debug.Log("Hey");

        ResetLevel();
    }
}
