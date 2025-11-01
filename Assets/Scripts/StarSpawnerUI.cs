using UnityEngine;
using UnityEngine.UI;

public class StarSpawnerUI : MonoBehaviour
{
    [SerializeField] private RectTransform canvasRect;   // Canvas RectTransform
    [SerializeField] private RectTransform parentRect;   // �P�P�n�ͦ��b���� UI �d��]�q�`�N�O Canvas�^
    [SerializeField] private GameObject starPrefab;      // �P�P prefab�]Image + StarController�^

    // ���եΰѼơA�i�b Inspector �վ�
    [SerializeField] private Vector2 screenPos = new Vector2(960, 540);
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color crossColor = Color.white;
    [SerializeField] private float scale = 1f;

    private void Update()
    {
        // ���աG�� S ��ͦ��@���P�P
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnStar(screenPos, baseColor, crossColor, scale);
        }
    }

    /// <summary>
    /// �b���w���ù��y�Хͦ��@���P�P�]Canvas �Ŷ��^
    /// </summary>
    public GameObject SpawnStar(Vector2 screenPos, Color baseColor, Color crossColor, float scale)
    {
        if (starPrefab == null || canvasRect == null)
        {
            Debug.LogWarning("Star prefab or canvas not assigned.");
            return null;
        }

        // �N�ù��y���ন Canvas local position
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            canvasRect.GetComponent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out anchoredPos
        );

        // �ͦ��P�P
        GameObject star = Instantiate(starPrefab, parentRect);
        RectTransform starRect = star.GetComponent<RectTransform>();
        starRect.anchoredPosition = anchoredPos;
        starRect.localScale = new Vector3(scale, scale, 1f);
        starRect.localRotation = Quaternion.identity;

        // �]�w�C��]�Y�P�P prefab �� StarController�^
        // StarSpawnerUI.cs �� SpawnStar(...) ���ͦ���q��

        var sc = star.GetComponent<StarController>();
        if (sc != null)
        {
            sc.SetColors(baseColor, crossColor); // ���T���w shader �� _BaseColor / _CrossColor
        }


        //// �Y�ϥ� Image �����A�i�H�������C��
        //Image img = star.GetComponent<Image>();
        //if (img != null)
        //{
        //    img.color = baseColor;
        //}

        return star;
    }
}
