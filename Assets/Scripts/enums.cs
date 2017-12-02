public enum MinoTypes
{
    Empty = 0,
    Green = 1,
    Yellow = 2,
    Blue = 3,
    Red = 4,
    Orange = 5,
    Pink = 6,
    Gray = 7,
    Black = 8
}

public enum MoveTypes
{
    None,
    Push,
    SlaminoPush
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum GameState
{
    GameStart,
    LoadingPlay,
    Play,
    Pause,
    LoadingGameOver,
    GameOver
}

public enum TouchSetting
{
    PointAndDrop, SwipeAndDrop
}