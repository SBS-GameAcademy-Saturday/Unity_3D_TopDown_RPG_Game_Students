//using Lesson_4;
using Lesson_5;
using Lesson_Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(PlayerController callingController);

        bool HandleRaycast(Lesson_11.PlayerController callingController);
    }
}

