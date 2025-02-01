using Godot;
public partial class Movable : CharacterBody2D
{
    public bool shouldMove = false;
    public NavigationAgent2D navigationAgent2D;
	public AnimatedSprite2D animatedSprite2D;
    public CollisionShape2D collisionShape2D;
	public TileMapLayer terrain;
	public Vector2[] desiredPath;
	public int currentPathIndex = 0;
	public Vector2 desiredLocation = Vector2.Zero;

	public TextureProgressBar healthBar;
	public float speed = 35;
    public Character character;

    public override void _PhysicsProcess(double delta) {
        if (!shouldMove) {
			return;
		}

		if (character.health != character.max_health) {
			var healthPercentage = (float) character.health / (float) character.max_health;

			healthBar.Value = (int) (healthPercentage * 100);
		}

		var currentAgentPosition = GlobalPosition;
       
		var nextPathPosition = desiredPath[currentPathIndex];
        nextPathPosition = terrain.ToGlobal(nextPathPosition);
	    navigationAgent2D.TargetPosition = nextPathPosition;
		var newVelocity = currentAgentPosition.DirectionTo(nextPathPosition) * speed;
   
		if (newVelocity.X < 0) {
			animatedSprite2D.FlipH = true;
		} else {
			animatedSprite2D.FlipH = false;
		}

		if (navigationAgent2D.AvoidanceEnabled) {
			navigationAgent2D.SetVelocity(newVelocity);
		} else {
			_on_navigation_agent_2d_velocity_computed(newVelocity);
		}
	
		if (navigationAgent2D.IsNavigationFinished())
		{
			if (currentPathIndex < desiredPath.Length - 1) {
				currentPathIndex++;
			} else {
				desiredLocation = Vector2.Zero;
				animatedSprite2D.FlipH = false;
				shouldMove = false;
				currentPathIndex = 0;
				desiredPath = new Vector2[0];
				animatedSprite2D.Play("idle");

                Highlighter.Instance.clearHighlights();
				return;
			}
		
		}

		MoveAndSlide();
    }

    public void _on_navigation_agent_2d_velocity_computed(Vector2 safe_velocity)
	{

		Velocity = safe_velocity;
	}

	public void startMovingTowardsPoint(Vector2 point, Vector2[] path, TileMapLayer terrain) {
		//desiredLocation = point;
		shouldMove = true;
		desiredPath = path;
		//animatedSprite2D.Play("walk");
		this.terrain = terrain;
	}
}