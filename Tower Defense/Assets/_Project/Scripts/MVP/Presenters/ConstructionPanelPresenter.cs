using System;
using UnityEngine;
using _Project.Scripts.GameEntities;
using _Project.Scripts.Services;

namespace _Project.Scripts.MVP
{
    public class ConstructionPanelPresenter : IDisposable
    {
        private readonly ITowerConstructionService _towerConstructionService;
        private readonly ConstructionPanelView _constructionPanelView;
        private readonly ScreenInputHandler _screenInputHandler;
        
        private Vector2 _buildPosition =  Vector2.zero;

        public ConstructionPanelPresenter(
            ConstructionPanelView constructionPanelView,
            ScreenInputHandler screenInputHandler,
            ITowerConstructionService towerConstructionService)
        {
            _constructionPanelView = constructionPanelView;
            _screenInputHandler = screenInputHandler;
            _towerConstructionService = towerConstructionService;
        }

        public void Dispose()
        {
            _constructionPanelView.OnConstructButtonClicked -= OnConstructButtonClickedHandler;
            _screenInputHandler.OnBuildableClicked -= OnBuildableClickedHandler;
            _towerConstructionService.OnTowerConstructed -= OnTowerConstructedHandler;        
        }

        public void Init()
        {
            _constructionPanelView.OnConstructButtonClicked += OnConstructButtonClickedHandler;
            _screenInputHandler.OnBuildableClicked += OnBuildableClickedHandler;
            _towerConstructionService.OnTowerConstructed += OnTowerConstructedHandler;        
        }

        private void OnTowerConstructedHandler()
        {
            _constructionPanelView.Hide();
        }

        private void OnConstructButtonClickedHandler(TowerType towerType)
        {
            _towerConstructionService.TryConstructTower(towerType, _buildPosition);
        }

        private void OnBuildableClickedHandler(Vector2 position)
        {
            _buildPosition = position;
            _constructionPanelView.MoveCanvasTo(position);
            _constructionPanelView.Show();
        }
    }
}