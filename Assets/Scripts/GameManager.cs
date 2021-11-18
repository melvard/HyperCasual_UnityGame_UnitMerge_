using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : Singleton<GameManager>
{
    #region PRIVATE_FIELDS

    [SerializeField] [Tooltip("Max level taht per unit can reach by merging")] private int maxUnitLevel;
    [SerializeField] [Tooltip("How much will the generator's count go down per second ?")] private int timeCountDownSize;
    [SerializeField] [Tooltip("How much will the generator's count go down per click ?")] private int clickCountDownSize;

    public int MaxUnitLevel => maxUnitLevel;
    public int TimeCountDownSize => timeCountDownSize;
    public int ClickCountDownSize => clickCountDownSize;

    private string colorJsonNameInResources = "UnitColorInfo.json"; //unit colors in accordance with lvls are stored in .json file
    private static Dictionary<string, string> colorsDict; //dictionary will be used to store colors for with key lvl

    private BoardManager boardManager;
    private UnitManager unitManager;
    private ProgressBarController progressBarController;

    public int CurrenLevel => progressBarController.CurrenLevel;

    #endregion

    #region PROPERTIES

    public float InLevelScores => progressBarController.GetProgressbarValue();
    public float CurrenLevelMaxScores => progressBarController.GetProgressbarMaxValue();

    #endregion

    #region MONO_BEHAVIOUR_METHODS

    private void Start()
    {
        //load data from json file
        PlayerData playeraData = SaveSystem.LoadData();

        //collecting all sub-managers
        boardManager = BoardManager.Instance;
        unitManager = UnitManager.Instance;
        progressBarController = ProgressBarController.Instance;

        boardManager.FormBoard(playeraData);

        if (playeraData != null)
        {
            progressBarController.LoadInfoFromPlayerData(playeraData);
        }
    }


    // When we attempt to QuitApplication the playerData will be saved in the persistantDataPath.
    // This method will also be called on changing the scene
    public void OnApplicationQuit()
    {
        PlayerData pd = new PlayerData(unitManager.unitsArray, CurrenLevel, progressBarController.GetProgressbarValue());
        SaveSystem.SaveData(pd);
    }
    #endregion

    #region COLOR_FROM_JSON

    // Read colors from json file and assign information containing dictionary to an <out> parameter
    private static void ReadColorsFromJson(string path, out Dictionary<string, string> jsonDict)
    {

        string jsonText = Resources.Load<TextAsset>(path.Replace(".json", "")).text;
        jsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText);

    }

    // From dictionary get the color corresponding the unit lvl
    public Color GetColorFromDict(int lvl)
    {
        if (colorsDict == null)
        {
            ReadColorsFromJson(colorJsonNameInResources, out colorsDict);
        }
        Color levelColor;
        var colorHexcode = colorsDict[lvl.ToString()];
        ColorUtility.TryParseHtmlString(colorHexcode, out levelColor);
        return levelColor;
    }

    #endregion

}

