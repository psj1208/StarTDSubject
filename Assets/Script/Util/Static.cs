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
            new EnemyData("EnemyExample",5),
            new EnemyData("EnemyExample",3)
        }),
        new WaveData( new List<EnemyData>()
        {
            new EnemyData("EnemyExample",8),
            new EnemyData("EnemyExample",6)
        })
    };

    public static int buy_Unit_Price = 10;
    public static int unit_Update_Price = 10;
}
