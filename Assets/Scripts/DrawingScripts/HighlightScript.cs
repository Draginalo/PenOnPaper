using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    [SerializeField] private HighlightPackage[] materialsToHighlight;
    [SerializeField] private Color highlightColor;
    [SerializeField] private GameObject connectingSketch;
    private bool isActive = false;

    [Serializable]
    private struct HighlightPackage
    {
        public Texture2D texture;
        public Renderer renderer;
    }

    // Start is called before the first frame update
    void Awake()
    {
        foreach (HighlightPackage highlightPackage in materialsToHighlight)
        {
            highlightPackage.renderer.material.SetTexture("_BaseTex", highlightPackage.texture);
            highlightPackage.renderer.material.SetColor("_HighlightColor", highlightColor);
        }

        if (connectingSketch != null)
        {
            DrawHandler connectingSketchScript = connectingSketch.GetComponentInChildren<DrawHandler>();
            DrawingManager.AddObjectToDraw(connectingSketchScript, this);
        }
    }

    public void SetHighlightStrength(float strength)
    {
        if (strength > 0)
        {
            isActive = true;
        }
        else
        {
            isActive = false;
        }

        foreach (HighlightPackage highlightPackage in materialsToHighlight)
        {
            highlightPackage.renderer.material.SetFloat("_Strength", strength);
        }
    }

    public bool GetIsActive()
    {
        return isActive;
    }
}
