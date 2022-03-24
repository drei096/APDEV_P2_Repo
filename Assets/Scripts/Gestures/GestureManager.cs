using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance;
    public SwipeProperty _swipeProperty;
    public SpreadProperty _spreadProperty;
    public RotateProperty _rotateProperty;
    public EventHandler<SwipeEventArgs> OnSwipe;
    public EventHandler<SpreadEventArgs> OnSpread;
    public EventHandler<RotateEventArgs> OnRotate;
    public GameHandler gameHandler;

    Touch trackedFinger1;
    Touch trackedFinger2;
    
    private Vector2 startPoint = Vector2.zero;
    private Vector2 endPoint = Vector2.zero;
    private float gestureTime = 0;

    private bool isFingerUp = true;
    private bool isTwoFingersUp = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.touchCount > 0)
        {
          
            if (Input.touchCount == 1)
            {
                checkSingleFingerGestures();
            }

            else if (Input.touchCount == 2)
            {
                checkTwoFingerGestures();
            }
        }
        else
        {
            isFingerUp = true;
            isTwoFingersUp = true;
        }
    }

    private void checkSingleFingerGestures()
    {
        trackedFinger1 = Input.GetTouch(0);

        if (trackedFinger1.phase == TouchPhase.Began)
        {
            startPoint = trackedFinger1.position;
            gestureTime = 0;
        }
        else if (trackedFinger1.phase == TouchPhase.Ended)
        {
            endPoint = trackedFinger1.position;

            //SWIPE
            if (gestureTime <= _swipeProperty.swipeTime &&
                Vector2.Distance(startPoint, endPoint) >= (_swipeProperty.minSwipeDistance * Screen.dpi) && isFingerUp == true)
            {
                FireSwipeEvent();
            }  
        }
        else
        {
            gestureTime += Time.deltaTime;
        }
    }

    private void checkTwoFingerGestures()
    {

        trackedFinger1 = Input.GetTouch(0);
        trackedFinger2 = Input.GetTouch(1);

        //SPREAD
        if (trackedFinger1.phase == TouchPhase.Moved || trackedFinger2.phase == TouchPhase.Moved)
        {
            Vector2 prevPoint1 = GetPreviousPoint(trackedFinger1);
            Vector2 prevPoint2 = GetPreviousPoint(trackedFinger2);

            float currDistance = Vector2.Distance(trackedFinger1.position, trackedFinger2.position);
            float prevDistance = Vector2.Distance(prevPoint1, prevPoint2);

            if (Mathf.Abs(currDistance - prevDistance) >= (_spreadProperty.minDistanceChange * Screen.dpi) && isTwoFingersUp == true)
            {
                FireSpreadEvent(currDistance - prevDistance);
            }
        }

        //ROTATE
        //Checked if either finger moved
        if ((trackedFinger1.phase == TouchPhase.Moved ||
            trackedFinger2.phase == TouchPhase.Moved) &&
            //Check if the distance bet the 2 fingers is greater than the set minimum
            Vector2.Distance(trackedFinger1.position, trackedFinger2.position) >=
            (Screen.dpi * _rotateProperty.minDistance))
        {
            //Get the positions of the finger in the previous frame
            Vector2 prevPoint1 = GetPreviousPoint(trackedFinger1);
            Vector2 prevPoint2 = GetPreviousPoint(trackedFinger2);

            //Get the vectors of the fingers and their respective prev. position
            Vector2 diffVector = trackedFinger1.position - trackedFinger2.position;
            Vector2 prevDiffVector = prevPoint1 - prevPoint2;

            float angle = Vector2.Angle(prevDiffVector, diffVector);

            //Check if angle is more than the required amount
            if (angle >= _rotateProperty.minChange && isTwoFingersUp == true)
            {
                FireRotateEvent(diffVector, prevDiffVector, angle);
            }
        }
    }

    private void FireSwipeEvent()
    {
        isFingerUp = false;

        Vector2 dir = endPoint - startPoint;

        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if(dir.x > 0)
            {
                Debug.Log("Right");
                if(gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                    gameHandler.AddHistoryNote(Notes.SWIPE_RIGHT);
            }
            else
            {
                Debug.Log("Left");
                if (gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                    gameHandler.AddHistoryNote(Notes.SWIPE_LEFT);
            }
        }
        else
        {
            if(dir.y > 0)
            {
                Debug.Log("Up");
                if (gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                    gameHandler.AddHistoryNote(Notes.SWIPE_UP);
            }
            else
            {
                Debug.Log("Down");
                if (gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                    gameHandler.AddHistoryNote(Notes.SWIPE_DOWN);
            }
        }
    }

    private Vector2 GetPreviousPoint(Touch finger)
    {
        return finger.position - finger.deltaPosition;
    }

    private void FireSpreadEvent(float dist)
    {
        isTwoFingersUp = false;

        Vector2 mid = GetMidPoint(trackedFinger1.position, trackedFinger2.position);
        
        if(dist > 0)
        {
            Debug.Log("spread");
            if (gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                gameHandler.AddHistoryNote(Notes.SPREAD);
        }
        else
        {
            Debug.Log("pinch");
            if (gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                gameHandler.AddHistoryNote(Notes.PINCH);
        }
    }

    private void FireRotateEvent(Vector2 diff, Vector2 prevDiff, float angle)
    {
        isTwoFingersUp = false;

        Vector3 cross = Vector3.Cross(prevDiff, diff);
        RotationDirections rotDir = RotationDirections.CW;
        if (cross.z > 0)
        {
            rotDir = RotationDirections.CCW;

            if (gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                gameHandler.AddHistoryNote(Notes.ROT_CCW);
        }
        else
        {
            rotDir = RotationDirections.CW;

            if (gameHandler.HistorySequence.Count < gameHandler.TargetSequence.Length)
                gameHandler.AddHistoryNote(Notes.ROT_CW);
        }
    }

    private Vector2 GetMidPoint(Vector2 finger1, Vector2 finger2)
    {
        return (finger1 + finger2) / 2;
    }
}


