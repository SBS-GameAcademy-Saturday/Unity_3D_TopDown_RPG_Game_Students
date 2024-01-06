using Lesson_Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_25
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField]
        Disjunction[] and; // 여러 개의 조건을 논리 AND로 결합하는 배열

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in and)
            {
                if (!dis.Check(evaluators))
                {
                    return false; // 모든 조건이 참이어야 true를 반환, 하나라도 거짓이면 false를 반환
                }
            }
            return true; // 모든 AND 조건이 참이면 true를 반환
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField]
            Predicate[] or; // 여러 개의 조건을 논리 OR로 결합하는 배열

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                {
                    if (pred.Check(evaluators))
                    {
                        return true; // 하나라도 조건이 참이면 true를 반환
                    }
                }
                return false; // 모든 OR 조건이 거짓이면 false를 반환
            }
        }

        [System.Serializable]
        class Predicate
        {
            [SerializeField]
            string predicate; // 조건을 나타내는 문자열
            [SerializeField]
            string[] parameters; // 조건에 필요한 매개변수의 배열
            [SerializeField]
            bool negate = false; // 조건 결과를 부정하는 여부

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters); // 조건을 평가하고 결과를 얻음
                    if (result == null)
                    {
                        continue; // 결과가 null이면 다음 평가로 이동
                    }

                    if (result == negate) return false; // 조건 결과를 부정하고 거짓인 경우 false를 반환
                }
                return true; // 모든 평가에서 조건이 참이면 true를 반환
            }
        }
    }
}
