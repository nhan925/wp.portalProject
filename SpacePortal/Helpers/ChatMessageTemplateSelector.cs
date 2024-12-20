using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SpacePortal.Models;

namespace SpacePortal.Helpers;

public class ChatMessageTemplateSelector : DataTemplateSelector
{
    public DataTemplate UserMessageTemplate
    {
        get; set;
    }
    public DataTemplate BotMessageTemplate
    {
        get; set;
    }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is InformationsForAIChatbot message)
        {
            return message.IsUser ? UserMessageTemplate : BotMessageTemplate;
        }

        return base.SelectTemplateCore(item, container) ?? throw new InvalidOperationException("No template found for the given item.");
    }
}