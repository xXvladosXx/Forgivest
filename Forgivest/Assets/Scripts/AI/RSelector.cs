using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class RSelector : Node
{
    bool shuffled = false;
    public RSelector(string n)
    {
        name = n;
    }

    public override Status Process()
    {
        if (!shuffled)
        {
            Children.Shuffle();
            shuffled = true;
        }

        Status childstatus = Children[CurrentChild].Process();
        if (childstatus == Status.RUNNING) return Status.RUNNING;

        if (childstatus == Status.SUCCESS)
        {
            CurrentChild = 0;
            shuffled = false;
            return Status.SUCCESS;
        }

        CurrentChild++;
        if (CurrentChild >= Children.Count)
        {
            CurrentChild = 0;
            shuffled = false;
            return Status.FAILURE;
        }

        return Status.RUNNING;
    }

}
