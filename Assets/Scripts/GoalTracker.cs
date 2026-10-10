using System.Collections.Generic;
using System;
using UnityEngine;

public class GoalTracker : MonoBehaviour
{
    public event Action<TileType, int> ProgressedTask;
    public event Action<TileType> CompletedTask;
    public event Action CompletedGoal;
    private Dictionary<TileType, int> goalTrackerDictionary = new();
    private int currentTaskAmount = 0;

    public Dictionary<TileType, int> GoalTrackerDictionary
    {
        get => goalTrackerDictionary;
    }


    public void SetupGoal(Dictionary<TileType, int> newGoalList)
    {
        goalTrackerDictionary.Clear();
        currentTaskAmount = 0;

        foreach (KeyValuePair<TileType, int> task in newGoalList)
        {
            goalTrackerDictionary.Add(task.Key, task.Value);
            currentTaskAmount += 1;
        }
    }

    public void Contribute(TileType tileType)
    {
        if (!goalTrackerDictionary.TryGetValue(tileType, out int amountLeft))
        {
            return;
        }

        goalTrackerDictionary[tileType] -= 1;

        ProgressedTask?.Invoke(tileType, goalTrackerDictionary[tileType]);

        if (goalTrackerDictionary[tileType] == 0)
        {
            CompletedTask?.Invoke(tileType);
            currentTaskAmount -= 1;

            if (currentTaskAmount == 0)
            {
                CompletedGoal?.Invoke();
            }
        }
    }
}
