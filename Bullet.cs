using Godot;
using System;

public partial class Bullet : RigidBody2D
{
	[Export]
	public float Speed = 750;
	
	[Export]
	public float GlobalRoation = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[0]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var forwardDirection = new Vector2(0,-1).Rotated(this.GlobalRotation);
		Position += new Vector2(forwardDirection.X * this.Speed * (float)delta, forwardDirection.Y * this.Speed * (float)delta);
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[0]);
	}
	private void OnBodyEntered(Node body)
	{
		Hide();
	}
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
