using UnityEngine;
public enum InputType
{
    AD,
    Arrow,
    ADObjection,
    ArrowObjection
}

public interface IInputHandler
{
    InputType Type { get; }
    Vector3 HandleInput();
}
