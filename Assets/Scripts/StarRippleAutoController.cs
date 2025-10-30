using UnityEngine;

public class StarRippleAutoController : MonoBehaviour
{
    [SerializeField] private Material rippleMat;

    [Range(1f, 100f)] public float frequency = 20f;
    [Range(0f, 100f)] public float waveSpeed = 2f;
    [Range(0f, 1f)] public float invertDirection = 0f;
    [Range(0f, 1f)] public float alpha = 1f;

    void Update()
    {
        rippleMat.SetFloat("_Frequency", frequency);
        rippleMat.SetFloat("_WaveSpeed", waveSpeed);
        rippleMat.SetFloat("_InvertDirection", invertDirection);
        rippleMat.SetFloat("_Alpha", alpha);
    }
}
