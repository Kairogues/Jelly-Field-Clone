using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class TransitionScreenUI : MonoBehaviour
{
    [SerializeField] private Image transitionImage;
    [SerializeField] private float fadeDuration = 0.5f;

    public async Task StartFadeIn(float duration)
    {
        await FadeIn(duration);
    }

    public async Task StartFadeOut(float duration)
    {
        await FadeOut(duration);
    }

    public async Task StartStayStill(float duration)
    {
        await StayStill(duration);
    }


    private async Task FadeIn(float duration)
    {
        float startAlpha = transitionImage.color.a;
        float targetAlpha = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            Color color = transitionImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            transitionImage.color = color;

            await Task.Yield();
        }

        Color finalColor = transitionImage.color;
        finalColor.a = targetAlpha;
        transitionImage.color = finalColor;
    }


    private async Task FadeOut(float duration)
    {
        float startAlpha = transitionImage.color.a;
        float targetAlpha = 0.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;

            Color color = transitionImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            transitionImage.color = color;

            await Task.Yield();
        }

        Color finalColor = transitionImage.color;
        finalColor.a = targetAlpha;
        transitionImage.color = finalColor;
    }


    private async Task StayStill(float second)
    {
        float elapsed = 0f;

        while (elapsed < second)
        {
            elapsed += Time.unscaledDeltaTime;

            await Task.Yield();
        }
    }
}
