using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class Ball : MonoBehaviour
{
    public TrailRenderer trailRender;
    public Slider slider;
    public float maxSpeed = 10f;
    public string jsonDataPath = "Assets/Resourses/ball_path.json";

    private List<Vector3> trajectoryPoints = new List<Vector3>();
    private Vector3 currentPosition;
    private Vector3 nextPosition;
    private Vector3 starterPosition;
    private int trajectoryCount;
    private int pointCount;
    private int clickCounter;
    private float startTime;
    private float distance;
    private float firstClickTime;
    private float timeBetweenClicks;
    private float speed;
    private bool isMoving;
    private bool isCoroutineAllowed;
    private bool isForsedToStop;
    private void Start()
    {
        speed = maxSpeed / 2;
        firstClickTime = 0f;
        timeBetweenClicks = 0.2f;
        clickCounter = 0;
        isCoroutineAllowed = true;
        isMoving = false;
        GetTrajectory();
        trajectoryCount = 0;
        SetStarterPoint();
    }

    private void GetTrajectory()
    {
        if (File.Exists(jsonDataPath))
        {
            string ballPathJSON = File.ReadAllText(jsonDataPath);
            var point = JSONArray.Parse(ballPathJSON);
            pointCount = point["x"].Count;
            for (int i = 0; i < pointCount; i++)
            {
                trajectoryPoints.Add(new Vector3(point["x"][i], point["y"][i], point["z"][i]));
            }
        }
    }

    private void SetStarterPoint()
    {
        startTime = Time.time;
        currentPosition = transform.position;
        starterPosition = currentPosition;
        distance = Vector3.Distance(starterPosition, trajectoryPoints[trajectoryCount % pointCount]);
    }

    private void Update()
    {
        if (isMoving && speed != 0)
        {
            int index = trajectoryCount % pointCount;
            currentPosition = transform.position;
            nextPosition = trajectoryPoints[index];
            if (currentPosition != nextPosition)
            {
                float distanceCovered = (Time.time - startTime) * speed;
                float journeyFraction = distanceCovered / distance;
                transform.position = Vector3.Lerp(starterPosition, nextPosition, journeyFraction);
            }
            else
            {
                trajectoryCount++;
                CheckEndOfPath();
                SetStarterPoint();

            }
        }
    }

    private void CheckEndOfPath()
    {
        if (trajectoryCount % pointCount == 0 || isForsedToStop)
        {
            isMoving = false;
            isForsedToStop = false;
            transform.position = trajectoryPoints[0];
            if (trailRender != null)
            {
                trailRender.Clear();
            }
        }
    }

    void OnMouseDown()
    {
        isMoving = true;
        clickCounter++;
        if (clickCounter == 1 && isCoroutineAllowed)
        {
            firstClickTime = Time.time;
            StartCoroutine(DoubleClickDetection());
        }
    }

    private IEnumerator DoubleClickDetection()
    {
        isCoroutineAllowed = false;
        while (Time.time - firstClickTime < timeBetweenClicks)
        {
            if (clickCounter == 2)
            {
                isForsedToStop = true;
                CheckEndOfPath();
                trajectoryCount = 0;
                SetStarterPoint();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        clickCounter = 0;
        firstClickTime = 0f;
        isCoroutineAllowed = true;
    }

    public void ChangeSpeed()
    {
        if (slider != null)
        {
            speed = slider.value * maxSpeed;
        }
    }

    public void Stop()
    {
        isMoving = false;
    }

}