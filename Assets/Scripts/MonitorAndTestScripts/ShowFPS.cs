using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private TextMeshProUGUI FPSText;

    // Start is called before the first frame update
    void Start()
    {
        GameObject fpsCanvasGameobject = new GameObject();
        RectTransform fpsTextRect = fpsCanvasGameobject.AddComponent<RectTransform>();
        fpsTextRect.anchorMax = new Vector2(0, 1);
        fpsTextRect.anchorMin = new Vector2(0, 1);
        FPSText = fpsCanvasGameobject.AddComponent<TextMeshProUGUI>();
        fpsCanvasGameobject.transform.SetParent(canvas.transform);
        fpsTextRect.anchoredPosition = new Vector2(fpsTextRect.sizeDelta.x, -fpsTextRect.sizeDelta.y);

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        StartCoroutine(FramesPerSecond());
    }

    private IEnumerator FramesPerSecond()
    {
        while (true)
        {
            int fps = (int)(1f / Time.deltaTime);
            FPSText.text = Mathf.Ceil(fps).ToString();

            yield return new WaitForSeconds(0.2f);
        }
    }

    // Update is called once per frame
    private void DisplayFPS()
    {
        FPSText.text = Mathf.Ceil(1.0f / Time.deltaTime).ToString();
    }
}
