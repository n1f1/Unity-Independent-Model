# Unity-Independent-Model
This project focuses on the development of a plain C# game logic model that is fully independent of any game engine and uses UnityEngine to run this model.

Active [branch.](https://github.com/n1f1/Unity-Independent-Model/tree/develop)

### Architecture
The main focus is on object-oriented design without the use of MV* patterns or any event-based approach to interact with the model.

When I started developing [some netcode](https://github.com/n1f1/SimpleGameNetworking), I needed a test game suitable for client-server real-time multiplayer. This one was a perfect match since it has an independent model that can be used on a server.
