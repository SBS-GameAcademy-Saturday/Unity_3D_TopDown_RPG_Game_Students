using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    public interface IPredicateEvaluator
    {
        // 'Evaluate' 메서드를 정의하는 인터페이스

        // 'predicate'는 평가할 조건식을 나타내는 문자열입니다.
        // 'parameters'는 조건식에 필요한 매개변수의 배열입니다.

        // 'Evaluate' 메서드는 조건식과 매개변수를 받아서 해당 조건식을 평가하고 결과를 반환합니다.
        // 결과가 불리언 값 또는 null (참, 거짓 또는 평가 불가능)이 될 수 있습니다.
        bool? Evaluate(string predicate, string[] parameters);
    }
}
