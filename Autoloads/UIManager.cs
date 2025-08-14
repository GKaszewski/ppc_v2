using Godot;
using Godot.Collections;

namespace Mr.BrickAdventures.Autoloads;

public partial class UIManager : Node
{
    [Export] public Array<Control> UiStack { get; set; } = new();
    
    [Signal] public delegate void ScreenPushedEventHandler(Control screen);
    [Signal] public delegate void ScreenPoppedEventHandler(Control screen);

    public void PushScreen(Control screen)
    {
        if (screen == null)
        {
            GD.PushError($"Cannot push a null screen.");
            return;
        }
        
        UiStack.Add(screen);
        screen.Show();
        screen.SetProcessInput(true);
        screen.SetFocusMode(Control.FocusModeEnum.All);
        screen.GrabFocus();
        EmitSignalScreenPushed(screen);
    }
    
    public void PopScreen()
    {
        if (UiStack.Count == 0)
        {
            GD.PushError($"Cannot pop screen from an empty stack.");
            return;
        }
        
        var top = (Control)UiStack[^1];
        UiStack.RemoveAt(UiStack.Count - 1);
        top.Hide();
        top.SetProcessInput(false);
        EmitSignalScreenPopped(top);
        top.AcceptEvent();

        if (UiStack.Count > 0) ((Control)UiStack[^1]).GrabFocus();
    }
    
    public Control TopScreen() => UiStack.Count > 0 ? (Control)UiStack[^1] : null;
    
    public bool IsScreenOnTop(Control screen) => UiStack.Count > 0 && (Control)UiStack[^1] == screen;
    
    public bool IsVisibleOnStack(Control screen) => UiStack.Contains(screen) && screen.Visible;
    
    public void CloseAll()
    {
        while (UiStack.Count > 0)
            PopScreen();
    }

    public static void HideAndDisable(Control screen)
    {
        screen.Hide();
        screen.SetProcessInput(false);
    }

}