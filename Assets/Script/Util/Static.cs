using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    string enemyName;
    public string EnemyName {  get { return enemyName; } }
    int enemyCount;
    public int EnemyCount { get { return enemyCount; } }

    public EnemyData(string name, int c)
    {
        enemyName = name; 
        enemyCount = c;
    }
}

public class WaveData
{
    List<EnemyData> enemyList;
    public List<EnemyData> EnemyList {  get { return enemyList; } }

    public WaveData(List<EnemyData> enemyList) {  this.enemyList = enemyList; }
}

public class Static
{
    public static List<WaveData> WaveLists = new List<WaveData>()
    {
        new WaveData( new List<EnemyData>()
        {
            new EnemyData("EnemyExample",2),
            new EnemyData("EnemyExample",3),
            new EnemyData("EnemyExample",4),
            new EnemyData("EnemyExample",6),
            new EnemyData("EnemyExample",8),
            new EnemyData("EnemyExample",10),
            new EnemyData("EnemyExample",12),
            new EnemyData("EnemyExample",18),
            new EnemyData("EnemyExample",20),
        }),
        new WaveData( new List<EnemyData>()
        {
            new EnemyData("EnemyExample",8),
            new EnemyData("EnemyExample",6)
        })
    };

    public static int buy_Unit_Price = 10;
    public static int unit_Update_Price = 10;
    public static int Replace_Tile_Price = 50;
    public static int exceed_Price = 100;
}
