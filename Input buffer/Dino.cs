using Godot;
using System;

/// <summary>
/// The game's brave protagonist.
/// </summary>
public class Dino : Sprite
{
    private static readonly string JUMP_ACTION = "ui_select";
    
    private enum DinoState
    {
        Idle,
        Jumping,
        Running,
        Dead
    }
    private DinoState _state = DinoState.Idle;
    private AnimationPlayer _animator;

    /// <summary> Pixels per second the dino currently moves downward. </summary>
    [Export] private float _speed = 0;
    /// <summary> How many pixels per second squared the dino accelerates towards the ground at. </summary>
    [Export] private float _gravity = 2400f;
    /// <summary>
    /// Pixels per second downward the dino moves the moment it jumps.
    /// Recall that the coordinate system has the positive y axis point down, so this should be negative.
    /// </summary>
    [Export] private float _initial_jump_speed = -800f;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// </summary>
    public override void _Ready()
    {
        _animator = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    /// <param name="delta"> The elapsed time since the previous frame. </param>
    public override void _Process(float delta)
    {
        switch (_state)
        {
            case DinoState.Idle:
            {
                if (Input.IsActionJustPressed(JUMP_ACTION))
                {
                    _state = DinoState.Jumping;
                    _animator.Play("Run");
                }
                break;
            }
            case DinoState.Jumping:
            {
                Position += Vector2.Down * _speed * delta;

                if (Position.y < 0)
                {
                    // In the air
                    _speed += _gravity * delta;
                }
                else
                {
                    // Hit the ground
                    Position = Vector2.Zero;
                    _speed = 0;
                    _state = DinoState.Running;
                }
                break;
            }
            case DinoState.Running:
            {
                if (Input.IsActionJustPressed(JUMP_ACTION))
                {
                    _speed = _initial_jump_speed;
                    _state = DinoState.Jumping;
                }
                break;
            }

            default: throw new InvalidOperationException("Unhandled state: " + _state);
        }
    }
}
