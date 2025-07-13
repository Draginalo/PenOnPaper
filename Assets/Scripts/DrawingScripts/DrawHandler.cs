using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.VFX;

public class DrawHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private int totalPixelsX = 1024;
    [SerializeField] private int totalPixelsY = 512;
    [SerializeField] private int brushSize = 8;
    [SerializeField] private Color brushColor = Color.black;

    [SerializeField] private Material drawingCanvasMaterial;
    [SerializeField] private Material finalCanvasMaterial;

    private Texture2D generatedTexture;
    private Material generatedMaterial;
    [SerializeField] private Texture2D currDrawTemplateTexture;
    [SerializeField] private Texture2D finalSketchTexture;

    private Color[] colorMap;

    [SerializeField] private Transform _TopLeftPoint;
    [SerializeField] private Transform _BottomRightPoint;
    [SerializeField] private Transform drawPoint;

    [SerializeField] private float _RaycastDistence;

    private int currXPixel = 0;
    private int currYPixel = 0;
    private float xDrawMult = 0;
    private float yDrawMult = 0;

    private bool pressedLastFrame = false;
    private bool startedDrawing = false;
    private Vector2Int lastPoint = Vector2Int.zero;

    [SerializeField] private SplineContainer drawSpline;
    private float minDistToSplinePoints = 0.0025f;
    private int startingKnotCount;
    private Vector2Int lastSplinePoint;
    private Vector2Int secondLastSplinePoint;

    [SerializeField] private GameObject _NextPointMarker;
    private DrawingMarkerHandler nextPointMarker;

    [SerializeField] private GameObject _CompleteVFX;

    [Tooltip("The game events to occure following the completion of the sketch")]
    [SerializeField] private GameEventChain followingGameEvents;

    [SerializeField] private bool triggerSketchCompletion = true;
    [SerializeField] private WhenToHandleFollowingEvents handleFollowingEvents = WhenToHandleFollowingEvents.AFTER_WHOLE_COMPLETION_VFX;
    private bool finishedDrawing = false;

    public enum WhenToHandleFollowingEvents
    {
        IMEDIATELY,
        AFTER_FLASH,
        AFTER_WHOLE_COMPLETION_VFX
    }

    public Camera MainCam { set { cam = value; } }
    //public DrawingManager.DrawingCompleteTrigger CompletionTrigger { get { return completeTrigger; } }

    private void OnEnable()
    {
        EventSystem.OnDeactivateAllOtherSketches += HandleOnDisable;
    }

    private void OnDisable()
    {
        EventSystem.OnDeactivateAllOtherSketches -= HandleOnDisable;
    }

    private void HandleOnDisable(DrawHandler initiator)
    {
        if (this != initiator)
        {
            if (nextPointMarker != null)
            {
                Destroy(nextPointMarker.gameObject);
            }

            this.enabled = false;
        }
    }

    private void Start()
    {
        colorMap = new Color[totalPixelsX * totalPixelsY];

        generatedTexture = new Texture2D(totalPixelsY, totalPixelsX, TextureFormat.RGBA32, false);
        generatedTexture.filterMode = FilterMode.Point;

        generatedMaterial = new Material(drawingCanvasMaterial);
        generatedMaterial.SetTexture("_BaseMap", generatedTexture);

        gameObject.GetComponent<MeshRenderer>().material = generatedMaterial;

        xDrawMult = totalPixelsX / (_BottomRightPoint.localPosition.x - _TopLeftPoint.localPosition.x);
        yDrawMult = totalPixelsY / (_BottomRightPoint.localPosition.y - _TopLeftPoint.localPosition.y);

        nextPointMarker = Instantiate(_NextPointMarker).GetComponent<DrawingMarkerHandler>();
        nextPointMarker.transform.SetParent(transform);
        nextPointMarker.transform.position = (Vector3)(drawSpline.Spline[0].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position;

        startingKnotCount = drawSpline.Spline.Count;
        Vector3 mappedLastPoint = transform.parent.InverseTransformPoint((Vector3)(drawSpline.Spline[drawSpline.Spline.Count - 1].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position);
        Vector3 mapped2NDLastPoint = transform.parent.InverseTransformPoint((Vector3)(drawSpline.Spline[drawSpline.Spline.Count - 2].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position);
        lastSplinePoint = new Vector2Int((int)((mappedLastPoint.x - _TopLeftPoint.localPosition.x) * xDrawMult), (int)((mappedLastPoint.y - _TopLeftPoint.localPosition.y) * yDrawMult));
        secondLastSplinePoint = new Vector2Int((int)((mapped2NDLastPoint.x - _TopLeftPoint.localPosition.x) * xDrawMult), (int)((mapped2NDLastPoint.y - _TopLeftPoint.localPosition.y) * yDrawMult));

        ClearDrawTexture();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && !finishedDrawing)
        {
            CalculatePixelToBeDrawnAt();
        }
        else
        {
            pressedLastFrame = false;
        }
    }

    private void CalculatePixelToBeDrawnAt()
    {
        Ray drawRay = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(drawRay, out hit, _RaycastDistence))
        {
            if (hit.collider.gameObject == gameObject)
            {
                drawPoint.position = hit.point;
                currXPixel = (int)((drawPoint.localPosition.x - _TopLeftPoint.localPosition.x) * xDrawMult);
                currYPixel = (int)((drawPoint.localPosition.y - _TopLeftPoint.localPosition.y) * yDrawMult);

                //Gets the flipped horizontal X pixel on the texure because the outline texture is read differently than the outline
                int flippedCurrXPixel = totalPixelsX - currXPixel;

                //Checks if you can draw there based on template texture
                if (currDrawTemplateTexture.GetPixel(flippedCurrXPixel, currYPixel) != Color.clear)
                {
                    //Handles logic if starting to draw for the first time
                    if (!startedDrawing)
                    {
                        if (StartDrawingSplineCheck())
                        {
                            HandleDrawingLogic();
                            return;
                        }
                    }

                    //Handles logic if continueing the drawing 
                    if (pressedLastFrame || colorMap[currXPixel * totalPixelsY + currYPixel] != Color.clear)
                    {
                        HandleDrawingLogic();
                        return;
                    }
                }
            }
        }

        pressedLastFrame = false;
    }

    private void DrawAndSetTexure()
    {
        if (pressedLastFrame && (lastPoint.x != currXPixel || lastPoint.y != currYPixel))
        {
            int dist = (int)Mathf.Sqrt((currXPixel - lastPoint.x) * (currXPixel - lastPoint.x) + (currYPixel - lastPoint.y) * (currYPixel - lastPoint.y));
            for (int i = 1; i <= dist; i++)
            {
                DrawPixels((i * currXPixel + (dist - i) * lastPoint.x) / dist, (i * currYPixel + (dist - i) * lastPoint.y) / dist);
            }
        }
        else
        {
            DrawPixels(currXPixel, currYPixel);
        }

        pressedLastFrame = true;
        lastPoint = new Vector2Int(currXPixel, currYPixel);
        SetDrawingTexture();
    }

    private void DrawPixels(int xPixel, int yPixel)
    {
        int i = xPixel - brushSize + 1;
        int j = yPixel - brushSize + 1;
        int maxI = xPixel + brushSize - 1;
        int maxJ = yPixel + brushSize - 1;

        if (i < 0)
        {
            i = 0;
        }
        if (j < 0)
        {
            j = 0;
        }

        if (maxI >= totalPixelsX)
        {
            maxI = totalPixelsX - 1;
        }
        if (maxJ >= totalPixelsY)
        {
            maxJ = totalPixelsY - 1;
        }

        for (int x = i; x <= maxI; x++)
        {
            for (int y = j; y <= maxJ; y++)
            {
                if ((x - xPixel) * (x - xPixel) + (y - yPixel) * (y - yPixel) <= brushSize * brushSize)
                {
                    colorMap[x * totalPixelsY + y] = brushColor;
                }
            }
        }
    }

    private void SetDrawingTexture()
    {
        generatedTexture.SetPixels(colorMap);
        generatedTexture.Apply();
    }

    private void ClearDrawTexture()
    {
        for (int i = 0; i < colorMap.Length; i++)
        {
            colorMap[i] = new Color(0, 0, 0, 0);
        }

        SetDrawingTexture();
    }

    private void HandleSplineChecking()
    {
        if (drawSpline.Spline.Count != 0)
        {
            //Checks if close enough to starting point
            Vector3 worldSpaceKnotLocation = (Vector3)(drawSpline.Spline[0].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position;
            Vector3 diference = worldSpaceKnotLocation - drawPoint.position;
            if (diference.sqrMagnitude <= minDistToSplinePoints)
            {
                drawSpline.Spline.RemoveAt(0);
                if (drawSpline.Spline.Count != 0)
                {
                    nextPointMarker.HandleMoveNextMarker((Vector3)(drawSpline.Spline[0].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position);
                }
            }
            else
            {
                //Checks if close enough to ending point
                //worldSpaceKnotLocation = (Vector3)(drawSpline.Spline[drawSpline.Spline.Count - 1].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position;
                //diference = worldSpaceKnotLocation - drawPoint.position;
                //if (diference.sqrMagnitude <= minDistToSplinePoints && startingKnotCount - drawSpline.Spline.Count > 2)
                //{
                //    drawSpline.Spline.RemoveAt(drawSpline.Spline.Count - 1);
                //    if (drawSpline.Spline.Count != 0)
                //    {
                //        nextPointMarker.HandleMoveNextMarker((Vector3)(drawSpline.Spline[drawSpline.Spline.Count - 1].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position);
                //    }
                //}
            }

            if (drawSpline.Spline.Count == 0)
            {
                //Finished drawing
                HandleCompletion();
            }
        }
    }

    private void HandleDrawingLogic()
    {
        DrawAndSetTexure();
        HandleSplineChecking();
        startedDrawing = true;
        return;
    }

    private void HandleDrawLastSegment()
    {
        int dist = (int)Mathf.Sqrt((lastSplinePoint.x - secondLastSplinePoint.x) * (lastSplinePoint.x - secondLastSplinePoint.x) + (lastSplinePoint.y - secondLastSplinePoint.y) * (lastSplinePoint.y - secondLastSplinePoint.y));
        for (int i = 1; i <= dist; i++)
        {
            DrawPixels((i * lastSplinePoint.x + (dist - i) * secondLastSplinePoint.x) / dist, (i * lastSplinePoint.y + (dist - i) * secondLastSplinePoint.y) / dist);
        }
        SetDrawingTexture();
    }

    protected virtual void HandleCompletion()
    {
        HandleDrawLastSegment();

        Destroy(nextPointMarker.gameObject);
        Destroy(gameObject.GetComponent<MeshCollider>());
        finishedDrawing = true;

        VisualEffect vfx = Instantiate(_CompleteVFX, transform.parent).GetComponent<VisualEffect>();
        vfx.SetTexture("DrawTexture", generatedTexture);
        vfx.SetFloat("SketchSizeOffset", transform.parent.localScale.x / 3.0f);
        vfx.transform.localEulerAngles += new Vector3(0, 90, -90);
        vfx.transform.localScale = new Vector3(3.0f / transform.parent.localScale.x, 3.0f / transform.parent.localScale.y, 3.0f / transform.parent.localScale.z);

        if (handleFollowingEvents == WhenToHandleFollowingEvents.IMEDIATELY)
        {
            HandleGameEvents();
        }

        StartCoroutine(Co_DelayFinalSketchChange(vfx.GetFloat("Delay")));
        StartCoroutine(Co_DelaySketchChangeVFXDone(vfx.GetFloat("Delay") - vfx.GetFloat("BeforeSpawnTime") + vfx.GetFloat("FinalMaxLifetime")));
    }

    private IEnumerator Co_DelayFinalSketchChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<Renderer>().material = finalCanvasMaterial;
        gameObject.GetComponent<Renderer>().material.SetTexture("_BaseMap", finalSketchTexture);
        gameObject.transform.localEulerAngles += new Vector3(-90, 0, 0);

        if (handleFollowingEvents == WhenToHandleFollowingEvents.AFTER_FLASH)
        {
            HandleGameEvents();
        }
    }

    private IEnumerator Co_DelaySketchChangeVFXDone(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (handleFollowingEvents == WhenToHandleFollowingEvents.AFTER_WHOLE_COMPLETION_VFX)
        {
            HandleGameEvents();
        }

        Destroy(this);
    }

    private void HandleGameEvents()
    {
        if (followingGameEvents.ComponentParent != null)
        {
            if (triggerSketchCompletion)
            {
                TriggerNextEventChain triggerNextEventChain = followingGameEvents.ComponentParent.AddComponent<TriggerNextEventChain>();
                followingGameEvents.AddEventToEnd(triggerNextEventChain);
            }

            GameEventManager.instance.LoadAndExecuteEventChain(followingGameEvents);

            //Attaches game event components to game event manager to make sure the components don't get destroyed
            // the following makes sure the drawing object does not get moved
            if (followingGameEvents.ComponentParent != gameObject)
            {
                followingGameEvents.ComponentParent.transform.SetParent(GameEventManager.instance.transform);
            }

            return;
        }

        EventSystem.TriggerNextEventChain();
    }

        private bool StartDrawingSplineCheck()
    {
        //Checks if close enough to starting point
        Vector3 worldSpaceKnotLocation = (Vector3)(drawSpline.Spline[0].Position * drawSpline.transform.lossyScale.x) + drawSpline.transform.position;
        Vector3 diference = worldSpaceKnotLocation - drawPoint.position;

        return diference.sqrMagnitude <= minDistToSplinePoints;
    }

    private void OnDestroy()
    {
        Destroy(generatedMaterial);
        Destroy(generatedTexture);
    }
}
