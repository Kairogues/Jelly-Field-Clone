using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Game game;
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private CellGenerator cellGenerator;
    [SerializeField] private List<Level> levels;
    private GoalTracker goalTracker = new GoalTracker();
    private int currentLevel = 0;

    public CellGenerator CellGenerator
    {
        get => cellGenerator;
    }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SetUpLevel();
    }


    private void Start()
    {
        gameLogic.PoppedTile += goalTracker.Contribute;
        goalTracker.ProgressedTask += PrintCurrentTask;
        goalTracker.CompletedTask += PrintCompletedTask;
        goalTracker.CompletedGoal += PrintCompletedGoal;


        game.StartNewGame();
    }


    private void PrintCurrentTask(TileType type, int num)
    {
        Debug.Log(type + " progress: " + num);
    }

    private void PrintCompletedTask(TileType type)
    {
        Debug.Log(type + " task completed!");
    }

    private void PrintCompletedGoal()
    {
        Debug.Log("Goal Completed");
    }


    private void Update()
    {
        ProcessGame();
    }


    private void SetUpLevel()
    {
        goalTracker.SetupGoal(levels[currentLevel].goals);
        gameLogic.CellDataGrid = levels[currentLevel].levelLayout;
        cellGenerator.SetSeed(currentLevel);
    }


    private void GoNextLevel()
    {
        if ((currentLevel + 1) != levels.Count)
        {
            currentLevel++;
        }
    }


    private void ProcessGame()
    {
        game.Process();
    }
}
