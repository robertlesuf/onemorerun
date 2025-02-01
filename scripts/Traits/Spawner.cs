using Godot;
public partial class Spawner: Node2D
{
    public Tween animation;

    public string roomType;
    public bool onTheRight = true; 

    public override void _Process(double delta)
    {
       if (CharacterSpawner.Instance.playableCharacter != null && Navigation.Instance.onTheSameTile(Position, CharacterSpawner.Instance.playableCharacter.Position)) {
           animation.Stop();
           var spawners = GetTree().GetNodesInGroup("spawners");

           foreach (Spawner spawner in spawners) {
               spawner.animation.Stop();
               spawner.QueueFree();
           }

            LevelGenerator.Instance.addSection("grass", onTheRight, roomType);
       }
    }
}