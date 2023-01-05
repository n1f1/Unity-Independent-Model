using System.Collections.Generic;
using Model;

public class UpdatablesContainer
{
    private readonly LinkedList<IUpdatable> _simulations = new();

    public void Add(IUpdatable updatable) => 
        _simulations.AddLast(updatable);

    public void Update(float deltaTime)
    {
        foreach (IUpdatable simulation in _simulations)
            simulation.UpdatePassedTime(deltaTime);
    }
}