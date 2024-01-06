using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_32
{
    public class UISwitcher : MonoBehaviour
    {
        [SerializeField] GameObject entryPoint;
        private void Start()
        {
            SwitchTo(entryPoint);
        }

        public void SwitchTo(GameObject toDisplay)
        {
            if (!toDisplay) return;
            if (toDisplay.transform.parent != transform) return;


            foreach(Transform child in this.transform)
            {
                child.gameObject.SetActive(child.gameObject == toDisplay);
            }
        }
    }
}

