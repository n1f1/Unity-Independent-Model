using System.Collections.Generic;
using GameModes.MultiPlayer.PlayerCharacter.Client.Movement;
using Model.Characters;
using Model.Characters.CharacterHealth;
using Model.Characters.Player;
using Simulation.Input;
using Simulation.Physics;
using UnityEngine;

namespace Simulation.Characters.Player
{
    public class PlayerSimulation : MonoBehaviour, IPlayerSimulation
    {
        [SerializeField] private bool _physicsDamageable;
        [SerializeField] private bool _botMovement;
        [SerializeField] private List<Vector2> _botMoveData;
        
        private IMovementInput _movementInput;

        public ISimulation<IMovable> Movable { get; set; }
        public ISimulation<ICharacterShooter> CharacterShooter { get; set; }
        public ISimulation<IDamageable> Damageable { get; set; }

        protected virtual void Awake()
        {
            _movementInput = new AxisInput();

#if UNITY_EDITOR
            if (_botMovement) 
                _movementInput = new BotInput(_botMoveData);
#endif
            
            Movable = gameObject.AddComponent<PlayerMovement>().Initialize(_movementInput);
            CharacterShooter = gameObject.AddComponent<PlayerShooter>();
            
            if(_physicsDamageable)
                Damageable = gameObject.AddComponent<DamageablePhysicsInteractableHolder>();
        }

        private void Update()
        {
            _botMoveData.Add(new Vector2(_movementInput.X, _movementInput.Y));
        }
    }
}