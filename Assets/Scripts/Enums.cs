public static class Enums
{
    public enum GameColors
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple,
        Orange,
        Pink,
        Brown,
        Gray
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        LevelComplete
    }

    public enum DishType
    {
        Appetizer,
        MainCourse,
        Dessert,
        Beverage
    }

    public enum WaiterState
    {
        Idle,
        Waiting,
        Serving,
        Returning
    }

    public enum ElevatorState
    {
        Moving,
        Stopped,
        Loading,
        Unloading
    }

    public enum SlotState
    {
        Empty,
        Occupied,
        Locked,
        Highlighted
    }
} 