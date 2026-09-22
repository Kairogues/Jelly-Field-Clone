using UnityEngine;

public class TestWarp : MonoBehaviour
{
    [SerializeField] private MeshFilter visual;
    [SerializeField] private float duration = 1.0f;

    private float elapsedTime = 0f;
    private float startScaleX = 1;
    private float targetScaleX = 2.0f;
    private bool isScaling = true;
    private float countingDown = 3f;
    private bool once = false;

    private void Update()
    {
        if (countingDown > 0f)
        {
            countingDown -= Time.deltaTime;
            return;
        }

        if (!once)
        {
            transform.position -= new Vector3(0.5f, 0f, 0f);
            visual.gameObject.transform.position -= new Vector3(-0.5f, 0f, 0f);
            once = true;
        }

        if (!isScaling) return;

        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / duration);

        float newX = Mathf.Lerp(startScaleX, targetScaleX, t);

        Vector3 currentScale = transform.localScale;
        transform.localScale = new Vector3(newX, currentScale.y, currentScale.z);

        if (t >= 1.0f)
        {
            isScaling = false;
        }
    }
}