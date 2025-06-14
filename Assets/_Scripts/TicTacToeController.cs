using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TicTacToeController : MonoBehaviour
{
    ///UI///
    [SerializeField]
    GameObject[] xArray;
    [SerializeField]
    GameObject[] oArray;
    [SerializeField]
    GameObject winLine;

    bool enemyTurnOver;
    int selectedNum;
    List<int> selectedNums = new List<int>();

    ///GAMEPLAY///
    List<string> playerNums = new List<string>
    {
        "036p", "04p", "057p", "13p", "1467p", "15p", "237p", "24p", "256p"
    };
    List<string> enemyNums = new List<string>
    {
        "036e", "04e", "057e", "13e", "1467e", "15e", "237e", "24e", "256e"
    };
    List<string> boardNums = new List<string>
    {
        "", "", "", "", "", "", "", "", ""
    };

    //Dictionary<string, int> nums = new Dictionary<string, int>()
    //{
    //    { "036", 0 }, {"04", 0 }, {"057", 0 }, {"13", 0 }, {"1467", 0 }, {"15", 0 }, {"237", 0 },{ "24", 0 }, { "256", 0 }
    //};

    void EnemyTurn()
    {
        enemyTurnOver = false;
        for(int i = 0; i < boardNums.Count; i++)
        {
            //int amount = 0;
            List<int> tempListP = new List<int>();
            List<int> tempListE = new List<int>();
            for (int j = 0; j < boardNums.Count; j++)
            {
                if (boardNums[j].Contains(i.ToString()))
                {
                    if (boardNums[j].Contains("e"))
                    {
                        tempListE.Add(j);
                    }
                    else
                    {
                        tempListP.Add(j);
                    }
                }
            }
            if (tempListE.Count == 2)
            {
                for (int j = 0; j < tempListE.Count; j++)
                {
                    for (int k = 0; k < enemyNums.Count; k++)
                    {
                        if (enemyNums[k].Contains(tempListE[j].ToString()) && enemyNums[k] != "")
                        {
                            selectedNum = k;
                            boardNums[k] = enemyNums[k];
                            enemyNums[k] = "";
                            enemyTurnOver = true;
                            break;
                        }
                    }
                }
            }
        }
        if(!enemyTurnOver)
        {
            
        }
    }

    void EnemyTurn1()
    {

    }

}
