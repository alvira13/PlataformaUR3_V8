/*
    Fuente: https://www.youtube.com/watch?v=n8giZTfEBkg
    Este ha sido ligeramente modificado
*/

/********************* Librerias ********************/
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;


public static class Exportar_importar
{
    /**************** Funciones ******************/
    public static void SaveData<T>(T data, string path, string fileName)        //Guardar datos en JSON
    {
        string fullPath = "/"+path+"/";
        bool checkFolderExit = Directory.Exists(fullPath);
        if (checkFolderExit == false)
        {
            Directory.CreateDirectory(fullPath);
        }
        //string json = JsonUtility.ToJson(data);
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(fullPath + fileName + ".json", json);
        Debug.Log("Save date ok. " + fullPath);
    }
    public static T LoadData<T>(string path, string fileName)                   //Cargar datos JSON
    {
        string fullPath =  "/"+path+"/"+ fileName + ".json";
        if (File.Exists(fullPath))
        {
            string textJson = File.ReadAllText(fullPath);
            //var obj = JsonUtility.FromJson<T>(textJson);
            var obj = JsonConvert.DeserializeObject<T>(textJson);
            return obj;
        }
        else
        {
            Debug.Log("not file found on load data");
            return default;
        }
    }
}