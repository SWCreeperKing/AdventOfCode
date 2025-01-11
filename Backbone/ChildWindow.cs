namespace Backbone;

public abstract class ChildWindow(string name)
{
    public readonly string Name = name;
    
    public abstract void Init();
    public abstract void Update();
    public abstract void Render();
}