using System.CodeDom;
using System.IO;
using System.Linq;
using System.Text;
using Invert.Core;
using Invert.Core.GraphDesigner;

using Invert.IOC;

public class PluginDesigner : DiagramPlugin
{
    public override void Initialize(UFrameContainer container)
    {
#if DEBUG && UNITY_EDITOR
        //container.RegisterInstance<IToolbarCommand>(new PrintPlugins(), "Json");
      
#endif
        container.RegisterInstance<IDiagramNodeCommand>(new SelectColorCommand(), "SelectColor");
        var pluginConfig = container
            .AddItem<ShellNodeSectionsSlot>()
            .AddItem<ShellNodeInputsSlot>()
            .AddItem<ShellNodeOutputsSlot>()
            .AddItem<TemplatePropertyReference>()
            .AddItem<TemplateMethodReference>()
            .AddItem<TemplateFieldReference>()
            .AddItem<TemplateEventReference>()
            .AddItem<ShellAcceptableReferenceType>()
            .AddItem<ShellConnectableReferenceType>()
            .AddTypeItem<ShellPropertySelectorItem>()
            .AddGraph<PluginGraphData,ShellPluginNode>("Shell Plugin")
            .Color(NodeColor.Green)
           
            .HasSubNode<IShellNode>()
            .HasSubNode<TypeReferenceNode>()
            .HasSubNode<ShellNodeConfig>()
            .HasSubNode<ScreenshotNode>()
#if UNITY_EDITOR
            .AddCodeTemplate<DocumentationTemplate>()
#endif
            ;
        container.AddNode<ScreenshotNode, ScreenshotNodeViewModel, ScreenshotNodeDrawer>("Screenshot");

        var shellConfigurationNode =
            container.AddNode<ShellNodeConfig, ShellNodeConfigViewModel, ShellNodeConfigDrawer>("Node Config")
                .HasSubNode<ShellNodeConfig>()
                .HasSubNode<ScreenshotNode>()
                .HasSubNode<ShellTemplateConfigNode>()
            ;
        shellConfigurationNode.AddFlag("Graph Type");

        container.AddNode<ShellTemplateConfigNode>("Code Template")
            .Color(NodeColor.Purple);
        
        
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeConfigTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfigSection, ShellNodeConfigReferenceSectionTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfigSection, ShellNodeConfigChildItemTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeAsGraphTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellPluginNode, ShellConfigPluginTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeConfigViewModelTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellNodeConfig, ShellNodeConfigDrawerTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellTemplateConfigNode, ShellNodeConfigTemplateTemplate>();
        RegisteredTemplateGeneratorsFactory.RegisterTemplate<IShellSlotType, ShellSlotItemTemplate>();
#if UNITY_EDITOR
        if (GenerateDocumentation)
        {
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<ShellPluginNode, DocumentationTemplate>();
            RegisteredTemplateGeneratorsFactory.RegisterTemplate<IDocumentable, DocumentationPageTemplate>();
        }
#endif
        

        container.Connectable<ShellNodeConfigSection, ShellNodeConfig>();
        container.Connectable<ShellNodeConfigSection, ShellNodeConfigSection>();
        container.Connectable<IShellNodeConfigItem, IShellNodeConfigItem>();
        container.Connectable<ShellNodeConfigOutput, ShellNodeConfigInput>();
        container.Connectable<ShellNodeConfigOutput, ShellNodeConfig>();
        container.Connectable<ShellNodeConfigOutput, ShellNodeConfigSection>();
        container.Connectable<ShellNodeConfig, ShellNodeConfigInput>();
        container.Connectable<ShellNodeConfig, ShellNodeConfigSection>();
        container.Connectable<IShellNodeConfigItem, ShellTemplateConfigNode>();

        container.Connectable<ShellNodeConfigSection, ShellNodeConfigInput>();
       
        container.Connectable<ShellNodeConfigSection, ShellNodeConfigSection>();
    }

    [InspectorProperty]
    public static bool GenerateDocumentation
    {
        get { return InvertGraphEditor.Prefs.GetBool("PLUGINDESIGNER_GENDOCS", true); }
        set
        {
            InvertGraphEditor.Prefs.SetBool("PLUGINDESIGNER_GENDOCS", value);
        }
    }
     


}