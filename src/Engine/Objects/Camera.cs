using System.Numerics;
using Raylib_cs;

namespace Engine.Objects
{

    public class Camera : IGameObject
    {
        private int _cameraWidth;
        private int _cameraHalfWidth;
        private int _cameraHeight;
        private int _cameraHalfHeight;
        private Camera2D _camera2D;
        private IGameObject _followTarget;
        private CoreEngine _coreEngine;
        private IMap _map;

        public Camera2D Camera2D => _camera2D;
        public Vector2 Position { get; private set; }

        public Camera(Camera2D camera)
        {
            _camera2D = camera;
            _followTarget = null;

            _cameraWidth = GameStaticData.WindowWidth / GameStaticData.Zoom;
            _cameraHeight = GameStaticData.WindowHeight / GameStaticData.Zoom;
            
            _cameraHalfWidth = _cameraWidth / 2;
            _cameraHalfHeight = _cameraHeight / 2;

            _coreEngine = DI.Get<CoreEngine>();
            _coreEngine.OnPostFrame += OnPostFrame;
        }

        public void SetFollower(IGameObject followTarget)
        {
            _followTarget = followTarget;
        }

        public void SetMap(IMap map)
        {
            _map = map;
        }

        public void OnDestroy()
        {
            _coreEngine.OnPostFrame -= OnPostFrame;
        }

        private void OnPostFrame(float deltatime)
        {
            if (_followTarget == null) return;
            
            var level = _map.CurrentLevel;

            var position = _followTarget.Position + (Vector2.UnitY * GameStaticData.TileSize);

            var xMinBound = position.X - _cameraHalfWidth;
            var xMaxBound = position.X + _cameraHalfWidth;

            var yMinBound = position.Y - _cameraHalfHeight;
            var yMaxBound = position.Y + _cameraHalfHeight;

            float levelWidth = level.MaxPosition.X - level.MinPosition.X;
            if (levelWidth > _cameraWidth)
            {
                if (xMinBound < level.MinPosition.X) position.X = level.MinPosition.X + _cameraHalfWidth;
                if (xMaxBound > level.MaxPosition.X) position.X = level.MaxPosition.X - _cameraHalfWidth;
            }
            else
            {
                position.X = levelWidth / 2;
            }

            float levelHeight = level.MaxPosition.Y - level.MinPosition.Y;
            if (levelHeight > _cameraHeight)
            {
                if (yMinBound < level.MinPosition.Y) position.Y = level.MinPosition.Y + _cameraHalfHeight;
                if (yMaxBound > level.MaxPosition.Y) position.Y = level.MaxPosition.Y - _cameraHalfHeight;
            }
            else
            {
                position.Y = levelHeight / 2;
            }

            position.X = (int)position.X;
            position.Y = (int)-position.Y;
            Position = position;
            _camera2D.target = Position;
        }

    }

}