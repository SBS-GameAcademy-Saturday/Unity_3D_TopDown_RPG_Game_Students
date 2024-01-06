using Lesson_Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    public class AbilityData : IAction
    {
        GameObject user;
        Vector3 targetedPoint;
        IEnumerable<GameObject> targets;
        bool cancled = false;

        public AbilityData(GameObject user) 
        {
            this.user = user;
        }
        public GameObject GetUser()
        {
            return user;
        }
        public IEnumerable<GameObject> GetTargets()
        {
            return targets;
        }
        public void SetTargets(IEnumerable<GameObject> targets)
        {
            this.targets = targets;
        }

        public void SetTragetedPoint(Vector3 targetedPoint)
        {
            this.targetedPoint = targetedPoint;
        }

        public Vector3 GetTragetedPoint()
        {
            return targetedPoint;
        }

        public void StartCoroutine(IEnumerator coroutine)
        {
            user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
        }

        public void Cancle()
        {
            cancled = true;
        }

        public bool IsCancled()
        {
            return cancled;
        }
    }

}
