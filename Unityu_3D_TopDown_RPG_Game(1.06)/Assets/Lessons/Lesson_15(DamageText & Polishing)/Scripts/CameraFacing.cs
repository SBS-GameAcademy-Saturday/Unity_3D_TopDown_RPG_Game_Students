using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_15
{
    public class CameraFacing : MonoBehaviour
    {
        void LateUpdate()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }

}
