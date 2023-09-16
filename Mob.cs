using Godot;
using System;

public partial class Mob : RigidBody2D
{
	[Signal]
	public delegate void HitEventHandler();
	
	[Export]
	public float Speed { get; set; } = 200;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		animatedSprite2D.Play(mobTypes[1]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		var velocity = Vector2.Zero;
		if(Position.X < 960){
			velocity.X += 1;
			animatedSprite2D.Play(mobTypes[1]);
		}
		if(Position.X > 960) {
			velocity.X -= 1;
			animatedSprite2D.Play(mobTypes[1]);
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		if(Position.Y > 540) {
			velocity.Y -= 1;
			animatedSprite2D.Play(mobTypes[2]);
			animatedSprite2D.FlipH = velocity.X < 0;
		}
		if(Position.Y < 540) {
			velocity.Y += 1;
			animatedSprite2D.Play(mobTypes[2]);
		}
		Position += velocity * (float)delta * this.Speed;
		Position = new Vector2(Position.X, Position.Y);
	}
	public void Start(Vector2 position)
	{
		Position = position;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
	private void OnBodyEntered(Node2D body)
	{
		Hide();
		EmitSignal(SignalName.Hit);
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
