using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchImageHandler : MonoBehaviour
{
    public Texture2D image;
    public Vector3 size;
    [SerializeField] private Material finalCanvasMaterial;

    // Start is called before the first frame update
    void Start()
    {
        if (size != Vector3.zero)
        {
            transform.localScale = size;
        }

        gameObject.GetComponent<Renderer>().material = finalCanvasMaterial;
        gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", image);
    }
}
