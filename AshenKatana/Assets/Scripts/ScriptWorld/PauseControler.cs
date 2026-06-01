using UnityEngine;

public class PauseControler : MonoBehaviour {
    public static bool IsGamePaused;

    public static void SetPause(bool pause) {
        IsGamePaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }
}