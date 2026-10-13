using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    public event Action LostGame;
    public event Action WonGame;
    public event Action ClickedRetryButton;
    public event Action ClickedNextLevelButton;

    [SerializeField] private Game game;
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private GoalTracker goalTracker;
    [SerializeField] private CellGenerator cellGenerator;
    [SerializeField] private CellSpawner spawner;
    [SerializeField] private GoalUI goalUI;
    //[SerializeField] private LevelData levelData;
    private int currentLevel;
    private bool isPlaying = false;

    public bool IsPlaying
    {
        get => isPlaying;
        set => isPlaying = value;
    }

    public CellGenerator CellGenerator
    {
        get => cellGenerator;
    }



    public void Update()
    {
        if (isPlaying)
        {
            game.Process();
        }
    }


    public void SetUpLevel(LevelData levelData)
    {
        currentLevel = levelData.level;

        goalTracker.SetupGoal(levelData.goals);
        goalUI.SetupGoal(goalTracker);
        gameLogic.CellDataGrid = levelData.levelLayout;
        cellGenerator.SetSeed(currentLevel);

        float zLevel = CellSpawner.Z_LEVEL;

        for (int i = 0; i < levelData.cellSpawnerCount; i++)
        {
            float t = (float)i / (levelData.cellSpawnerCount - 1);
            float x = Mathf.Lerp(CellSpawner.X_LEFT, CellSpawner.X_RIGHT, t);

            Vector3 position = new Vector3(
                x,
                transform.position.y,
                zLevel
            );

            Instantiate(spawner, position, Quaternion.identity, transform);
        }

        gameLogic.PoppedTile += goalTracker.Contribute;
        goalTracker.CompletedGoal += OnGameWon;

        game.StartNewGame();

        isPlaying = true;
    }


    public void OnResetButtonClicked()
    {
        ClickedRetryButton?.Invoke();
    }


    public void OnNextLevelButtonClicked()
    {
        ClickedNextLevelButton?.Invoke();
    }


    private void OnGameWon()
    {
        WonGame?.Invoke();
    }
}
