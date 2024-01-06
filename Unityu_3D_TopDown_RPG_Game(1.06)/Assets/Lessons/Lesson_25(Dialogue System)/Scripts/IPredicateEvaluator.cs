using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_Common
{
    public interface IPredicateEvaluator
    {
        // 'Evaluate' �޼��带 �����ϴ� �������̽�

        // 'predicate'�� ���� ���ǽ��� ��Ÿ���� ���ڿ��Դϴ�.
        // 'parameters'�� ���ǽĿ� �ʿ��� �Ű������� �迭�Դϴ�.

        // 'Evaluate' �޼���� ���ǽİ� �Ű������� �޾Ƽ� �ش� ���ǽ��� ���ϰ� ����� ��ȯ�մϴ�.
        // ����� �Ҹ��� �� �Ǵ� null (��, ���� �Ǵ� �� �Ұ���)�� �� �� �ֽ��ϴ�.
        bool? Evaluate(string predicate, string[] parameters);
    }
}
