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
    [SerializeField] private GameObject m_IntroImage;
    [SerializeField] private GameObject m_PlankBlock;
    [SerializeField] private GameObject m_DoorLock;
    [SerializeField] private FaintEvent loseFaintEvent;
    private int scriptedNumber = 0;
    private bool confrontationOver = false;
    private Coroutine nextConfrontationEvent;
    private Coroutine loseBattleCoroutine;
    private GameObject currDoorLock;
    private GameObject currWindowPlanks;
    private float loseTime = 20;

    //This is to not make a new chain within the functions which will go out of scope after run and deleted (before events run)
    private GameEventChain nextEventsChain = new();

    public enum Issues
    {
        WINDOW,
        DOOR,
        LANTERN,
        ALL
    }

    private void OnEnable()
    {
        EventSystem.OnStartFinalConfrontation += StartFinalConfrontation;
        EventSystem.OnFinishFinalConfrontation += FinishFinalConfrontation;
        EventSystem.OnFixConfrontationIssue += HandleFixIssue;
    }

    private void OnDisable()

    {
        EventSystem.OnStartFinalConfrontation -= StartFinalConfrontation;
        EventSystem.OnFinishFinalConfrontation -= FinishFinalConfrontation;
        EventSystem.OnFixConfrontationIssue -= HandleFixIssue;
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

    private void StartFinalConfrontation(bool intro)
    {
        if (intro)
        {
            StartFinalConfrontation secondFinalConfrontation = gameObject.AddComponent<StartFinalConfrontation>();
            secondFinalConfrontation.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_UP);
            secondFinalConfrontation.intro = false;
            secondFinalConfrontation.SetIndipendentEventNotDestroyParent();
            secondFinalConfrontation.enabled = true;
            secondFinalConfrontation.Begin();

            SpawnNextSketch introTextEvent = gameObject.AddComponent<SpawnNextSketch>();
            introTextEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.NONE);
            introTextEvent.SetSketch(m_IntroImage);
            introTextEvent.isIndipendent = true;
            introTextEvent.SetIndipendentEventNotDestroyParent();
            introTextEvent.enabled = true;
            introTextEvent.Begin();

            ClearNotepad clearEvent = gameObject.AddComponent<ClearNotepad>();
            clearEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
            clearEvent.clearIndipendentSketches = true;
            clearEvent.SetIndipendentEventNotDestroyParent();
            clearEvent.enabled = true;
            clearEvent.Begin();
            return;
        }

        nextEventsChain.destroyParentComponent = false;

        SpawnNextSketch newSketchEvent = gameObject.AddComponent<SpawnNextSketch>();
        newSketchEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        newSketchEvent.SetSketch(m_FinalSketch);
        newSketchEvent.isIndipendent = true;

        newSketchEvent.SetIndipendentEventNotDestroyParent();
        newSketchEvent.enabled = true;
        newSketchEvent.Begin();

        confrontationOver = false;
        loseFaintEvent.SetIndipendentEventNotDestroyParent();

        HandleNextEvent();
    }

    private void FinishFinalConfrontation()
    {
        nextEventsChain.CleanupChain();

        confrontationOver = true;

        //To reset window animations and destroy doctor
        EventSystem.FixConfrontationIssue(Issues.ALL);
        HandleDestroyPotentialFix(Issues.ALL);

        if (nextConfrontationEvent != null)
        {
            StopCoroutine(nextConfrontationEvent);
        }

        if (loseBattleCoroutine != null)
        {
            StopCoroutine(loseBattleCoroutine);
        }

        HandleReturnFaintEffect();

        Destroy(loseFaintEvent);

        SetLightIntesityEvent lightEvent = gameObject.AddComponent<SetLightIntesityEvent>();
        lightEvent.curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        lightEvent.curveEvaluationSpeed = 10f;
        lightEvent.maxIntensity = 0;
        lightEvent.newRange = 0;
        lightEvent.markedAsCompletedTimeOnCurve = 0.1f;
        lightEvent.SetIndipendentEventNotDestroyParent();
        lightEvent.enabled = true;
        lightEvent.Begin();

        ClearNotepad clearEvent = gameObject.AddComponent<ClearNotepad>();
        clearEvent.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        clearEvent.SetIndipendentEventNotDestroyParent();
        clearEvent.enabled = true;
        clearEvent.Begin();
    }

    private IEnumerator Co_DelayHandleLose()
    {
        yield return new WaitForSeconds(loseTime);
        HandleLose();
    }

    private void HandleLose()
    {
        nextEventsChain.CleanupChain();

        confrontationOver = true;

        //To reset window animations and destroy doctor
        EventSystem.FixConfrontationIssue(Issues.ALL);
        HandleDestroyPotentialFix(Issues.ALL);

        if (nextConfrontationEvent != null)
        {
            StopCoroutine(nextConfrontationEvent);
        }

        EventSystem.ClearNotepadPage(true);

        SetLightIntesityEvent lightEvent = gameObject.AddComponent<SetLightIntesityEvent>();
        lightEvent.curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        lightEvent.curveEvaluationSpeed = 5f;
        lightEvent.maxIntensity = 45;
        lightEvent.newRange = 45;

        HandleReturnFaintEffect();

        loseFaintEvent.ResetEvent();

        //scriptedNumber = 0;

        StartFinalConfrontation introFinalConfrontation = gameObject.AddComponent<StartFinalConfrontation>();
        introFinalConfrontation.SetEventTrigger(DrawingManager.DrawingCompleteTrigger.LOOKING_DOWN);
        introFinalConfrontation.intro = true;
        introFinalConfrontation.SetIndipendentEventNotDestroyParent();
        introFinalConfrontation.enabled = true;
        introFinalConfrontation.Begin();
    }

    private void HandleReturnFaintEffect()
    {
        FaintEvent returnFaint = gameObject.AddComponent<FaintEvent>();
        returnFaint.SetCurve(AnimationCurve.EaseInOut(0, loseFaintEvent.GetCurrValue(), 1, 0));
        returnFaint.BlurrMultiple = loseFaintEvent.BlurrMultiple;
        returnFaint.ColorMultiple = loseFaintEvent.ColorMultiple;
        returnFaint.VignetteMultiple = loseFaintEvent.VignetteMultiple;
        returnFaint.CurveEvaluationSpeed = 0.8f;

        loseFaintEvent.ResetEvent();
        loseFaintEvent.enabled = false;

        returnFaint.SetIndipendentEventNotDestroyParent();
        returnFaint.enabled = true;
        returnFaint.Begin();
    }

    private void HandleFixIssue(Issues issueFixed)
    {
        HandleSpawnFix(issueFixed);

        if (!confrontationOver)
        {
            HandleReturnFaintEffect();

            HandleNextEvent();
        }
    }

    private void HandleSpawnFix(Issues issueFixed)
    {
        switch (issueFixed)
        {
            case Issues.DOOR:
                currDoorLock = Instantiate(m_DoorLock);
                break;
            case Issues.WINDOW:
                currWindowPlanks = Instantiate(m_PlankBlock);
                break;
        }
    }

    private void HandleDestroyPotentialFix(Issues issue)
    {
        switch (issue)
        {
            case Issues.DOOR:
                if (currDoorLock != null)
                {
                    Destroy(currDoorLock);
                }
                break;
            case Issues.WINDOW:
                if (currWindowPlanks != null)
                {
                    Destroy(currWindowPlanks);
                }
                break;
            case Issues.ALL:
                HandleDestroyPotentialFix(Issues.DOOR);
                HandleDestroyPotentialFix(Issues.WINDOW);
                break;
        }
    }

    private void HandleNextEvent()
    {
        if (loseBattleCoroutine != null)
        {
            StopCoroutine(loseBattleCoroutine);
        }

        //if (!confrontationOver)
        //{
        nextConfrontationEvent = StartCoroutine(Co_PickNextEventWithRandomDelay());
        //}
    }

    private IEnumerator Co_PickNextEventWithRandomDelay()
    {
        yield return new WaitForSeconds(Random.Range(3, 9));

        loseBattleCoroutine = StartCoroutine(Co_DelayHandleLose());

        loseFaintEvent.enabled = true;
        loseFaintEvent.Begin();

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
        HandleDestroyPotentialFix((Issues)nextEvent);

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
