using Godot;
using System;

public class Gun : StaticBody
{
    [Export]
    float shoot_power = 20f;

    private PackedScene bulletScene;
    private Spatial bulletSpawnPoint;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bulletSpawnPoint = GetNode<Spatial>("BulletSpawnPoint");
        bulletScene = ResourceLoader.Load<PackedScene>("res://Scenes/Asteroid.tscn");
        Input.SetMouseMode(Input.MouseMode.Captured);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(Input.IsActionJustPressed("ui_cancel"))
            Input.SetMouseMode(Input.MouseMode.Visible);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        handle_movement(delta);
    }

    private async void handle_movement(float delta)
    {
        if(Input.IsActionPressed("shoot"))
        {
            RigidBody bullet = bulletScene.Instance<RigidBody>();
            GetTree().Root.AddChild(bullet);

            // set position of bullet
            bullet.GlobalTransform = bulletSpawnPoint.GlobalTransform;
            // apply impulse
            bullet.ApplyCentralImpulse(bulletSpawnPoint.GlobalTransform.basis.y * shoot_power);

            // 5 seconds dynamic yield
            await ToSignal(GetTree().CreateTimer(5), "timeout");
            bullet.QueueFree(); // Destroy bullet
            /*
            if(Input.GetMouseMode() != Input.MouseMode.Captured)
                Input.SetMouseMode(Input.MouseMode.Captured);*/
        }
    }
}
