using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson_21
{
    /// <summary>
    /// 이 인터페이스를 구현하는 컴포넌트는 `DragItem`을 드래그한 후 대상으로 동작할 수 있습니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDragDestination<T> where T : class
    {
        /// <summary>
        /// 주어진 아이템의 얼마나 많이 받아들일 수 있는지를 나타냅니다.
        /// </summary>
        /// <param name="item">추가될 수 있는 아이템 유형.</param>
        /// <returns>제한이 없는 경우 Int.MaxValue를 반환해야 합니다.</returns>
        int MaxAcceptable(T item);

        /// <summary>
        /// 아이템을 이 대상에 추가했을 때 UI 및 데이터를 업데이트하는 메서드입니다.
        /// </summary>
        /// <param name="item">아이템 유형.</param>
        /// <param name="number">아이템 수량.</param>
        void AddItems(T item, int number);
    }

}