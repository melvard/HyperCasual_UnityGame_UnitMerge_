using UnityEngine;
using System;

[Serializable]
public class PlayerData
{
    public Row[] Units;
    public int Level;
    public float ScoresReached;


    public PlayerData(Unit[][] unitsArray, int lvlReached, float scoresReached)
    {
        Units = new Row[unitsArray.Length];
        Level = lvlReached;
        ScoresReached = scoresReached;

        for (int i = 0; i < unitsArray.Length; i++)
        {
            Units[i] = new Row(unitsArray[i].Length);
        }

        for (int i = 0; i < unitsArray.Length; i++)
        {
            for (int j = 0; j < unitsArray[i].Length; j++)
            {
                if(unitsArray[i][j] == null)
                {
                    Units[i][j] = new CellInfo(-1);
                }
                else
                {
                    Units[i][j] = new CellInfo(unitsArray[i][j].Level);
                }
                    
            }
        }
    }

}

[Serializable]

public class Row
{
    [SerializeField] private CellInfo[] cellsInfo;

    public Row(int cellCount)
    {
        cellsInfo = new CellInfo[cellCount];
    }

    public int GeetLength() => cellsInfo.Length;

    public CellInfo this[int index]
    {
        get
        {
            return cellsInfo[index];
        }
        set
        {
            cellsInfo[index] = value;
        }
    }

}

[System.Serializable]

public class CellInfo
{
    [SerializeField] private int level;

    public CellInfo(int lvl)
    {
        level = lvl;
    }

    public int GetLevel() => level;
}

