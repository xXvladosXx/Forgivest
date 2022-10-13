using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities.UI.Dragging
{
 
    public class DragItem  : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 _startPosition;
        private Transform _originalParent;
        private IDragSource _source;

        private Canvas _parentCanvas;

        public event Action<int, GameObject> OnDragEnded;
        public event Action<int, int> OnItemSwapped; 
        public event Action<int> OnItemSlotChanged; 
        private void Awake()
        {
            _parentCanvas = GetComponentInParent<Canvas>();
            _source = GetComponentInParent<IDragSource >();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _startPosition = transform.position;
            _originalParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            transform.SetParent(_parentCanvas.transform, true);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            transform.position = _startPosition;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.SetParent(_originalParent, true);

            IDragDestination  container;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                container = _parentCanvas.GetComponent<IDragDestination >();
            }
            else
            {
                container = GetContainer(eventData);
            }

            if (container != null)
            {
                DropItemIntoContainer(container);
            }
            
            OnDragEnded?.Invoke(_source.SourceIndex , gameObject);
        }

        private IDragDestination GetContainer(PointerEventData eventData)
        {
            if (eventData.pointerEnter)
            {
                var container = eventData.pointerEnter.GetComponentInParent<IDragDestination>();

                return container;
            }
            return null;
        }

        private void DropItemIntoContainer(IDragDestination  destination)
        {
            if (object.ReferenceEquals(destination, _source)) return;

            var destinationContainer = destination as IDragContainer ;
            var sourceContainer = _source as IDragContainer ;

            if (destinationContainer == null || sourceContainer == null || 
                destinationContainer.GetItem() == null || 
                object.ReferenceEquals(destinationContainer.GetItem(), sourceContainer.GetItem()))
            {
                AttemptSimpleTransfer(destination);
                
                OnItemSlotChanged?.Invoke(destination.Index );
                print($"item swapped {destination.Index }");
                return;
            }

            AttemptSwap(destinationContainer, sourceContainer);
        }

        private void AttemptSwap(IDragContainer  destination, IDragContainer  source)
        {
            var removedSourceNumber = source.GetNumber();
            var removedSourceItem = source.GetItem();
            var removedDestinationNumber = destination.GetNumber();
            var removedDestinationItem = destination.GetItem();
            var removedSourceIndex = source.Index;
            var removedDestination = destination.Index;

            source.RemoveItems(removedSourceNumber);
            destination.RemoveItems(removedDestinationNumber);

            var sourceTakeBackNumber = CalculateTakeBack(removedSourceItem, removedSourceNumber, source, destination);
            var destinationTakeBackNumber = CalculateTakeBack(removedDestinationItem, removedDestinationNumber, destination, source);

            if (sourceTakeBackNumber > 0)
            {
                source.AddItems(removedSourceItem, sourceTakeBackNumber, source.Index);
                removedSourceNumber -= sourceTakeBackNumber;
            }
            if (destinationTakeBackNumber > 0)
            {
                destination.AddItems(removedDestinationItem, destinationTakeBackNumber, destination.Index);
                removedDestinationNumber -= destinationTakeBackNumber;
            }

            if (source.MaxAcceptable(removedDestinationItem) < removedDestinationNumber ||
                destination.MaxAcceptable(removedSourceItem) < removedSourceNumber)
            {
                destination.AddItems(removedDestinationItem, removedDestinationNumber, destination.Index);
                source.AddItems(removedSourceItem, removedSourceNumber, source.Index);
                
                OnItemSwapped?.Invoke(source.Index , destination.Index );
                print($"item swapped {source.Index }, {destination.Index }");
                return;
            }

            if (removedDestinationNumber > 0)
            {
                source.AddItems(removedDestinationItem, removedDestinationNumber, removedSourceIndex);
            }
            if (removedSourceNumber > 0)
            {
                destination.AddItems(removedSourceItem, removedSourceNumber, removedDestination);
            }
            
            OnItemSwapped?.Invoke(source.Index, destination.Index );
            print($"item swapped {source.SourceIndex }, {destination.Index }");
        }

        private bool AttemptSimpleTransfer(IDragDestination destination)
        {
            var draggingItem = _source.GetItem();
            var draggingNumber = _source.GetNumber();

            var acceptable = destination.MaxAcceptable(draggingItem);
            var toTransfer = Mathf.Min(acceptable, draggingNumber);

            if (toTransfer > 0)
            {
                _source.RemoveItems(toTransfer);

                destination.AddItems(draggingItem, toTransfer, destination.Index);
                
                return false;
            }

            return true;
        }

        private int CalculateTakeBack(Sprite removedItem, int removedNumber, IDragContainer  removeSource, IDragContainer  destination)
        {
            var takeBackNumber = 0;
            var destinationMaxAcceptable = destination.MaxAcceptable(removedItem);

            if (destinationMaxAcceptable < removedNumber)
            {
                takeBackNumber = removedNumber - destinationMaxAcceptable;

                var sourceTakeBackAcceptable = removeSource.MaxAcceptable(removedItem);

                // Abort and reset
                if (sourceTakeBackAcceptable < takeBackNumber)
                {
                    return 0;
                }
            }
            return takeBackNumber;
        }
    }
}