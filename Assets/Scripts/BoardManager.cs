using UnityEngine;
using System.Linq;

public class BoardManager : Singleton<BoardManager>
{
    #region PRIVATE_FIELDS

    [SerializeField] private GameObject board;
    [Tooltip("Size relative to slot scale, that will affect on board's slot to slot spacing")] [SerializeField] private float spacingFactor = 0.1f;
    [SerializeField] private int[] gridRowCount; //this is an array wich length is board row count and the value of each element is the number of slots in the each row in the board
    [SerializeField] private UnitSlot unitSlotSample; // the sample of UnitSlot from game scene
    private BoardSize boardSizes;

    #endregion

    #region PROPERTIES
    public int MaxRows { get { return gridRowCount.Length; } }
    public int MaxColumns { get { return gridRowCount.Max(); } }
    public int TotalUnits { get { return gridRowCount.Sum(); } }
#endregion

    private void Start()
    {
        boardSizes = new BoardSize(board.GetComponent<MeshRenderer>().bounds.size.x, board.GetComponent<MeshRenderer>().bounds.size.y);
    }

    //main function for creating the board, will be started from main GameManager, passing a parameter the playerData
    public void FormBoard(PlayerData pd)
    {
        UnitManager.Instance.InitializeArrays(gridRowCount); //Initialize the arrays managing the connections between grid coordinates, units and slots

        var maxLength = (MaxRows > MaxColumns) ? MaxRows : MaxColumns; //get the maximum Length horizonaly or verticaly for creating responsive board
        var perSlotWidth = boardSizes.Width/ MaxColumns;
        var perSlotHeight= boardSizes.Height/ MaxRows;

        for (int i=0; i<MaxRows; i++)
        {
            for (int j = 0; j < gridRowCount[i]; j++)
            {
                var slotCoordinates = (i, j);
                GameObject slotGameObject = Instantiate(unitSlotSample.gameObject);
                UnitSlot slot  = slotGameObject.GetComponent<UnitSlot>();
                Transform slotTransform = slotGameObject.transform;

                UnitManager.Instance.AddUnit(slotCoordinates, null);
                UnitManager.Instance.AddUnitSlot(slotCoordinates, slot);

                slot.SetGridCoordiantes(slotCoordinates);
                slotGameObject.name = $"UnitSlot{slotCoordinates}";
                //scaling the slot considering the spacingFactor
                slotTransform.localScale = new Vector3(board.transform.localScale.x * (1 - spacingFactor) *10/ maxLength, board.transform.localScale.y * (1 - spacingFactor) * 10 / maxLength, 0f);
                slotTransform.SetParent(board.transform, true); // set as a sibling to board.transform keeping the scaling, position and rotation in WorldSpace
                slotTransform.rotation = new Quaternion(90, 0, 0, 0);

                //positioning the slot on the board on the proper position
                slotTransform.position = board.transform.position - new Vector3(boardSizes.Width / 2 - perSlotWidth / 2 - j* perSlotWidth, -(boardSizes.Height / 2 - perSlotHeight / 2 - i * perSlotHeight), 0); 
            }
        }

        //if after creating slots on the board, the player data loaded from storage is found, then load the units.
        if (pd != null)
        {
            UnitManager.Instance.LoadBoardUnitsFromPlayerData(pd);
        }
    }

}


public struct BoardSize
{
    public float Width;
    public float Height;

    public BoardSize(float width, float height)
    {
        Width = width;
        Height = height;
    }
}

