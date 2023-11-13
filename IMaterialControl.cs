namespace MaterialSkin
{
    public interface IMaterialControl
    {
        int Depth { get; set; }
        MaterialSkinManager SkinManager { get; }
        MouseState MouseState { get; set; }

    }

    public enum MouseState
    {
        HOVER,
        DOWN,
        OUT
    }
    public enum ObjectSide
    {
        
        Right,
        Left

    }
    public enum MarginSide
    {
        Right,
        Left
        

    }
    public enum ButtonIconPositionX
    {
        Right,
        Center,
        Left
    }
    public enum ButtonIconPositionY
    {
        Top,
        Center,
        Bottom
    }

}