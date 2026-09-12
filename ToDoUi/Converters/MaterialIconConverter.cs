using MauiIcons.Material;
using MauiIcons.Material.Outlined;
using ToDoUi.Factories;

namespace ToDoUi.Converters;

public class MaterialIconConverter : BaseIconConverter<MaterialIcons>
{
    protected override Enum GetFallbackIcon() => MaterialIcons.Help;
}

public class MaterialOutlinedIconConverter : BaseIconConverter<MaterialOutlinedIcons>
{
    protected override Enum GetFallbackIcon() => MaterialOutlinedIcons.Help;
}