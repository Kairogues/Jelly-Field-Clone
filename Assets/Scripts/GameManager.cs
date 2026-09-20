using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private GameLogic gameLogic;



    private void Start()
    {
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
