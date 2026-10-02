using System.Collections.Generic;
using System;

[System.Serializable]
public class GoalTracker
{
    public event Action<TileType, int> ProgressedTask;
    public event Action<TileType> CompletedTask;
    public event Action CompletedGoal;
    private Dictionary<TileType, int> goalTracker = new();
    private int currentTaskAmount = 0;


    public void SetupGoal(Dictionary<TileType, int> newGoalList)
    {
        goalTracker.Clear();
        currentTaskAmount = 0;

        foreach (KeyValuePair<TileType, int> task in newGoalList)
        {
            goalTracker.Add(task.Key, task.Value);
            currentTaskAmount += 1;
        }
    }

    public void Contribute(TileType tileType)
    {
        if (!goalTracker.TryGetValue(tileType, out int amountLeft))
        {
            return;
        }

        goalTracker[tileType] -= 1;

        ProgressedTask?.Invoke(tileType, goalTracker[tileType]);

        if (goalTracker[tileType] == 0)
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
