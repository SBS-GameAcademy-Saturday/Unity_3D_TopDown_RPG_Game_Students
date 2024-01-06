using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson.Utils
{
    /// <summary>
    /// ��(value)�� ���ΰ�, ù ��� ������ �ʱ�ȭ�� �����ϴ� �����̳� Ŭ�����Դϴ�.
    /// </summary>
    public class LazyValue<T>
    {
        private T _value; // �� ����� �ʵ�
        private bool _initialized = false; // �ʱ�ȭ ���θ� ��Ÿ���� �÷���
        private InitializerDelegate _initializer; // �ʱ�ȭ ��������Ʈ�� �����ϴ� �ʵ�

        public delegate T InitializerDelegate();

        /// <summary>
        /// �����̳ʸ� �����ϵ�, ���� ���� �ʱ�ȭ���� �ʽ��ϴ�.
        /// </summary>
        /// <param name="initializer">
        /// ó�� ����� �� ȣ���� �ʱ�ȭ ��������Ʈ�Դϴ�.
        /// </param>
        public LazyValue(InitializerDelegate initializer)
        {
            _initializer = initializer;
        }

        /// <summary>
        /// �� �����̳��� ������ �������ų� �����մϴ�.
        /// </summary>
        /// <remarks>
        /// �ʱ�ȭ ���� ���� �����ϴ� ��� Ŭ������ �ʱ�ȭ�մϴ�.
        /// </remarks>
        public T value
        {
            get
            {
                // ���� ��ȯ�ϱ� ���� �ʱ�ȭ�� �����մϴ�.
                ForceInit();
                return _value;
            }
            set
            {
                // �� �̻� �⺻ �ʱ�ȭ�� ������� �ʽ��ϴ�.
                _initialized = true;
                _value = value;
            }
        }

        /// <summary>
        /// ��������Ʈ�� ���� ���� �ʱ�ȭ�� �����մϴ�.
        /// </summary>
        public void ForceInit()
        {
            if (!_initialized)
            {
                _value = _initializer();
                _initialized = true;
            }
        }
    }

}
