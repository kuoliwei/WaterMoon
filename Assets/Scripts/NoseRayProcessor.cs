using System.Collections.Generic;
using UnityEngine;

public class NoseRayProcessor : MonoBehaviour
{
    [Header("狀態設定")]
    [SerializeField] private float loseTrackThreshold = 0.2f;

    [Header("Star spawner")]
    [SerializeField] private StarSpawnerUI starSpawnerUI;

    [Header("Range")]
    [SerializeField] private float range = 150f;

    [Header("Speed")]
    [SerializeField] private float speed = 0.5f;

    [Header("Color brightness range")]
    [SerializeField] private float colorBrightnessRangeLow = 0.5f;
    [SerializeField] private float colorBrightnessRangeHigh = 1.0f;

    [Header("Scale range")]
    [SerializeField] private float scaleRangeLow = 0.5f;
    [SerializeField] private float scaleRangeHigh = 2.0f;

    [Header("Debug")]
    [SerializeField] private bool enableDebug = true;

    private bool isInsideRange = false;
    private bool lastState = false;
    private float lastReceivedTime = -999f;
    private List<Vector2> latestNoseHits = new();
    private float spawnTimer = 0f;

    private Canvas targetCanvas;
    private Camera uiCamera;

    private void Start()
    {
        if (starSpawnerUI != null)
        {
            targetCanvas = starSpawnerUI.GetTargetCanvas();
            uiCamera = starSpawnerUI.GetUICamera();
        }
    }

    public void OnReceiveNoseHits(List<Vector2> noseHitList)
    {
        if (noseHitList != null && noseHitList.Count > 0)
        {
            lastReceivedTime = Time.time;
            latestNoseHits.Clear();
            latestNoseHits.AddRange(noseHitList);
        }
    }

    private void Update()
    {
        bool hasRecentData = (Time.time - lastReceivedTime) <= loseTrackThreshold;
        isInsideRange = hasRecentData;

        if (isInsideRange != lastState)
        {
            if (enableDebug)
                Debug.Log(isInsideRange ? "進入體驗範圍" : "離開體驗範圍");
            lastState = isInsideRange;
        }

        if (isInsideRange && latestNoseHits.Count > 0)
        {
            spawnTimer += Time.deltaTime;
            float interval = 1f / Mathf.Max(speed, 0.01f);
            while (spawnTimer >= interval)
            {
                spawnTimer -= interval;
                GenerateStars();
            }
        }
    }

    private void GenerateStars()
    {
        if (starSpawnerUI == null || targetCanvas == null)
            return;

        foreach (var uv in latestNoseHits)
        {
            Vector2 screenCenter = new Vector2(uv.x * Screen.width, uv.y * Screen.height);
            Vector2 offset = Random.insideUnitCircle * range;
            Vector2 screenPos = screenCenter + offset;

            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetCanvas.transform as RectTransform,
                screenPos,
                targetCanvas.renderMode == RenderMode.ScreenSpaceCamera ? uiCamera : null,
                out canvasPos
            );

            float r = Random.Range(colorBrightnessRangeLow, colorBrightnessRangeHigh);
            float g = Random.Range(colorBrightnessRangeLow, colorBrightnessRangeHigh);
            float b = Random.Range(colorBrightnessRangeLow, colorBrightnessRangeHigh);
            Color baseColor = new Color(r, g, b, 1f);
            Color crossColor = Color.white;
            float scale = Random.Range(scaleRangeLow, scaleRangeHigh);

            starSpawnerUI.SpawnStar(canvasPos, baseColor, crossColor, scale);
        }
    }
}
