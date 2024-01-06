using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson.Utils
{
    /// <summary>
    /// 값(value)을 감싸고, 첫 사용 직전에 초기화를 보장하는 컨테이너 클래스입니다.
    /// </summary>
    public class LazyValue<T>
    {
        private T _value; // 값 저장용 필드
        private bool _initialized = false; // 초기화 여부를 나타내는 플래그
        private InitializerDelegate _initializer; // 초기화 델리게이트를 저장하는 필드

        public delegate T InitializerDelegate();

        /// <summary>
        /// 컨테이너를 설정하되, 값을 아직 초기화하지 않습니다.
        /// </summary>
        /// <param name="initializer">
        /// 처음 사용할 때 호출할 초기화 델리게이트입니다.
        /// </param>
        public LazyValue(InitializerDelegate initializer)
        {
            _initializer = initializer;
        }

        /// <summary>
        /// 이 컨테이너의 내용을 가져오거나 설정합니다.
        /// </summary>
        /// <remarks>
        /// 초기화 전에 값을 설정하는 경우 클래스를 초기화합니다.
        /// </remarks>
        public T value
        {
            get
            {
                // 값을 반환하기 전에 초기화를 보장합니다.
                ForceInit();
                return _value;
            }
            set
            {
                // 더 이상 기본 초기화를 사용하지 않습니다.
                _initialized = true;
                _value = value;
            }
        }

        /// <summary>
        /// 델리게이트를 통해 값의 초기화를 강제합니다.
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
