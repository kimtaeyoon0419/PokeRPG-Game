namespace PokeRPG.Manager
{

    // # System
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using TMPro;

    // # JSON
    using Newtonsoft.Json;

    // # Unity
    using UnityEngine;

    [System.Serializable]
    public class MonsterData
    {
        public MonsterData( string _name, string _type, string _hp, string _def, string _sDef, string _atk, string _sAtk )
        { name = _name; type = _type; hp = _hp; def = _def; sDef = _sDef; atk = _atk; sAtk = _sAtk; }

        public string name, type, hp, def, sDef, atk, sAtk;
    }

    public class DataManager : MonoBehaviour
    {

        public static DataManager instance = null; //ΩÃ±€≈Ê

        public TextAsset monsterData = null;

        public List<MonsterData> allMonsterDataList, playerMonsterList;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            string[] line = monsterData.text.Substring(0, monsterData.text.Length - 1).Split('\n');

            for(int i = 0; i < line.Length; i++)
            {
                string[] row = line[i].Split('\t');
                allMonsterDataList.Add(new MonsterData(row[0], row[1], row[2], row[3], row[4], row[5], row[6]));
            }

            Load();
        }

        public void Save()
        {
            string jdata = JsonConvert.SerializeObject(playerMonsterList);
            File.WriteAllText(Application.dataPath + "/02.WOOSEUNG/07.Data/PlayerMonsterText.txt", jdata);
        }

        public void Load()
        {
            string jdata = File.ReadAllText(Application.dataPath + "/02.WOOSEUNG/07.Data/PlayerMonsterText.txt");
            playerMonsterList = JsonConvert.DeserializeObject<List<MonsterData>>(jdata);
        }
    }
}


