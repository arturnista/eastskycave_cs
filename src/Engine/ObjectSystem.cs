using Engine.Objects;

namespace Engine
{
    
    public class ObjectSystem
    {

        public List<Actor> Actors { get; private set; } = new List<Actor>();
        public List<Solid> Solids { get; private set; } = new List<Solid>();
        public List<Trigger> Triggers { get; private set; } = new List<Trigger>();
        public List<IGameObject> GameObjects { get; private set; } = new List<IGameObject>();

        private List<IGameObject> _gameObjectsToAdd = new List<IGameObject>();

        private List<IGameObject> _gameObjectsToRemove = new List<IGameObject>();
        private CoreEngine _coreEngine;

        public ObjectSystem()
        {
            _coreEngine = DI.Get<CoreEngine>();
        }

        public IGameObject Create(IGameObject gameObject)
        {
            _gameObjectsToAdd.Add(gameObject);
            return gameObject;
        }

        public IGameObject Destroy(IGameObject gameObject)
        {
            _gameObjectsToRemove.Add(gameObject);
            return gameObject;
        }

        public void Update()
        {
            foreach (var gameObject in _gameObjectsToAdd)
            {
                if (gameObject is Actor actor)
                {
                    Actors.Add(actor);
                }
                else if (gameObject is Solid solid)
                {
                    Solids.Add(solid);
                }
                else if (gameObject is Trigger trigger)
                {
                    Triggers.Add(trigger);
                }

                GameObjects.Add(gameObject);
            }
            _gameObjectsToAdd.Clear();

            var objectsToDelete = new List<IGameObject>(_gameObjectsToRemove);
            _gameObjectsToRemove.Clear();

            foreach (var gameObject in objectsToDelete)
            {
                if (gameObject is Actor actor)
                {
                    Actors.Remove(actor);
                }
                else if (gameObject is Solid solid)
                {
                    Solids.Remove(solid);
                }
                else if (gameObject is Trigger trigger)
                {
                    Triggers.Remove(trigger);
                }

                GameObjects.Remove(gameObject);
                gameObject.OnDestroy();
            }
        }
        
    }

}