using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]

public static class SaveScript
{
    public static bool openedOnce = false;

    public static void SaveHighScore(int hs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Rocket_R1.kt";
        //Debug.Log("Path Created : " + path);
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        //FileStream stream2 = new FileStream(path, FileMode.Open);

        SaveData save = new SaveData(hs, openedOnce);

        if (File.Exists(path) && stream.Length > 0)
        {
            stream.Close();
            Debug.Log("File Exists"); 
            FileStream stream2 = new FileStream(path, FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(stream2);
            //int? tmphighScoreInFile = formatter.Deserialize(stream2) as int?;
            stream2.Close();

            { //dont need this
                /*
                if (tmphighScoreInFile <= highScoreOfPlayer)
                {
                    //stream2.Close();
                    FileStream stream3 = new FileStream(path, FileMode.Create);
                    formatter.Serialize(stream3, highScoreOfPlayer);
                    Debug.Log("File Created :  " + tmphighScoreInFile);
                    stream3.Close();
                }
                */
            }

            if(data.score <= save.score)
            {
                FileStream stream3 = new FileStream(path, FileMode.Create);
                formatter.Serialize(stream3, save);
                Debug.Log("File Created :  " + save.score);
                stream3.Close(); 
            }
        }
        else
        {
            formatter.Serialize(stream, save);
            stream.Close();
        } 
        //stream2.Close();
    }

    public static int LoadHighScore()
    {
        string path = Application.persistentDataPath + "/Rocket_R1.kt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            //stream.
            SaveData data = (SaveData)formatter.Deserialize(stream);
            //int? highScoreInFile = formatter.Deserialize(stream) as int?;
            openedOnce = data.openedOnce;
            stream.Close();

            return data.score;
        }

        else
        {
            Debug.Log("File Not Found");
            return 0;
        }
    }
} 

[System.Serializable]
public class SaveData
{
    public int score;
    public bool openedOnce;

    public SaveData(int sc, bool once)
    {
        score = sc;
        openedOnce = once;
    }
}
