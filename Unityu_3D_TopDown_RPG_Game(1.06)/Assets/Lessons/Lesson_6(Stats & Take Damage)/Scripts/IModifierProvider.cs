using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifiers(Stat stat);
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}
