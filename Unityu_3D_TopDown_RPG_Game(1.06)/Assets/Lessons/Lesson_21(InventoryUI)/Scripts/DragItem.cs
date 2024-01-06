using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lesson_21
{
    /// <summary>
    /// UI 요소가 컨테이너로부터 드래그 앤 드롭될 수 있도록 허용합니다.
    /// 
    /// 드래그 가능한 유형을 나타내는 하위 클래스를 만들고,
    /// 드래그 가능한 UI 요소에 배치합니다.
    /// 
    /// 드래그 중에 아이템은 부모 캔버스로 재부모화됩니다.
    /// 
    /// 아이템이 드롭된 후에는 원래의 UI 부모로 자동으로 반환됩니다.
    /// 드래그가 발생한 후 인터페이스를 업데이트하는 컴포넌트의 역할은
    /// 'IDragContainer`, `IDragDestination` 및 `IDragSource`를 구현하는 것입니다.
    /// </summary>
    /// <typeparam name="T">드래그되는 아이템을 나타내는 유형.</typeparam>
    public class DragItem<T> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
        where T : class
    {
        // 프라이빗 상태
        Vector3 startPosition; // 시작 위치
        Transform originalParent; // 원래 부모
        IDragSource<T> source; // 드래그 소스

        // 캐시된 참조
        Canvas parentCanvas; // 부모 캔버스

        // 라이프사이클 메서드
        private void Awake()
        {
            parentCanvas = GetComponentInParent<Canvas>(); // 부모 캔버스 가져오기
            source = GetComponentInParent<IDragSource<T>>(); // 드래그 소스 가져오기
        }

        // 시작 드래그 핸들러
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            startPosition = transform.position; // 시작 위치 기록
            originalParent = transform.parent; // 원래 부모 기록
                                               // 아니면 드롭 이벤트를 받을 수 없음.
            GetComponent<CanvasGroup>().blocksRaycasts = false; // 레이캐스트 차단 해제
            transform.SetParent(parentCanvas.transform, true); // 부모 캔버스로 이동
        }

        // 드래그 핸들러
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position; // 위치를 이벤트 위치로 업데이트
        }

        // 드래그 종료 핸들러
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            transform.position = startPosition; // 위치를 시작 위치로 복원
            GetComponent<CanvasGroup>().blocksRaycasts = true; // 레이캐스트 차단 활성화
            transform.SetParent(originalParent, true); // 원래 부모로 복원

            IDragDestination<T> container;
            if (!EventSystem.current.IsPointerOverGameObject()) // UI 요소 위에 포인터가 없는 경우
            {
                container = parentCanvas.GetComponent<IDragDestination<T>>(); // 부모 캔버스에서 드래그 대상 가져오기
            }
            else
            {
                container = GetContainer(eventData); // 컨테이너 가져오기
            }

            if (container != null)
            {
                DropItemIntoContainer(container); // 컨테이너로 아이템 드롭
            }
        }

        // 컨테이너를 가져오는 프라이빗 메서드
        private IDragDestination<T> GetContainer(PointerEventData eventData)
        {
            if (eventData.pointerEnter)
            {
                var container = eventData.pointerEnter.GetComponentInParent<IDragDestination<T>>(); // 컨테이너 가져오기

                return container;
            }
            return null;
        }

        // 컨테이너로 아이템을 드롭하는 프라이빗 메서드
        private void DropItemIntoContainer(IDragDestination<T> destination)
        {
            if (object.ReferenceEquals(destination, source)) return; // 같은 소스와 대상인 경우 아무 작업도 수행하지 않음

            var destinationContainer = destination as IDragContainer<T>; // 대상 컨테이너
            var sourceContainer = source as IDragContainer<T>; // 소스 컨테이너

            // 교환할 수 없는 경우
            if (destinationContainer == null || sourceContainer == null ||
                destinationContainer.GetItem() == null ||
                object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination); // 간단한 전송 시도
                return;
            }

            AttemptSwap(destinationContainer, sourceContainer); // 아이템 교환 시도
        }

        // 아이템 교환을 시도하는 프라이빗 메서드
        private void AttemptSwap(IDragContainer<T> destination, IDragContainer<T> source)
        {
            // 양쪽에서 아이템을 일시적으로 제거
            var removedSourceNumber = source.GetNumber();
            var removedSourceItem = source.GetItem();
            var removedDestinationNumber = destination.GetNumber();
            var removedDestinationItem = destination.GetItem();

            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
            var destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

            // 되돌릴 작업 수행 (필요한 경우)
            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedSourceItem, sourceTakeBackNumber);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            // 성공적인 교환을 수행할 수 없는 경우
            if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber)
            {
                destination.AddItems(removedDestinationItem, removedDestinationNumber);
                source.AddItems(removedSourceItem, removedSourceNumber);
                return;
            }

            // 교환 수행
            if (removedDestinationNumber > 0)
            {
                source.AddItems(removedDestinationItem, removedDestinationNumber);
            }
            if (removedSourceNumber > 0)
            {
                destination.AddItems(removedSourceItem, removedSourceNumber);
            }
        }

        // 간단한 전송을 시도하는 프라이빗 메서드
        private bool AttemptSimpleTransfer(IDragDestination<T> destination)
        {
            var draggingItem = source.GetItem();
            var draggingNumber = source.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                source.RemoveItems(toTransfer);
                destination.AddItems(draggingItem, toTransfer);
                return false;
            }

            return true;
        }

        // 되돌려야 할 작업 수를 계산하는 프라이빹 메서드
        private int CalculateTakeBack(T removedItem, int removedNumber, IDragContainer<T> removeSource, IDragContainer<T> destination)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = removeSource.MaxAcceptable(removedItem);

                // 작업을 중단하고 리셋
                if (sourceTakeBackAcceptable < takeBackNumber)
                {
                    return 0;
                }
            }
            return takeBackNumber;
        }
    }

}