using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalConfrontationManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Doctor;
    [SerializeField] private GameObject m_WindowSketch;
    [SerializeField] private GameObject m_DoorSketch;

    //This is to not make a new chain within the functions which will go out of scope after run and deleted (before events run)
    private GameEventChain nextEventsChain = new();

    private void OnEnable()
    {
        EventSystem.OnStartFinalConfrontation += OpenDoor;
    }

    private void OnDisable()

    {
        EventSystem.OnStartFinalConfrontation -= OpenDoor;
    }

    private void Start()
    {
        nextEventsChain.SetEvents(new List<GameEvent> { });
    }
    private void Update()
    {
        Debug.Log(nextEventsChain.GetEventChain().Count);
    }

    private void OpenWindow()
    {
        nextEventsChain.CleanupChain();
        SpawnDoctorWithAnimation newEvent = gameObject.AddComponent<SpawnDoctorWithAnimation>();
        newEvent.animationToTrigger = "OpenWindow";
        newEvent.m_Doctor = m_Doctor;
        nextEventsChain.AddEventToEnd(newEvent);

        ClearNotepad clearEvent = gameObject.AddComponent<ClearNotepad>();
        clearEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        nextEventsChain.AddEventToEnd(clearEvent);

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.NONE);
        newSketchEvent.SetSketch(m_WindowSketch);
        nextEventsChain.AddEventToEnd(newSketchEvent);

        GameEventManager.instance.LoadAndExecuteEventChain(nextEventsChain);
        EventSystem.OpenWindow();
    }

    private void OpenDoor()
    {
        nextEventsChain.CleanupChain();
        SpawnDoctorWithAnimation newEvent = gameObject.AddComponent<SpawnDoctorWithAnimation>();
        newEvent.animationToTrigger = "OpenDoor";
        newEvent.m_Doctor = m_Doctor;

        ClearNotepad clearEvent = gameObject.AddComponent<ClearNotepad>();
        clearEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.NONE);
        newSketchEvent.SetSketch(m_DoorSketch);

        nextEventsChain.SetEvents(new List<GameEvent> { newEvent, clearEvent, newSketchEvent });

        GameEventManager.instance.LoadAndExecuteEventChain(nextEventsChain);
        EventSystem.OpenHospitalDoor(true);
    }
}
