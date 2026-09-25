using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Game game;
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private List<Level> levels;
    private GoalTracker goalTracker = new GoalTracker();
    private int currentLevel = 0;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    private void Start()
    {
        goalTracker.SetupGoal(levels[currentLevel].goals);
        gameLogic.SetLevelLayout(levels[currentLevel].levelLayout);
        game.StartNewGame();
    }


    private void Update()
    {
        ProcessGame();
    }


    private void HandleInput()
    {
        
    }


    private void ProcessGame()
    {
        game.Process();
    }
}
