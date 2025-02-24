using System;
using UnityEngine;

public interface ICarDown
{
    public event Action Die;
    void Sink(Transform _body);
    void Rising(Transform _body);
    
}
