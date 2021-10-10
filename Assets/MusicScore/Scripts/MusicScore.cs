using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEngine;
using Note;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
//using UnityEditor.Experimental.GraphView;

namespace MusicScore
{
    [Serializable]
    [DataContract]
    public class MusicScore
    {
        [DataMember(Name = "musicScore")]
        public List<Note.Note> musicScore;  //不需要写索引器，它自身就可以用[]去访问
        [DataMember(Name = "scoreOfNoteBar")]
        public int scoreOfNoteBar;  //这个乐谱中音符块的分数：成功按下音符块的分数
        [DataMember(Name = "scorePerSecOfNoteStrip")]
        public int scorePerSecOfNoteStrip;  //这个乐谱中音符条每秒的分数：按下1s能加的分数
        [DataMember(Name = "easyVel")]
        public float easyVel;  //这个乐谱简单难度的速度
        [DataMember(Name = "difficultVel")]
        public float difficultVel;  //这个乐谱困难难度的速度

        public MusicScore()
        {
            musicScore = new List<Note.Note>();
        }
        public MusicScore(int scoreOfNoteBar, int scorePerSecOfNoteStrip)
        {
            musicScore = new List<Note.Note>();
            this.scoreOfNoteBar = scoreOfNoteBar;
            this.scorePerSecOfNoteStrip = scorePerSecOfNoteStrip;
        }
    }

    public class MusicScoreManager
    {
        public static MusicScore musicScore = new MusicScore();
        public static void ExportToBinary(string name)    //把musicScore导出为binary格式文件，进行数据持久化，导出的位置在一个固定的文件夹内，name是文件名
        {
            string filepath = Directory.GetCurrentDirectory() + "\\" + name + ".bin";
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, musicScore);
            }
        }

        public static void ImportFromBinary(string name)  //因为我们的乐谱binary是放在固定文件夹里的，所以只需给个参数:文件名name就可以了，name就是文件名，把这个文件里的binary对象：MusicScore导入，赋值给静态成员musicScore
        {
            string filepath = Directory.GetCurrentDirectory() + "\\" + name + ".bin";
            if (File.Exists(filepath))
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    MusicScore msobj = (MusicScore)bf.Deserialize(fs);
                    musicScore = msobj;
                }
            }
        }

        public static void ExportToJSON(string name)  //把musicScore导出为JSON格式文件，进行数据持久化，导出的位置在一个固定的文件夹内，name是文件名
        {
            string filepath = Directory.GetCurrentDirectory() + "\\" + name + ".json";
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(MusicScore));
                jsonSerializer.WriteObject(fs, musicScore);
            }
        }

        public static void ImportFromJSON(string name)  //因为我们的乐谱json是放在固定文件夹里的，所以只需给个参数:文件名name就可以了，name就是文件名，把这个文件里的JSON对象：MusicScore导入，赋值给静态成员musicScore
        {
            //string filepath = Directory.GetCurrentDirectory() + "\\" + name + ".json";
            string filepath = "Assets\\MusicScore\\MusicScores\\"+name+"\\"+name+".json";
            if (File.Exists(filepath))
            {
                using (FileStream fs = new FileStream(filepath, FileMode.Open))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(MusicScore));
                    MusicScore jsonObject = (MusicScore)jsonSerializer.ReadObject(fs);
                    musicScore = jsonObject;
                }
            }

        }

    }
}
