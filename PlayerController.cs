using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{
	private float speed = 150.0f;
	private float sprintSpeed = 400.0f;
	private float jumpSpeed = -400.0f;
	private float wallJumpSpeed = 250.0f;
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

		float direction = Input.GetAxis("left", "right");
		float leftStrength = Input.GetActionStrength("left");
		float rightStrength = Input.GetActionStrength("right");
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

		if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RaycastLeft").IsColliding() && IsOnWallOnly()){
			velocity.Y = jumpSpeed;
			velocity.X = wallJumpSpeed;
			GD.Print("i wall jumped!");
			
		}

		if (Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RaycastRight").IsColliding() && IsOnWallOnly()){
			velocity.Y = jumpSpeed;
			velocity.X = -wallJumpSpeed;
			GD.Print("i wall jumped!");
		}

		if (Input.IsActionJustPressed("sprint"))
		{
			sprinting = true;
		}

		if (direction != lastDirection && sprinting == true)
		{
			sprinting = false;
		}

		if (leftStrength == 0f && rightStrength == 0f && IsOnFloor())
		{
			velocity.X = Mathf.Lerp(velocity.X, 0, friction);
			sprinting = false;
			
		}

		else
		{
			switch (sprinting)
			{
				case true:
					velocity.X = Mathf.Lerp(velocity.X, direction * sprintSpeed, acceleration);
					break;
				default :
					velocity.X = Mathf.Lerp(velocity.X, direction * speed, acceleration);
					break;

			}
		}
		lastDirection = direction;
		Velocity = velocity;
		MoveAndSlide();

	}

	public override void _Process(double delta)
	{

	}
}
