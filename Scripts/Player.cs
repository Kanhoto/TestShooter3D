using Godot;
using System;

public class Player : KinematicBody
{
    [Export]
    float speed = 7.0f;
    [Export]
    float acceleration = 15f;
    [Export]
    float air_acceleration = 5f;
    [Export]
    float gravity = 0.98f;
    [Export]
    float max_terminal_velocity = 54f;
    [Export]
    float jump_power = 20f;

    [Export(PropertyHint.Range, "0.1,1.0")]
    
    float mouse_sensitivity = 0.3f;
    [Export(PropertyHint.Range, "-90,0,1")]
    
    float min_pitch = -90f;
    [Export(PropertyHint.Range, "0,90,1")]
    
    float max_pitch = 90f;

    public Vector3 velocity;
    private float y_velocity;

    private Spatial pivot;
    private Camera camera;

    /// <summary> Called when the node enters the scene tree for the first time </summary>
    public override void _Ready()
    {
        pivot = GetNode<Spatial>("Pivot");
        camera = GetNode<Camera>("Pivot/SpringArm/Camera");
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    /// <summary> Called every frame. 'delta' is the elapsed time since the previous frame </summary>
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
            Input.SetMouseMode(Input.MouseMode.Visible);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if(@event is InputEventMouseMotion motionEvent)
        {
            Vector3 rotDeg = RotationDegrees;
            rotDeg.y -= motionEvent.Relative.x * mouse_sensitivity;
            RotationDegrees = rotDeg;

            rotDeg = pivot.RotationDegrees;
            rotDeg.x += motionEvent.Relative.y * mouse_sensitivity;
            rotDeg.x = Mathf.Clamp(rotDeg.x, min_pitch, max_pitch);
            pivot.RotationDegrees = rotDeg;
        }
    }

    /// <summary> Called every frame. 'delta' is the elapsed time since the previous frame. </summary>
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        handle_movement(delta);
    }

    /// <summary> Handle the movement of player and action of player </summary>
    private void handle_movement(float delta)
    {
        Vector3 direction = new Vector3(Vector3.Zero);

        if(Input.IsActionPressed("forward"))
            direction += Transform.basis.z;
        if(Input.IsActionPressed("backward"))
            direction -= Transform.basis.z;
        if(Input.IsActionPressed("left"))
            direction += Transform.basis.x;
        if(Input.IsActionPressed("right"))
            direction -= Transform.basis.x;
        direction = direction.Normalized();

        float accel = IsOnFloor() ? acceleration : air_acceleration;
        velocity = velocity.LinearInterpolate(direction * speed, accel*delta);

        if(IsOnFloor())
            y_velocity = -0.01f; // apply a small ammout of downward force if on floor
        else
            y_velocity = Mathf.Clamp(y_velocity-gravity, -max_terminal_velocity, max_terminal_velocity);
        
        if(Input.IsActionJustPressed("jump") && IsOnFloor())
            y_velocity = jump_power;
        
        velocity.y = y_velocity;
        velocity = MoveAndSlide(velocity, Vector3.Up);
    }
}
