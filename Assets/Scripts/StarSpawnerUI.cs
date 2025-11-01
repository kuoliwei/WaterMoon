using UnityEngine;
using UnityEngine.UI;

public class StarSpawnerUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect;   // Canvas RectTransform
    [SerializeField] private RectTransform parentRect;   // 星星要生成在哪個 UI 範圍（通常就是 Canvas）
    [SerializeField] private GameObject starPrefab;      // 星星 prefab（Image + StarController）

    // 測試用參數，可在 Inspector 調整
    [SerializeField] private Vector2 screenPos = new Vector2(960, 540);
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color crossColor = Color.white;
    [SerializeField] private float scale = 1f;

    private void Update()
    {
        // 測試：按 S 鍵生成一顆星星
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnStar(screenPos, baseColor, crossColor, scale);
        }
    }

    /// <summary>
    /// 在指定的螢幕座標生成一顆星星（Canvas 空間）
    /// </summary>
    public GameObject SpawnStar(Vector2 screenPos, Color baseColor, Color crossColor, float scale)
    {
        if (starPrefab == null || canvasRect == null)
        {
            Debug.LogWarning("Star prefab or canvas not assigned.");
            return null;
        }

        // 將螢幕座標轉成 Canvas local position
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvasRect.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out anchoredPos
        );

        // 生成星星
        GameObject star = Instantiate(starPrefab, parentRect);
        RectTransform starRect = star.GetComponent<RectTransform>();
        starRect.anchoredPosition = anchoredPos;
        starRect.localScale = new Vector3(scale, scale, 1f);
        starRect.localRotation = Quaternion.identity;

        // 設定顏色（若星星 prefab 有 StarController）
        // StarSpawnerUI.cs 內 SpawnStar(...) 的生成後段落

        var sc = star.GetComponent<StarController>();
        if (sc != null)
        {
            sc.SetColors(baseColor, crossColor); // 正確指定 shader 的 _BaseColor / _CrossColor
        }


        //// 若使用 Image 版本，可以直接改顏色
        //Image img = star.GetComponent<Image>();
        //if (img != null)
        //{
        //    img.color = baseColor;
        //}

        return star;
    }
}
