using Godot;
using System;

public partial class Player : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	
	[Export]
	public float Speed { get; set; } = 200;
	
	[Export]
	public float ringRadius = 150;
	
	public float x0 = 0;
	public float y0 = 0;
	
	public float currentAngle = 0;
	public float Rotation = 0;
	
	public Vector2 ScreenSize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector.
		float d_theta = 0;

		if (Input.IsActionPressed("move_right"))
		{
			d_theta = -this.Speed * (float)delta;
			this.Rotation -= Mathf.DegToRad(d_theta)*(float)delta*this.Speed;
		}

		if (Input.IsActionPressed("move_left"))
		{
			d_theta = this.Speed * (float)delta;
			//Rotation += Mathf.DegToRad(d_theta)*(float)delta*this.Speed;
			Rotation += 10 * this.Speed * (float)delta;
			//MoveAndSlide();
		}
		this.currentAngle += d_theta;
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		
		float newY = this.y0 + this.ringRadius * Mathf.Sin(-Mathf.DegToRad(this.currentAngle));
		float newX = this.x0 + this.ringRadius * Mathf.Cos(Mathf.DegToRad(this.currentAngle));
		
		Position = new Vector2(
			x: Mathf.Clamp(newX, 0, ScreenSize.X),
			y: Mathf.Clamp(newY, 0, ScreenSize.Y)
		);
	}
	
	public void Start(Vector2 position)
	{
		Position = position;
		this.x0 = Position.X;
		this.y0 = Position.Y;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body.Name == "Mob"){
			Hide();
			EmitSignal(SignalName.Hit);
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
	}
}
