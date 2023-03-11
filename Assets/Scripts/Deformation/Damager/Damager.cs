using System.Collections.Generic;
using Deformation;
using UnityEngine;

public class Damager : MonoBehaviour, IChanger
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
}