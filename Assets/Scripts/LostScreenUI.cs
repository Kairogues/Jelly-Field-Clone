using UnityEngine;

public class LostScreenUI : MonoBehaviour
{
    [SerializeField] private Level currentLevel;



    private void Start()
    {
        currentLevel.LostGame += OnGameLost;

        HideUI();
    }


    private void OnGameLost()
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


    public void OnResetLevelClicked()
    {
        // Fade
        GameManager.Instance.ResetLevel();
    }
}
