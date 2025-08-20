using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace CardGame
{
    public class LevelSelection : MonoBehaviour,IGameState<GameState>,ISave,ILoad
    {
        public List<LevelData> levelInfoDataList;
        public LevelInfoUI levelInfoUIPrefab;
        public RectTransform gridTransform;
        
        public GameState Type => GameState.LevelSelection;
        public LevelData CurrLevel { get; private set; }

        private IGameManager<GameState> gameManager;
        private bool isInitialize;

        public void Init(IGameManager<GameState> gameManager)
        {
            this.gameManager = gameManager;
            GlobalEvents.Register<NextLevelEvent>(NextLevel);
        }

        private void NextLevel(NextLevelEvent obj)
        {
            var index = levelInfoDataList.FindIndex(a => a == CurrLevel);
            index++;
            if (index >= levelInfoDataList.Count)
            {
                index =  levelInfoDataList.Count -1;
            }

            CurrLevel = levelInfoDataList[index];
            GlobalEvents.Trigger(new LevelSelectionEvent() { data = CurrLevel });
            GlobalEvents.Trigger(new LoadGameInfoEvent());

            gameManager.ChangeState(GameState.GamePlay);
        }

        private void OnDestroy()
        {
            GlobalEvents.UnRegister<NextLevelEvent>(NextLevel);
        }

        public void Enter()
        {
            gameObject.SetActive(true);
            if (!isInitialize)
            {
                foreach (var data in levelInfoDataList)
                {
                    var levelInfoUI = Instantiate(levelInfoUIPrefab, gridTransform);
                    levelInfoUI.Init(data, (levelInfo) =>
                    {
                        CurrLevel = data;
                        GlobalEvents.Trigger(new LevelSelectionEvent() { data = data });
                        gameManager.ChangeState(GameState.GamePlay);
                    });
                }

                isInitialize = true;
            }
        }

        public void UpdateState()
        {
          
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }

        public void Save(GameInfo gameInfo)
        {
            gameInfo.levelId = CurrLevel.name;
        }

        public void Load(GameInfo gameInfo)
        {
            var data = levelInfoDataList.Find(a => a.name == gameInfo.levelId);
            if (data != null)
            {
                CurrLevel = data;
                GlobalEvents.Trigger(new LevelSelectionEvent() { data = data });
            }
        }
    }
}

