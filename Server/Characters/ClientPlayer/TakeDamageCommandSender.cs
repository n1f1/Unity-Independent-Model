using System;
using GameModes.MultiPlayer;
using GameModes.MultiPlayer.PlayerCharacter.Common.Health;
using Model.Characters.CharacterHealth;
using Server.Simulation.CommandSend;

namespace Server.Characters.ClientPlayer
{
    internal class TakeDamageCommandSender : IDamageable
    {
        private readonly SimulationCommandSender<TakeDamageCommand> _commandHandler;
        private readonly Health _health;

        public TakeDamageCommandSender(SimulationCommandSender<TakeDamageCommand> commandHandler, Health health)
        {
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _health = health ?? throw new ArgumentNullException(nameof(health));
        }

        public void TakeDamage(float damage)
        {
            float amount = _health.Amount;
            _health.TakeDamage(damage);
            
            TakeDamageCommand command = new TakeDamageCommand(this, amount - _health.Amount);
            _commandHandler.Send(command);
        }

        public bool CanTakeDamage() => 
            _health.CanTakeDamage();
    }
}