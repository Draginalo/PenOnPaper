using System.Collections;
using UnityEngine;

public class SpawnNextSketch : GameEvent
{
    [SerializeField] private GameObject m_NextSketchToSpawn;
    public bool isIndipendent = false;
    [SerializeField] private Texture2D m_Image;

    public void SetSketch(GameObject sketch)
    {
        m_NextSketchToSpawn = sketch;
    }

   public override void Execute()
    {
        base.Execute();

        if (m_Image)
        {
            m_NextSketchToSpawn.GetComponent<SketchImageHandler>().image = m_Image;
        }

        EventSystem.SpawnSketch(m_NextSketchToSpawn, isIndipendent);
        GameEventCompleted(this);
    }
}
