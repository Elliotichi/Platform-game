using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	private float speed = 300.0f;
	private float jumpSpeed = -400.0f;
	private float acceleration = .05f;
	private float friction = .25f;
	private bool sprinting;
	private float lastDirection;
	private int jumpCount = 2;

	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		velocity.Y += gravity * (float)delta;
		if (IsOnFloor())
		{
			jumpCount = 2;
		}


		// Handle jump.
		if (Input.IsActionJustPressed("jump") && jumpCount > 0)
		{
			jumpCount--;
			velocity.Y = jumpSpeed;
		}

		if (Input.IsActionJustPressed("sprint")){
			sprinting = true;
		}

		// Get the input direction.
		float direction = Input.GetAxis("left", "right");
		float leftStrength = Input.GetActionStrength("left");
		float rightStrength = Input.GetActionStrength("right");

		if (leftStrength == 0f && rightStrength == 0f && IsOnFloor())
		{
			velocity.X = Mathf.Lerp(velocity.X, 0, friction);
			sprinting = false;
		}
		
		else
		{
			velocity.X = Mathf.Lerp(velocity.X, direction * speed, acceleration);
		}

		Velocity = velocity;
		MoveAndSlide();

	}

	public override void _Process(double delta)
	{

	}
}
