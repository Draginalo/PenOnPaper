using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangableDrawHandler : DrawHandler
{
    [SerializeField] private Texture2D sketchToChangeTo;

    protected override void OnEnable()
    {
        base.OnEnable();
        EventSystem.OnChangeSketch += HandleChangeSketch;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EventSystem.OnChangeSketch -= HandleChangeSketch;
    }

    protected override void Start()
    {
        base.Start();
        destroySelf = false;
    }

    private void HandleChangeSketch()
    {
        gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", sketchToChangeTo);
    }
}
