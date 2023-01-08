using System.Threading.Tasks;
using poetools;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Services
{
    public static EventBus EventBus;
}

public struct LoadLevelEvent
{
    public string LevelName;
}
    
/// <summary>
/// This class should be the first one that is loaded in the game.
/// It should persist for the entire application lifetime, only being destroyed when the application quits.
/// It controls the startup, updating, and shutdown of the game sub-systems.
/// </summary>
public class Entrypoint : MonoBehaviour
{
    #region Initialization Validation

    private static bool _initialized;
        
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _initialized = false;
    }
            
    private void Awake()
    {
        Initialize();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static async void CheckForAwake()
    {
        if (_initialized == false)
        {
            Debug.LogWarning("Entrypoint must be initialized before anything else!");
            Application.Quit();
#if UNITY_EDITOR
            Debug.Log($"{"[EDITOR ONLY]".Bold()} Loading Entrypoint...");
            string originalScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Entrypoint", LoadSceneMode.Single);
            await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
            Debug.Log($"{"[EDITOR ONLY]".Bold()} Trying to load {originalScene} after Entrypoint...");
            var loadLevelEvent = new LoadLevelEvent { LevelName = originalScene };
            Services.EventBus.Invoke(loadLevelEvent, "Editor Entrypoint Setup");
#endif
        }
    }

    #endregion

    [SerializeField] private FootstepPlayer footstepPlayer;
    
    private void Initialize()
    {
        _initialized = true;
            
        // Basic implementation of scene persistence. Could move to a dedicated persistent scene, but that is hard.
        DontDestroyOnLoad(this);
        
        // Demo of how we could implement cross-cutting concerns.
        // Ensures global access, polymorphism, and control over construction order + dependencies.
        Services.EventBus = new EventBus();
        
        footstepPlayer.Initialize();

        Services.EventBus.AddListener<LoadLevelEvent>(e =>
        {
            SceneManager.LoadScene(e.LevelName);
        }, "Level Loader");
    }

    private void OnDestroy()
    {
        footstepPlayer.Dispose();
    }
}