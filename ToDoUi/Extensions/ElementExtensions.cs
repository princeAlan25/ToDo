using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoUi.Extensions;

public partial class ElementExtensions
{
    public static readonly BindableProperty ParentIdentityProperty =
        BindableProperty.CreateAttached("ParentIdentity", typeof(int), typeof(ElementExtensions), 0);
    public static readonly BindableProperty ChildIdentityProperty =
    BindableProperty.CreateAttached("ChildIdentity", typeof(int), typeof(ElementExtensions), 0);

    public static int GetParentIdentity(BindableObject view)
    {
        return (int)view.GetValue(ParentIdentityProperty);
    }

    public static void SetParentIdentity(BindableObject view, int value)
    {
        view.SetValue(ParentIdentityProperty, value);
    }

    public static int GetChildIdentity(BindableObject view)
    {
        return (int)view.GetValue(ChildIdentityProperty);
    }

    public static void SetChildIdentity(BindableObject view, int value)
    {
        view.SetValue(ChildIdentityProperty, value);
    }
}
