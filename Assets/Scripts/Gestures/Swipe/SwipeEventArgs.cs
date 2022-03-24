using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class SwipeEventArgs : EventArgs
{
    public enum SwipeDirections
    {
        RIGHT,
        LEFT,
        UP,
        DOWN
    }

    private Vector2 swipePos;
    private SwipeDirections swipeDirection;
    private Vector2 swipeVector;
    
    public SwipeEventArgs(Vector2 _swipePos, SwipeDirections _swipeDir, Vector2 _swipeVector)
    {
        swipePos = _swipePos;
        swipeDirection = _swipeDir;
        swipeVector = _swipeVector;
    }

    public Vector2 SwipePos
    {
        get { return swipePos; }
    }

    public SwipeDirections SwipeDirection
    {
        get { return swipeDirection; }
    }

    public Vector2 SwipeVector
    {
        get { return swipeVector; }
    }
}
