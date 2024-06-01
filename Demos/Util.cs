using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
namespace Zinc.Sandbox.Demos;


[AttributeUsage(AttributeTargets.Class)]
public class DemoSceneAttribute : Attribute
{
    public string Name { get; private set; }
    public DemoSceneAttribute(string name)
    {
        Name = name;
    }
}

public record DemoSceneInfo(Type Type, string Name);
public static class Util
{
    public static IEnumerable<DemoSceneInfo> GetDemoSceneTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.Namespace == "Zinc.Sandbox.Demos" || type.Namespace == "Zinc.Sandbox.Demos.dungeon" )
            .Select(type => new
            {
                Type = type,
                Attribute = type.GetCustomAttribute<DemoSceneAttribute>()
            })
            .Where(t => t.Attribute != null)
            .Select(t => new DemoSceneInfo(t.Type, t.Attribute.Name))
            .OrderBy((info => info.Name));
    }
    
    public static IEnumerable<DemoSceneInfo> GetGenuarySceneTypes()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.Namespace == "Zinc.Sandbox.Demos.Genuary24")
            .Select(type => new
            {
                Type = type,
                Attribute = type.GetCustomAttribute<DemoSceneAttribute>()
            })
            .Where(t => t.Attribute != null)
            .Select(t => new DemoSceneInfo(t.Type, t.Attribute.Name))
            .OrderBy((info => info.Name));
    }

    public static object CreateInstance(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
        {
            return Activator.CreateInstance(type);
        }
        else
        {
            throw new InvalidOperationException("No default constructor found for type " + type.FullName);
        }
    }
}