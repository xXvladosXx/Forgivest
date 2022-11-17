using System;
using System.Collections;
using System.Collections.Generic;
using UI.Inventory.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;

public class news : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipScreenSpace.Instance.ShowTooltip("WWWWWW");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipScreenSpace.Instance.HideTooltip();
    }
}
