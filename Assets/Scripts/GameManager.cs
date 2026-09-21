using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Game game;
    [SerializeField] private GameLogic gameLogic;
    [SerializeField] private List<Level> levels;
    private int currentLevel = 0;



    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        gameLogic.SetLevelLayout(levels[currentLevel]);
        game.StartNewGame();
    }


    private void Update()
    {
        HandleInput();
        ProcessGame();
    }


    private void HandleInput()
    {
        
    }


    private void ProcessGame()
    {
        
    }
}
