using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraShake : GameEvent
{
    [SerializeField] private float m_ShakeStrength;
    [SerializeField] private float m_ShakeSpeed;

    public override void Execute()
    {
        base.Execute();
        CameraHandler.instance.InitCameraShake(m_ShakeStrength, m_ShakeSpeed);

        GameEventCompleted(this);
    }
}
