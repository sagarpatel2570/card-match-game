using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace CardGame
{
    public class SaveLoadManager : MonoBehaviour
    {
        private const string game_data_id = "game_data_id";
        private GameInfo gameInfo;

        private List<ISave> saverList = new List<ISave>();
        private List<ILoad> loaderList = new List<ILoad>();
        private void Awake()
        {
            GlobalEvents.Register<GameFinishEvent>(GameFinish);
            GlobalEvents.Register<SaveGameInfoEvent>(SaveGame);
            GlobalEvents.Register<LoadGameInfoEvent>(LoadGame);
        }

        private void Start()
        {
            saverList = ComponentFinder.FindAll<ISave>();
            loaderList = ComponentFinder.FindAll<ILoad>();
            
            LoadGameData();
        }

        private void OnDestroy()
        {
            GlobalEvents.UnRegister<GameFinishEvent>(GameFinish);
            GlobalEvents.UnRegister<SaveGameInfoEvent>(SaveGame);
            GlobalEvents.UnRegister<LoadGameInfoEvent>(LoadGame);
        }

        private void LoadGameData()
        {
            var data = PlayerPrefs.GetString(game_data_id, "");
            if (string.IsNullOrEmpty(data))
            {
                gameInfo = new GameInfo();
            }
            else
            {
                gameInfo = JsonUtility.FromJson<GameInfo>(data);
            }
            
            GlobalEvents.Trigger(new LoadDataEvent(){gameInfo = gameInfo});
        }

        private void SaveGame(SaveGameInfoEvent obj)
        {
            foreach (var saver in saverList)
            {
                saver.Save(gameInfo);
            }
            SaveData(gameInfo);
        }

        private void SaveData(GameInfo gameInfo)
        {
            var data = JsonUtility.ToJson(gameInfo);
            PlayerPrefs.SetString(game_data_id,data);
            PlayerPrefs.Save();
        }
        
        private void LoadGame(LoadGameInfoEvent obj)
        {
            foreach (var loader in loaderList)
            {
                loader.Load(gameInfo);
            }
        }
        
        private void GameFinish(GameFinishEvent obj)
        {
            gameInfo = new GameInfo();
            
            foreach (var loader in loaderList)
            {
                loader.Load(gameInfo);
            }
            SaveData(gameInfo);
        }
    }
}

