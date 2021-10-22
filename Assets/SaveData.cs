using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;


[System.Serializable]
public class SaveData
{



    [SerializeField]
    Buildings build_red;
    [SerializeField]
    Buildings build_orange;



    [SerializeField]
    IdleNumber moneyMain;



    const string filename = "coloridle.txt";
    public void save(IdleNumber n, List<Buildings> bs)
    {
        /*Debug.Log(Application.persistentDataPath + "/" + filename);
        string file = Application.persistentDataPath + "/" + filename;
        string fileData = "0";
        
        File.WriteAllText(file, fileData);*/
        //Application.ExternalCall("SyncFiles");

        //money_value = n.getValue();
        //money_mag = n.getMag();
        moneyMain = n;

        foreach (Buildings b in bs)
        {
            if (b.getName() == "Red")
            {
                build_red = b;
            }


        }

        string filePath = Application.persistentDataPath + "/" + filename;
        FileStream dataStream = new FileStream(filePath, FileMode.Create);
        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, this);
        dataStream.Close();
    }

    public List<Buildings> getBuilds()
    {
        List<Buildings>  bs = new List<Buildings>();
        if (build_red != null)
        {
            bs.Add(build_red);
        }

        if (build_orange != null)
        {
            bs.Add(build_orange);
        }


        return bs;
    }

    public IdleNumber getMoney()
    {
        return moneyMain;

        //return new IdleNumber(money_value, money_mag);
    }

    public SaveData load()
    {
        /*
        string file = Application.persistentDataPath + "/" + filename;
        string fileData = "0";
        if (File.Exists(file))
        {
            fileData = File.ReadAllText(file);
            int fDNo = Convert.ToInt32(fileData);
            fDNo++;
            fileData = fDNo.ToString();
        }*/
        string filePath = Application.persistentDataPath + "/" + filename;

        if (File.Exists(filePath))
        {
            // File exists 
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            SaveData saveData = converter.Deserialize(dataStream) as SaveData;

            dataStream.Close();
            return saveData;
        }
        else
        {
            // File does not exist
            Debug.LogError("Save file not found in " + filePath);
            return null;
        }
    }

}
