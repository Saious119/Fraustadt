using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	
	[Export]
	public PackedScene BulletScene { get; set; }

	private int _score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
	}
	public void NewGame()
	{
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		_score = 0;
		
		//GetNode<AudioStreamPlayer>("Music").Play();

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		var hud = GetNode<HUD>("HUD");
		hud.UpdateScore(_score);
		hud.ShowMessage("Get Ready!");
		
		GetNode<Timer>("StartTimer").Start();
	}
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
		GetNode<Timer>("BulletTimer").Start();
	}
	private void OnMobTimerTimeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		float direction = mobSpawnLocation.Rotation+Mathf.Pi / 2;
		mob.Position = mobSpawnLocation.Position;
		
		direction += (float)GD.RandRange(-Mathf.Pi/4, Mathf.Pi/4);
		mob.Rotation = direction;
		
		var velocity = new Vector2((float)GD.RandRange(150.0,250.0), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
		
		mob.Start(mob.Position);
		
		AddChild(mob);
	}
	private void OnBulletTimerTimeout()
	{
		if(Input.IsActionPressed("left_click")){
			Player player = GetNode<Player>("Player");
			Bullet bullet = BulletScene.Instantiate<Bullet>();
			bullet.Position = player.Position;
			bullet.GlobalRoation = player.Rotation;
			bullet.Start(bullet.Position);
			AddChild(bullet);
		}
	}

}
