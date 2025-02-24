using UnityEngine;
public enum InputType
{
    AD,
    Arrow
}

public interface IInputHandler
{
    InputType Type { get; }
    Vector3 HandleInput();
}
