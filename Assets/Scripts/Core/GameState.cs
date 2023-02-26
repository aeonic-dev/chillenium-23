namespace Core {
    public enum GameState {
        Menu,
        Hub,
        Level
    }
    
    static class GameStateExtensions {
        public static bool IsPlaying(this GameState state) {
            return state == GameState.Hub || state == GameState.Level;
        }
    }
}