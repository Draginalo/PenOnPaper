using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingMarkerHandler : MonoBehaviour
{
    [SerializeField] private float timeToMoveNextDrawPoint = 0.1f;
    [SerializeField] private AnimationCurve speedCurve;
    private Coroutine markerCoroutine;

    public void HandleMoveNextMarker(Vector3 destination)
    {
        if (markerCoroutine != null)
        {
            StopCoroutine(markerCoroutine);
        }

        markerCoroutine = StartCoroutine(Co_MoveNextDrawPoint(destination));
    }

    private IEnumerator Co_MoveNextDrawPoint(Vector3 destination)
    {
        Vector3 origin = transform.position;
        float currTime = 0;
        while (currTime < timeToMoveNextDrawPoint)
        {
            float t = speedCurve.Evaluate(currTime / timeToMoveNextDrawPoint);
            transform.position = Vector3.Lerp(origin, destination, t);
            currTime += Time.deltaTime;
            yield return null;
        }
    }
}
