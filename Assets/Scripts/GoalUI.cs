using System.Collections.Generic;
using UnityEngine;

public class GoalUI : MonoBehaviour
{
    [SerializeField] private TaskElementUI taskElementUI;
    [SerializeField] private AudioClip taskCompletion;
    [SerializeField] private AudioClip goalReach;
    private Dictionary<TileType, TaskElementUI> taskElementUIDictionary = new();

    public void SetupGoal(GoalTracker tracker)
    {
        tracker.ProgressedTask += OnTaskProgressed;
        tracker.CompletedTask += OnTaskCompleted;
        tracker.CompletedGoal += OnGoalCompleted;

        foreach (KeyValuePair<TileType, int> taskEntry in tracker.GoalTrackerDictionary)
        {
            TaskElementUI newTaskElementUI = Instantiate(taskElementUI, transform);
            newTaskElementUI.SetupTask(taskEntry.Key, taskEntry.Value);
            taskElementUIDictionary.Add(taskEntry.Key, newTaskElementUI);
        }
    }


    public void OnTaskProgressed(TileType type, int newAmount)
    {
        taskElementUIDictionary[type].UpdateAmountText(newAmount);
    }


    public void OnTaskCompleted(TileType type)
    {
        taskElementUIDictionary[type].CompleteTask();
        AudioManager.Instance.PlaySoundFX(taskCompletion, transform, 1.0f);
    }


    public void OnGoalCompleted()
    {
        AudioManager.Instance.PlaySoundFX(goalReach, transform, 1.0f);
    }
}
