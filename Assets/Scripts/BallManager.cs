using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    private List<Ball> ballList = new List<Ball>();
    private List<Camera> cameraList = new List<Camera>();
    private int currentBall;
    private int ballCount;
    private void Start()
    { 
        var ballObjects = Object.FindObjectsOfType<Ball>();
        ballList.AddRange(ballObjects);
        foreach (Ball ball in ballObjects)
        {
            cameraList.Add(ball.GetComponentInChildren<Camera>());
      
        }
        ballCount = ballObjects.Length;
        currentBall = ballCount - 1;
    }

    public void ChangeBall(int direction)
    {       
        ballList[currentBall].Stop();
        cameraList[currentBall].enabled = false;
        currentBall = (currentBall + direction + ballCount) % ballCount;
        cameraList[currentBall].enabled = true;
    }
}
