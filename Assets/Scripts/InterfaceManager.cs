using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;
    [SerializeField] private GoalUI goalUI;

    public GoalUI GoalUI
    {
        get => goalUI;
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


    public void SetupGoalUI(GoalTracker tracker)
    {
        goalUI.SetupGoal(tracker);
    }
}
