using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskElementUI : MonoBehaviour
{
    [SerializeField] private Image cubeIcon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image completionTick;



    public void SetupTask(TileType type, int amount)
    {
        if (MaterialLoader.Instance.materialDictionary.TryGetValue(type, out Material material))
        {
            cubeIcon.color = material.color;
        }

        amountText.text = amount.ToString();

        amountText.enabled = true;
        completionTick.enabled = false;
    }


    public void UpdateAmountText(int newAmount)
    {
        if (newAmount < 0)
        {
            return;
        }

        amountText.text = newAmount.ToString();
    }


    public void CompleteTask()
    {
        if (MaterialLoader.Instance.materialDictionary.TryGetValue(TileType.NONE, out Material material))
        {
            cubeIcon.color = material.color;
        }

        amountText.enabled = false;
        completionTick.enabled = true;

        
    }
}