using UnityEngine;
using TMPro;

public class TextScroller : MonoBehaviour
{
    public RectTransform textTransform;
    public float scrollSpeed = 30f; // Units per second
    public float bottomPadding = 200f; // Adjust as needed for your layout

    private float startY;
    private float targetY;
    private bool isScrolling = true;

    void Start()
    {
        startY = textTransform.anchoredPosition.y;

        // Extend the text container height to add padding at the bottom
        Vector2 size = textTransform.sizeDelta;
        size.y += bottomPadding;
        textTransform.sizeDelta = size;

        // Target is the full scroll length
        targetY = size.y;
    }

    void Update()
    {
        if (!isScrolling) return;

        Vector2 pos = textTransform.anchoredPosition;
        pos.y += scrollSpeed * Time.deltaTime;

        if (pos.y >= targetY)
        {
            pos.y = targetY;
            isScrolling = false;
        }

        textTransform.anchoredPosition = pos;
    }
}
