﻿// See https://aka.ms/new-console-template for more information

namespace ZeUnit.Activators;

public static class ClassActivatorFactory
{
    public static IZeActivator Get(TypeInfo @class)
    {
        var activators = @class.GetCustomAttributes()
            .Where(n => n.GetType().IsAssignableTo(typeof(ZeActivatorAttribute)))
            .Select(n => (ZeActivatorAttribute)n)
            .Select(n => n.Activator)
            .Distinct();

        switch (activators.Count())
        {
            case 0:
                return new CoreTestActivator();
            case 1:
                return (IZeActivator)Activator.CreateInstance(activators.First());
            default:
                break;
        }

        throw new Exception("Could not create instance of class, defining mulitple activator types on a single class is not supported.");
    }
}
