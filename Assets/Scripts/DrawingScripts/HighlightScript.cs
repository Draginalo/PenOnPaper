using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightScript : MonoBehaviour
{
    [SerializeField] private Texture2D texture;
    [SerializeField] private Color highlightColor;
    [SerializeField] private GameObject connectingSketch;
    private bool isActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.GetComponent<Renderer>().material.SetTexture("_BaseTex", texture);
        gameObject.GetComponent<Renderer>().material.SetColor("_HighlightColor", highlightColor);

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

        gameObject.GetComponent<Renderer>().material.SetFloat("_Strength", strength);
    }

    public bool GetIsActive()
    {
        return isActive;
    }
}
