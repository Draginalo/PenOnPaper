using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalConfrontationManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Doctor;

    private void OnEnable()
    {
        EventSystem.OnStartFinalConfrontation += OpenWindow;
    }

    private void OnDisable()

    {
        EventSystem.OnStartFinalConfrontation -= OpenWindow;
    }

    private void OpenWindow()
    {
        GameEventChain newChain = new GameEventChain();
        SpawnDoctorWithAnimation newEvent = gameObject.AddComponent<SpawnDoctorWithAnimation>();
        newEvent.animationToTrigger = "OpenWindow";
        newEvent.m_Doctor = m_Doctor;
        newChain.AddEventToEnd(newEvent);
        GameEventManager.instance.LoadAndExecuteEventChain(newChain);
        EventSystem.OpenWindow();
    }
}
