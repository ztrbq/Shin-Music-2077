using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

namespace Score
{
    public static class Score
    {
        public static float totalScore { get; set; }  //总分

        public static int missNum { get; set; }   //miss掉的方块数

        public static int goodNum { get; set; }  //good级命中数

        public static int perfectNum { get; set; } //perfect评分方块数

        public static void Reset()
        {
            totalScore = 0;
            missNum = 0;
            goodNum = 0;
            perfectNum = 0;
        }
    }

}
