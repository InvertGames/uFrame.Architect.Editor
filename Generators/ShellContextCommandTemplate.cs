using Invert.Core.GraphDesigner;

public class ShellContextCommandTemplate : EditorCommand<GenericNode>
{
    [GenerateMethod(TemplateLocation.Both,true)]
    public override void Perform(GenericNode node)
    {
        
    }

    [GenerateMethod(TemplateLocation.Both, true)]
    public override string CanPerform(GenericNode node)
    {
        return null;
    }
}