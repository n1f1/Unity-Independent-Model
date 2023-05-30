using System;
using System.Collections.Generic;
using System.Numerics;

namespace Server.Simulation.Physics
{
    internal class PhysicsSimulation : IPhysicsSimulation
    {
        private readonly Dictionary<IRigidbody, object> _collidableBodies = new();
        private readonly Dictionary<object, IRigidbody> _objectToRigidbody = new();
        private readonly Dictionary<IRigidbody, object> _rigidbodyToObject = new();
        private readonly List<RigidBody> _rigidbodies = new();
        private Dictionary<IRigidbody, ICollision> _collisions = new();

        public void Update(float time)
        {
            for (var i = 0; i < _rigidbodies.Count; i++)
            {
                for (int j = i + 1; j < _rigidbodies.Count; j++)
                {
                    if (_collisions.ContainsKey(_rigidbodies[i]) || _collisions.ContainsKey(_rigidbodies[j]))
                        Process(_rigidbodies[i], _rigidbodies[j]);
                }
            }
        }

        private void Process(RigidBody rigidbody, RigidBody rigidbody1)
        {
            Vector3 position = rigidbody.Position;
            Vector3 position2 = rigidbody1.Position;
            position.Y = 0;
            position2.Y = 0;

            if (Vector3.Distance(position, position2) < rigidbody.Radius + rigidbody1.Radius)
            {
                TryInvokeCollision(rigidbody, rigidbody1);
                TryInvokeCollision(rigidbody1, rigidbody);
            }
        }

        private void TryInvokeCollision(RigidBody rigidbody, RigidBody rigidbody1)
        {
            if (_collisions.ContainsKey(rigidbody))
                _collisions[rigidbody].Collide(_collidableBodies[rigidbody1]);
        }

        public IRigidbody CreateCapsuleRigidbody(float radius)
        {
            RigidBody rigidBody = new RigidBody(radius);
            _rigidbodies.Add(rigidBody);

            return rigidBody;
        }

        public void RegisterCollidable(IRigidbody rigidbody, object body)
        {
            _collidableBodies.Add(rigidbody, body);
            _objectToRigidbody.Add(body, rigidbody);
            _rigidbodyToObject.Add(rigidbody, body);
        }

        public void AddCollision(IRigidbody rigidbody, ICollision collision)
        {
            _collisions.Add(rigidbody, collision);
        }

        public void Remove(IRigidbody rigidBody)
        {
            Remove(_rigidbodyToObject[rigidBody]);
        }

        public void Remove(object body)
        {
            IRigidbody rigidbody = _objectToRigidbody[body];
            _rigidbodies.Remove((RigidBody) rigidbody);

            if (_collisions.ContainsKey(rigidbody))
                _collisions.Remove(rigidbody);

            _objectToRigidbody.Remove(body);
        }
    }
}