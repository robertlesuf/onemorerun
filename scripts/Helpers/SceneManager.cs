using Godot;

public partial class SceneManager : Node
{
    private Node currentScene;

    // Singleton pattern
    private static SceneManager instance;
    public static SceneManager Instance => instance;

    public override void _EnterTree()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            QueueFree();
        }
    }

    /// <summary>
    /// Changes the main scene to the specified scene path.
    /// </summary>
    /// <param name="newScenePath">Path to the new scene.</param>
    public void changeScene(string newScenePath)
    {
        // Load the new scene
        PackedScene newScene = (PackedScene)GD.Load(newScenePath);
        if (newScene == null)
        {
            GD.PrintErr($"Failed to load scene: {newScenePath}");
            return;
        }

        GetTree().ChangeSceneToPacked(newScene);
    }
}