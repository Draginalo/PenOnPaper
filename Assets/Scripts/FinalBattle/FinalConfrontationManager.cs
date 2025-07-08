using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Cinemachine.CinemachineBrain;

public class FinalConfrontationManager : MonoBehaviour
{
    [SerializeField] private GameObject m_Doctor;
    [SerializeField] private GameObject m_WindowSketch;
    [SerializeField] private GameObject m_DoorSketch;
    [SerializeField] private GameObject m_LanternSketch;
    [SerializeField] private GameObject m_FinalSketch;
    private int scriptedNumber = 0;
    private bool confrontationOver = false;
    private Coroutine nextConfrontationEvent;

    //This is to not make a new chain within the functions which will go out of scope after run and deleted (before events run)
    private GameEventChain nextEventsChain = new();

    private void OnEnable()
    {
        EventSystem.OnStartFinalConfrontation += StartFinalConfrontation;
        EventSystem.OnFinishFinalConfrontation += FinishFinalConfrontation;
        EventSystem.OnStopOpening += HandleNextEvent;
        EventSystem.OnRepairLight += HandleNextEvent;
    }

    private void OnDisable()

    {
        EventSystem.OnStartFinalConfrontation -= StartFinalConfrontation;
        EventSystem.OnFinishFinalConfrontation -= FinishFinalConfrontation;
        EventSystem.OnStopOpening -= HandleNextEvent;
        EventSystem.OnRepairLight -= HandleNextEvent;
    }

    private void OpenWindow()
    {
        nextEventsChain.CleanupChain();
        SpawnDoctorWithAnimation newEvent = gameObject.AddComponent<SpawnDoctorWithAnimation>();
        newEvent.animationToTrigger = "OpenWindow";
        newEvent.m_Doctor = m_Doctor;

        ClearNotepad clearEvent = gameObject.AddComponent<ClearNotepad>();
        clearEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.NONE);
        newSketchEvent.SetSketch(m_WindowSketch);

        nextEventsChain.SetEvents(new List<GameEvent> { newEvent, clearEvent, newSketchEvent });

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

    private void BlowOutLight()
    {
        nextEventsChain.CleanupChain();
        //SpawnDoctorWithAnimation newEvent = gameObject.AddComponent<SpawnDoctorWithAnimation>();
        //newEvent.animationToTrigger = "OpenDoor";
        //newEvent.m_Doctor = m_Doctor;

        SetLightIntesityEvent lightEvent = gameObject.AddComponent<SetLightIntesityEvent>();
        lightEvent.curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        lightEvent.curveEvaluationSpeed = 0.2f;
        lightEvent.maxIntensity = 0;
        lightEvent.newRange = 0;

        ClearNotepad clearEvent = gameObject.AddComponent<ClearNotepad>();
        clearEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.NONE);
        newSketchEvent.SetSketch(m_LanternSketch);

        nextEventsChain.SetEvents(new List<GameEvent> { lightEvent, clearEvent, newSketchEvent });

        GameEventManager.instance.LoadAndExecuteEventChain(nextEventsChain);
    }

    private void StartFinalConfrontation()
    {
        nextEventsChain.destroyParentComponent = false;

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        newSketchEvent.SetSketch(m_FinalSketch);
        newSketchEvent.isIndipendent = true;

        newSketchEvent.SetIndipendentEventNotDestroyParent();
        newSketchEvent.enabled = true;
        newSketchEvent.Begin();

        HandleNextEvent();
    }

    private void FinishFinalConfrontation()
    {
        nextEventsChain.CleanupChain();

        confrontationOver = true;

        //To reset window animations and destroy doctor
        EventSystem.StopOpening();

        if (nextConfrontationEvent != null)
        {
            StopCoroutine(nextConfrontationEvent);
        }

        SetLightIntesityEvent lightEvent = gameObject.AddComponent<SetLightIntesityEvent>();
        lightEvent.curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        lightEvent.curveEvaluationSpeed = 0.2f;
        lightEvent.maxIntensity = 0;
        lightEvent.newRange = 0;
        lightEvent.SetIndipendentEventNotDestroyParent();
        lightEvent.enabled = true;
        lightEvent.Begin();

        ClearNotepad clearEvent = gameObject.AddComponent<ClearNotepad>();
        clearEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        clearEvent.SetIndipendentEventNotDestroyParent();
        clearEvent.enabled = true;
        clearEvent.Begin();
    }

    private void HandleNextEvent()
    {
        if (!confrontationOver)
        {
            nextConfrontationEvent = StartCoroutine(Co_PickNextEventWithRandomDelay());
        }
    }

    private IEnumerator Co_PickNextEventWithRandomDelay()
    {
        yield return new WaitForSeconds(Random.Range(3, 9));

        if (scriptedNumber < 3)
        {
            PickEvent(scriptedNumber);
            scriptedNumber++;
        }
        else
        {
            PickEvent(Random.Range(0, 3));
        }
    }

    private void PickEvent(int nextEvent)
    {
        switch (nextEvent)
        {
            case 0:
                OpenWindow();
                break;
            case 1:
                OpenDoor();
                break;
            case 2:
                BlowOutLight();
                break;
        }
    }
}
