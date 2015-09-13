using System;
using System.Collections.Generic;
using Invert.Core.GraphDesigner;

public class SelectColorCommand : EditorCommand<ShellNodeTypeNode>, IDynamicOptionsCommand
{
    public override void Perform(ShellNodeTypeNode node)
    {
        node.DataBag["Color"] = SelectedOption.Value.ToString();
    }

    public override string CanPerform(ShellNodeTypeNode node)
    {
        if (node == null) return "Invalid node type";
        return null;
    }

    public IEnumerable<UFContextMenuItem> GetOptions(object item)
    {
        var node = item as ShellNodeTypeNode;
        if (node == null)
        {
            yield break;
        }
        var names = Enum.GetNames(typeof(NodeColor));
        foreach (var name in names)
        {
            yield return new UFContextMenuItem()
            {
                Checked = node.DataBag["Color"] == name,
                Value = name,
                Name = "Color/" + name
            };
        }
    }

    public UFContextMenuItem SelectedOption { get; set; }
    public MultiOptionType OptionsType { get; private set; }
}