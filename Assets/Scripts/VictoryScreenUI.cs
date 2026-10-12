using UnityEngine;

public class VictoryScreenUI : MonoBehaviour
{
    [SerializeField] private Level currentLevel;



    private void Start()
    {
        currentLevel.WonGame += OnGameWon;

        HideUI();
    }


    private void OnGameWon()
    {
        ShowUI();
    }


    private void ShowUI()
    {
        gameObject.SetActive(true);
    }

    
    private void HideUI()
    {
        gameObject.SetActive(false);
    }


    public void OnNextLevelClicked()
    {
        // Fade
        GameManager.Instance.GoNextLevel();
    }
}
