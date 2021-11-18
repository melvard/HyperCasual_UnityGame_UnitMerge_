using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class UnitSlot : MonoBehaviour, IDropHandler
{
    private Unit unit;

    public (int, int) GridCoordinates; // unit coordinates in the board grid

    public Unit Unit
    {
        get
        {
            return UnitManager.Instance.GetUnitWithCoordinates(GridCoordinates);
        }
        private set
        {
            UnitManager.Instance.AddUnit(GridCoordinates, value);
        }
    }

    /// <summary>
    /// This funtion will be called if any object (actually any unit) is dropped
    /// on the slot, depending on the status of slot (busy, free, same unit)
    /// the proper decision will be applied.
    /// </summary>

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            unit = eventData.pointerDrag.GetComponent<Unit>(); // the Unit which is droped on the Slot

            if (Unit == null) //if slot is empty (has no Units in the same gridCoordinates)
            {
                Debug.Log("Empty slot is found");
                Transform unitTransform = unit.transform;
                //unitTransform.SetParent(gameObject.transform); //set the Unit Game Object as a sibling for the slot
                unitTransform.position = transform.position - new Vector3(0, 0, 1f);
                UnitManager.Instance.RemoveUnitFromSlot(unit.UnitCoordinates); //remove the previous position information
                unit.UnitCoordinates =  GridCoordinates; //add the new grid position info to Unit
                Unit = unit; // set unit for new grid position
                
            }
            else
            {
                // 1st situation - Another object is already in the slot 
                if (Unit.gameObject != eventData.pointerDrag)
                {
                    // Wait, then just merge with the Unit in this free slot, no? 
                    // This situation can occure when the unit is not dropped exactly on the another unit
                    // but on the unit slot. In this situation we will threat like collision with the Unit in slot.

                    Debug.Log("Another Unit found");
                    Unit.OnDrop(eventData);
                }
                // 2nd situation - The unit is already belongs to that slot
                else
                {
                    Debug.Log("This unit is already in the grid");
                    unit.ResetBegginPosition();
                }
            }
        }
    }

    public void SetGridCoordiantes((int, int) coordinates)
    {
        GridCoordinates = coordinates;
    }

}
