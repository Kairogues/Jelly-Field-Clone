using UnityEngine;

public class BoardCamera : MonoBehaviour
{
    [SerializeField] private int boardWidth = 10;
    [SerializeField] private int boardHeight = 8;

    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float padding = 1f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        FitBoard();
    }

    private void FitBoard()
    {
        float boardWorldWidth = boardWidth * cellSize;
        float boardWorldHeight = boardHeight * cellSize;

        float aspect = (float)Screen.width / Screen.height;

        float sizeFromHeight = boardWorldHeight / 2f;
        float sizeFromWidth = boardWorldWidth / (2f * aspect);

        cam.orthographicSize = Mathf.Max(sizeFromHeight, sizeFromWidth) + padding;
    }
}