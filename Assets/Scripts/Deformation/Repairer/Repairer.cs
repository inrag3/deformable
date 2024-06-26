using System.Collections.Generic;
using Deformation;
using UnityEngine;

public class Repairer : MonoBehaviour, IChanger
{
    private IEnumerable<IDeformable> _deformables;

    public void Init(IEnumerable<IDeformable> deformables)
    {
        _deformables = deformables;
    }

    public void Change()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize(IEnumerable<IDeformable> value)
    {
        throw new System.NotImplementedException();
    }
}