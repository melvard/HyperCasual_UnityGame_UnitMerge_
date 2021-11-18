using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Collider))]
public class Unit : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region PRIVATE_FIELDS

    private SpriteRenderer spriteRenderer;
    private BoxCollider unitBoxCollider; // to recieve pointer click events the gameObject needs Colliders
    private Vector3 beginDragUnitPosition; // for fixing the position in the world Space on pointer down, this will not be changed untill next click 
    private Vector3 pointerBeginPosition; // for fixing the position of pointer in OnDrag method, to calculate the delta change
    private TextMesh levelTextMesh;
    private int maxLevel => GameManager.Instance.MaxUnitLevel;
    private bool movementDetected; //if after OnPointerDown the draging started, record movement

    #endregion

    #region PROPERTIES

    public int Level { get; private set; } = 0; //level of current Unit
    public (int, int) UnitCoordinates { get; set; } // coordinates in board grid
    public Transform unitTransform { get; private set; } //

    // how much the unit weights when adding scores after merging (somehow depends on unit lvl)
    public float Weight
    {
        get
        {
            return Mathf.Pow(Level, 2);
        }
    }

    #endregion


    void Start()
    {
        unitBoxCollider = GetComponent<BoxCollider>();
        unitTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = GameManager.Instance.GetColorFromDict(Level);
        levelTextMesh = transform.GetChild(0).GetComponent<TextMesh>();

    }

    public void LevelUpLevelUp()
    {
        Debug.Log("Level up, Level up, yeah, level up :D");
        Level += 1;
        this.SetOnUnitTextMeshText(Level.ToString());
        spriteRenderer.color = GameManager.Instance.GetColorFromDict(Level);

    }

    public void SetLevel(int lvl)
    {
        //if coder attempts to set lvl less/greater than it was supposed to be then normalize
        if (lvl < 0)
        {
            lvl = -1;
        }
        else if (lvl > maxLevel)
        {
            lvl = maxLevel;
        }
        // if lvl is zero then it is a box, so change text to "Box"
        SetOnUnitTextMeshText((lvl == 0) ? "Box" : lvl.ToString());
        Level = lvl;
    }


    public void SetUnitPositionAndScale(Transform unitSlotTransform)
    {
        transform.position = unitSlotTransform.position - new Vector3(0, 0, 0.1f); 
        transform.SetParent(unitSlotTransform.parent);
        transform.localScale = unitSlotTransform.localScale;
    }

    //when the pointer is down on the unit object, OnPointerDown() will work 
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer is down");
        movementDetected = false;
        this.DecreaseSpriteTransparency();
    }

    //if after clikcing on the object the pointer starts moving firstly the OnBegginDrag() will work
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begining dragging");
        movementDetected = true;
        unitBoxCollider.enabled = false;
        beginDragUnitPosition = unitTransform.position;
        pointerBeginPosition = Camera.allCameras[0].ScreenToWorldPoint(eventData.pressPosition);

    }

    //during the movement of the pointer all the time in every frame OnDrag() function will be implemented
    public void OnDrag(PointerEventData eventData)
    {
        //get the world space position of the pointer to accomplish calculation of movement in accordance with pointer
        Vector3 pointerCurrentWorldSpace = Camera.allCameras[0].ScreenToWorldPoint(eventData.position);
        Vector3 pointerDeltaWorldSpace = pointerCurrentWorldSpace - pointerBeginPosition;
        unitTransform.position += pointerDeltaWorldSpace;
        pointerBeginPosition = pointerCurrentWorldSpace; // itWill always be the previous frame's position for pointer

    }

    //as soon as the pointer's drag is finished on the last frame of movement the OnEndDrag() will work

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Ending dragging");
        unitBoxCollider.enabled = true;
        if (eventData.pointerEnter == null)
        {
            //if somewhere unessential the unit is droped, then restore position
            ResetBegginPosition();
        }
    }

    //when the pointer is up OnPointerUp() will work
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer is up");
        this.IncreaseSpriteTransparency();
        if (!movementDetected && Level == 0)
        {
            // if user didn't stated draging the unit and unit has lvl 0 (is suprise box)
            // then LevelUp this unit when pointerUp. This creates an imitation of button.
            LevelUpLevelUp();
        }

    }

    //if any other object is droped on this unit object then OnDrop() will work (all is about merging)
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("On drop worked");
        Unit droppingUnit = eventData.pointerDrag.GetComponent<Unit>();

        if (droppingUnit.Level == maxLevel || Level == maxLevel)
        {
            MessageAlertHandler.Instance.DisplayTimedTextMessage($"The maximum unit level reached ({maxLevel}) !", 3f);
        }
        else if (droppingUnit.Level == Level && Level != 0)
        {
            droppingUnit.GetComponent<Transform>().position = unitTransform.position;
            Destroy(droppingUnit.gameObject);
            ProgressBarController.Instance.AddScore(Weight);
            UnitManager.Instance.RemoveUnitFromSlot(droppingUnit.UnitCoordinates);
            this.LevelUpLevelUp();
            return;
        }
        else if (droppingUnit.Level == 0 || Level == 0)
        {
            MessageAlertHandler.Instance.DisplayTimedTextMessage("Ops, don't merge, just open box !", 3f);
        }
        else
        {
            MessageAlertHandler.Instance.DisplayTimedTextMessage("Ops, can't merge units of different levels !", 3f);
        }
        droppingUnit.ResetBegginPosition();

    }

    public void ResetBegginPosition()
    {
        transform.position = new Vector3(beginDragUnitPosition.x, beginDragUnitPosition.y, transform.position.z);
    }


    // when pointer is down, then decrease opacity of unit gameobject
    private void DecreaseSpriteTransparency()
    {
        var clr = spriteRenderer.color;
        spriteRenderer.color = new Color(clr.r, clr.g, clr.b, 0.5f);
    }

    // when pointer is up, then increase opacity of unit gameobject
    private void IncreaseSpriteTransparency()
    {
        var clr = spriteRenderer.color;
        spriteRenderer.color = new Color(clr.r, clr.g, clr.b, 1f);
    }

    private void SetOnUnitTextMeshText(string txt)
    {
        int charCount = txt.Length;
        if (levelTextMesh == null)
        {
            levelTextMesh = transform.GetChild(0).GetComponent<TextMesh>();
        }
        levelTextMesh.text = txt;

        // change font size so that it couldn't get out of the unit area
        // (another function besides kx+b could be applied, for example a^kx + b)
        levelTextMesh.fontSize = (int)(-9 * charCount + 100);
    }
}
