using System;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class SceneLoaderService : ISceneLoaderService
    {
        public void LoadBattleScene()
        {
            SceneManager.LoadScene("BattleScene");
        }
        
        public void LoadMenuScene()
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}