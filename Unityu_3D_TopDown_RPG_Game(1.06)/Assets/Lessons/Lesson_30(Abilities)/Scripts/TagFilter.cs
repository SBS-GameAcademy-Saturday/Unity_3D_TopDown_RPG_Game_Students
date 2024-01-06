using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_30
{
    [CreateAssetMenu(fileName = "TagFilter", menuName = "Lesson/Filters/TagFilter", order = 0)]
    public class TagFilter : FilterStrategy
    {
        [SerializeField] private string tagToFilter = ""; 

        public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
        {
            foreach(var gameObject in objectsToFilter)
            {
                if (gameObject.CompareTag(tagToFilter))
                {
                    yield return gameObject;
                }
            }
        }
    }
}

