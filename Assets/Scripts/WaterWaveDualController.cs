using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class WaterWaveDualController : MonoBehaviour
{
    [SerializeField] private Material waterMat;

    [Range(1, 8)] public int waveCountX = 3;
    [Range(1, 8)] public int waveCountY = 3;

    [Header("X Direction Waves")]
    public float[] waveFrequenciesX = new float[8];
    public float[] waveSpeedsX = new float[8];
    public float[] waveStrengthsX = new float[8];
    public float[] ampVarSpeedsX = new float[8]; // 震幅變化速度（X）

    [Header("Y Direction Waves")]
    public float[] waveFrequenciesY = new float[8];
    public float[] waveSpeedsY = new float[8];
    public float[] waveStrengthsY = new float[8];
    public float[] ampVarSpeedsY = new float[8]; // 震幅變化速度（Y）

    [Header("Randomization Ranges (X)")]
    public float waveFrequenciesX_Min;
    public float waveFrequenciesX_Max;
    public float waveSpeedsX_Min;
    public float waveSpeedsX_Max;
    public float waveStrengthsX_Min;
    public float waveStrengthsX_Max;
    public float ampVarSpeedsX_Min;
    public float ampVarSpeedsX_Max;

    [Header("Randomization Ranges (Y)")]
    public float waveFrequenciesY_Min;
    public float waveFrequenciesY_Max;
    public float waveSpeedsY_Min;
    public float waveSpeedsY_Max;
    public float waveStrengthsY_Min;
    public float waveStrengthsY_Max;
    public float ampVarSpeedsY_Min;
    public float ampVarSpeedsY_Max;

    [Header("Amplitude Pulse Control")]
    public bool enablePulse = true; // 是否啟用正弦脈動
    public float pulseMultiplier = 1.0f; // 可調整脈動強度

    private static readonly int WaveCountXID = Shader.PropertyToID("_WaveCountX");
    private static readonly int WaveCountYID = Shader.PropertyToID("_WaveCountY");
    private static readonly int WaveFrequenciesXID = Shader.PropertyToID("_WaveFrequenciesX");
    private static readonly int WaveSpeedsXID = Shader.PropertyToID("_WaveSpeedsX");
    private static readonly int WaveStrengthsXID = Shader.PropertyToID("_WaveStrengthsX");
    private static readonly int WaveFrequenciesYID = Shader.PropertyToID("_WaveFrequenciesY");
    private static readonly int WaveSpeedsYID = Shader.PropertyToID("_WaveSpeedsY");
    private static readonly int WaveStrengthsYID = Shader.PropertyToID("_WaveStrengthsY");


    private float[] currentWaveStrengthsX = new float[8];
    private float[] currentWaveStrengthsY = new float[8];
    void Start()
    {
        //if (waterMat == null)
        //    waterMat = GetComponent<Renderer>()?.sharedMaterial;
        RandomizeWaves();
    }

    void Update()
    {
        if (waterMat == null) return;

        // 讓震幅隨時間脈動
        if (enablePulse)
        {
            UpdateWaveAmplitudePulse();
        }

        // 套用到 Shader
        waterMat.SetInt(WaveCountXID, waveCountX);
        waterMat.SetFloatArray(WaveFrequenciesXID, waveFrequenciesX);
        waterMat.SetFloatArray(WaveSpeedsXID, waveSpeedsX);
        waterMat.SetFloatArray(WaveStrengthsXID, currentWaveStrengthsX);

        waterMat.SetInt(WaveCountYID, waveCountY);
        waterMat.SetFloatArray(WaveFrequenciesYID, waveFrequenciesY);
        waterMat.SetFloatArray(WaveSpeedsYID, waveSpeedsY);
        waterMat.SetFloatArray(WaveStrengthsYID, currentWaveStrengthsY);
    }

    // -----------------------------------------------------------
    // 隨機生成波參數
    // -----------------------------------------------------------
    [ContextMenu("Randomize Wave Parameters")]
    public void RandomizeWaves()
    {
        for (int i = 0; i < waveCountX; i++)
        {
            waveFrequenciesX[i] = Random.Range(waveFrequenciesX_Min, waveFrequenciesX_Max);
            waveSpeedsX[i] = Random.Range(waveSpeedsX_Min, waveSpeedsX_Max);
            waveStrengthsX[i] = Random.Range(waveStrengthsX_Min, waveStrengthsX_Max);
            ampVarSpeedsX[i] = Random.Range(ampVarSpeedsX_Min, ampVarSpeedsX_Max);
        }

        for (int i = 0; i < waveCountY; i++)
        {
            waveFrequenciesY[i] = Random.Range(waveFrequenciesY_Min, waveFrequenciesY_Max);
            waveSpeedsY[i] = Random.Range(waveSpeedsY_Min, waveSpeedsY_Max);
            waveStrengthsY[i] = Random.Range(waveStrengthsY_Min, waveStrengthsY_Max);
            ampVarSpeedsY[i] = Random.Range(ampVarSpeedsY_Min, ampVarSpeedsY_Max);
        }

        Debug.Log($"[WaterWaveDualController] Randomized {waveCountX} X-waves & {waveCountY} Y-waves");
    }

    // -----------------------------------------------------------
    // 震幅脈動更新邏輯
    // -----------------------------------------------------------
    private void UpdateWaveAmplitudePulse()
    {
        for (int i = 0; i < waveCountX; i++)
        {
            // 基於時間的正弦波
            float sinValue = Mathf.Sin(2 * Mathf.PI * Time.time * ampVarSpeedsX[i]);
            // 讓值永遠為正（或保留原本正負可選）
            currentWaveStrengthsX[i] = Mathf.Abs(sinValue) * pulseMultiplier * waveStrengthsX[i];
        }

        for (int j = 0; j < waveCountY; j++)
        {
            float sinValue = Mathf.Sin(2 * Mathf.PI * Time.time * ampVarSpeedsY[j]);
            currentWaveStrengthsY[j] = Mathf.Abs(sinValue) * pulseMultiplier * waveStrengthsY[j];
        }
    }
}
