using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalConfrontationManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Doctor;
    [SerializeField] private GameObject m_WindowSketch;
    [SerializeField] private GameObject m_DoorSketch;

    private void OnEnable()
    {
        EventSystem.OnStartFinalConfrontation += OpenDoor;
    }

    private void OnDisable()

    {
        EventSystem.OnStartFinalConfrontation -= OpenDoor;
    }

    private void OpenWindow()
    {
        EventSystem.ClearNotepadPage();
        GameEventChain newChain = new GameEventChain();
        SpawnDoctorWithAnimation newEvent = gameObject.AddComponent<SpawnDoctorWithAnimation>();
        newEvent.animationToTrigger = "OpenWindow";
        newEvent.m_Doctor = m_Doctor;
        newChain.AddEventToEnd(newEvent);

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        newSketchEvent.SetSketch(m_WindowSketch);
        newChain.AddEventToEnd(newSketchEvent);

        GameEventManager.instance.LoadAndExecuteEventChain(newChain);
        EventSystem.OpenWindow();
    }

    private void OpenDoor()
    {
        EventSystem.ClearNotepadPage();
        GameEventChain newChain = new GameEventChain();
        SpawnDoctorWithAnimation newEvent = gameObject.AddComponent<SpawnDoctorWithAnimation>();
        newEvent.animationToTrigger = "OpenDoor";
        newEvent.m_Doctor = m_Doctor;
        newChain.AddEventToEnd(newEvent);

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        newSketchEvent.SetSketch(m_DoorSketch);
        newChain.AddEventToEnd(newSketchEvent);

        GameEventManager.instance.LoadAndExecuteEventChain(newChain);
        EventSystem.OpenHospitalDoor(true);
    }
}
