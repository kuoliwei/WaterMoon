using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 處理鼻子射線擊中資料，判斷體驗者是否持續靠近銀幕。
/// OnReceiveNoseHits 由 SkeletonDataProcessor 事件呼叫。
/// Update() 會檢查距離上次有效資料的時間間隔。
/// </summary>
public class NoseRayProcessor : MonoBehaviour
{
    [Header("狀態設定")]
    [SerializeField] private float loseTrackThreshold = 0.2f; // 超過此秒數未收到資料 → 離開範圍

    [Header("Debug")]
    [SerializeField] private bool enableDebug = true;

    // 狀態
    private bool isInsideRange = false;
    private float lastReceivedTime = -999f;
    private bool lastState = false; // 上一幀狀態，用來偵測進入/離開變化

    private Vector2 lastValidUV = Vector2.zero; // 可供外部參考目前鼻子位置

    /// <summary>
    /// SkeletonDataProcessor 每幀呼叫：收到鼻子射線命中 UV List。
    /// </summary>
    public void OnReceiveNoseHits(List<Vector2> noseHitList)
    {
        if (noseHitList != null && noseHitList.Count > 0)
        {
            lastReceivedTime = Time.time; // 記錄最後一次收到資料的時間
            lastValidUV = ComputeAverageUV(noseHitList);
        }
    }

    private void Update()
    {
        bool hasRecentData = (Time.time - lastReceivedTime) <= loseTrackThreshold;

        if (hasRecentData)
        {
            isInsideRange = true;
        }
        else
        {
            isInsideRange = false;
        }

        // 偵測狀態變化
        if (isInsideRange != lastState)
        {
            if (isInsideRange)
            {
                if (enableDebug)
                    Debug.Log($"進入體驗範圍 (最近UV: {lastValidUV})");
            }
            else
            {
                if (enableDebug)
                    Debug.Log("離開體驗範圍");
            }
            lastState = isInsideRange;
        }
    }

    private Vector2 ComputeAverageUV(List<Vector2> list)
    {
        if (list == null || list.Count == 0)
            return Vector2.zero;

        Vector2 sum = Vector2.zero;
        foreach (var uv in list)
            sum += uv;
        return sum / list.Count;
    }

    // 讓其他系統可查詢目前狀態
    public bool IsInsideRange => isInsideRange;
    public Vector2 CurrentNoseUV => lastValidUV;
}
