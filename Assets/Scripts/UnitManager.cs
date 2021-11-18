using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// This class is responsible for controlling the Units information, positions, states and etc.
/// The manager also serves as a box / unit generator.
/// </summary>
public class UnitManager : Singleton<UnitManager>
{

    #region PRIVATE_FIELDS

    [SerializeField] private Button countDownButton;
    [SerializeField] private Text countText;
    [SerializeField] private Slider generatorProgress;
    [SerializeField] [Tooltip("A sample Unit component from scene")] private Unit unitSample;

    private int timerCountDown => GameManager.Instance.TimeCountDownSize;
    private int clickCountDown => GameManager.Instance.ClickCountDownSize;
    private bool continueAutoCountDown = true; //this will prevent Update function from continuing auto countDown;
    private int count = 10; //box generator counter
    private float time = 0;
    #endregion

    #region PROPERTIES
    public UnitSlot[][] unitSlotsArray; //an array of UnitSlots corresponding to grid coordinates with indices.
    public Unit[][] unitsArray; //an array of Unit corresponding to grid coordinates with indices.

    // returns true if any free slot is found, otherwise => false
    public bool PlaceIsAvailable => unitsArray.Any(i => i.Contains(null));
    #endregion

    #region MONO_BEHAVIOUR_METHODS

    private void Start()
    {
        countText.text = count.ToString();
        generatorProgress.value = 0;
    }


    private void Update()
    {
        if (continueAutoCountDown)
        {
            countDownButton.interactable = true;
            time += Time.deltaTime;
            if (time >= 1f)
            {
                time = 0;
                CountDown(timerCountDown);
            }
        }
        else
        {
            countDownButton.interactable = false;
        }

    }

    #endregion

    #region BOX_GENERATOR_COUNTDOWN

    private void CountDown(int countDownSize)
    {
        count -= countDownSize;
        countText.text = count.ToString();
        //some math here ))
        generatorProgress.value = (-count / 10f) + 1; // y = kx + b, where k = -1/10 , b = 1
        if (count <= 0)
        {
            count = 10;
            TryGenerateBoxInFirstFreeSlot(); //box generation
            if (!PlaceIsAvailable)
            {
                continueAutoCountDown = false;
                generatorProgress.value = 0f;
                countText.text = count.ToString();
            }
        }     
    }


    public void ClickCountDown()
    {
        CountDown(clickCountDown);
    }

    #endregion

    #region SLOT_AND_UNIT_ARRAYS
    public void InitializeArrays(int[] gridCountArray)
    {
        unitsArray = new Unit[gridCountArray.Length][];
        unitSlotsArray = new UnitSlot[gridCountArray.Length][];

        for (int i = 0; i < unitSlotsArray.Length; i++)
        {
            unitsArray[i] = new Unit[gridCountArray[i]];
            unitSlotsArray[i] = new UnitSlot[gridCountArray[i]];
        }
    }

    public void AddUnitSlot((int, int) gridCoordinates, UnitSlot slot)
    {
        unitSlotsArray[gridCoordinates.Item1][gridCoordinates.Item2] = slot;
    }

    public void AddUnit((int, int) gridCoordinates, Unit unit)
    {
        unitsArray[gridCoordinates.Item1][gridCoordinates.Item2] = unit;

    }

    public void RemoveUnitFromSlot((int, int) gridCoordinates)
    {
        unitsArray[gridCoordinates.Item1][gridCoordinates.Item2] = null;
        continueAutoCountDown = true; //when any unit is removed ( merged ), then this means that auto countdown can work
    }

    public Unit GetUnitWithCoordinates((int, int) gridCoordinates)
    {
        return unitsArray[gridCoordinates.Item1][gridCoordinates.Item2];
    }

    #endregion

    #region UNIT_GENERATION_METHODS

    private void TryGenerateBoxInFirstFreeSlot()
    {
        for (int i = 0; i < unitsArray.Length; i++)
        {
            for (int j = 0; j < unitsArray[i].Length; j++)
            {
                if (unitsArray[i][j] == null) //if the slot is free
                {
                    GenerateBoxOnGridWithCoordinates(i, j, 0);
                    return;
                }
            }
        }
    }

    private void GenerateBoxOnGridWithCoordinates(int i, int j, int lvl)
    {
        GameObject unitGameObject = Instantiate(unitSample.gameObject);
        unitGameObject.transform.rotation = Quaternion.identity;
        Unit unit = unitGameObject.GetComponent<Unit>();
        unit.SetLevel(lvl);
        unit.UnitCoordinates = (i, j);
        unit.SetUnitPositionAndScale(unitSlotsArray[i][j].transform);
        unitsArray[i][j] = unit;
    }

    public void LoadBoardUnitsFromPlayerData(PlayerData pd)
    {
        for (int i = 0; i < pd.Units.Length; i++)
        {
            for (int j = 0; j < pd.Units[i].GeetLength(); j++)
            {
                var lvl = pd.Units[i][j].GetLevel();
                if (lvl != -1)
                {
                    GenerateBoxOnGridWithCoordinates(i, j, lvl);
                }
            }
        }
    }
    #endregion
}
