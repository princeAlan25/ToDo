using MauiIcons.Material;
using MauiIcons.Material.Outlined;
using System;
using System.Collections.Generic;
using System.Text;
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