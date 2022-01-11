using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tweener 
{
    public static IEnumerator MoveTo(Transform objectForMove, Vector2 VectorTo, float speed,Action callBack = null)
    {
        while(Vector2.Distance(objectForMove.localPosition,VectorTo) > 0.00001f)
        {
            objectForMove.localPosition = Vector2.MoveTowards(objectForMove.localPosition, VectorTo, Time.deltaTime * speed);
            yield return null;
        }
        if (callBack != null) callBack.Invoke();
    }
}
