using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    //[CreateAssetMenu(fileName = "TargetingStrategy", menuName = "Lesson/Strategies/TargetingStrategy", order = 0)]
    public abstract class FilterStrategy : ScriptableObject
    {
        public abstract IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter);
    }
}
