using NorskaLib.Spreadsheets;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
[CreateAssetMenu(fileName = "DataLevel", menuName = "DataLevel")]
public class DataLevel : SpreadsheetsContainerBase
{
    [SpreadsheetContent]
    [SerializeField] listLevel content;
    public listLevel Conten => content;
    public List<List<Level>> GetAllLevels()
    {
        List<List<Level>> alllevels = new List<List<Level>>();
        FieldInfo[] fields = typeof(List<Level>).GetFields(BindingFlags.Public | BindingFlags.Static);  
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType == typeof(List<Level>))
            {
                List<Level> levelList = (List<Level>)field.GetValue(this);
                if (levelList != null)
                {
                    alllevels.Add(levelList);
                }
            }
        }
        return alllevels;
    }
    [Serializable]
    public class Level
    {
        public string Colum1;
        public string Colum2;
        public string Colum3;
        public string Colum4;
        public string Colum5;
        public string Colum6;
        public string Colum7;
        public string Colum8;
        public string Colum9;
        public string Colum10;
        public List<string> GetAllColums()
        {
            return new List<string>
            {
                Colum1,Colum2, Colum3, Colum4,  Colum5, Colum6, Colum7, Colum8, Colum9, Colum10
            };
        }
    }
    [Serializable]
    public class listLevel
    {
        [SpreadsheetPage ("Level1")]
        public List<Level> lv1;
        [SpreadsheetPage("Level2")]
        public List<Level> lv2;
        [SpreadsheetPage("Level3")]
        public List<Level> lv3;
    }
}