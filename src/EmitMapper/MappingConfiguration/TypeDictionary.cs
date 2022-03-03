﻿using System;
using System.Collections.Generic;
using System.Linq;
using EmitMapper.Utils;

namespace EmitMapper.MappingConfiguration;

internal class TypeDictionary<T>
  where T : class
{
  private readonly List<ListElement> _elements = new();

  public void Add(Type[] types, T value)
  {
    var newElem = new ListElement(types, value);

    if (_elements.Contains(newElem))
      _elements.Remove(newElem);

    _elements.Add(new ListElement(types, value));
  }

  public T GetValue(Type[] types)
  {
    var elem = FindTypes(types);

    return elem?.Value;
  }

  public T GetValue(Type type)
  {
    var elem = FindTypes(type);

    return elem?.Value;
  }

  public bool IsTypesInList(Type[] types)
  {
    return FindTypes(types).HasValue;
  }

  public override string ToString()
  {
    return _elements.Select(e => e.Types.ToCsv("|") + (e.Value == null ? "|" : "|" + e.Value)).ToCsv("||");
  }

  private static bool IsGeneralType(Type generalType, Type type)
  {
    if (generalType == type)
      return true;

    if (generalType.IsGenericTypeDefinition)
    {
      if (generalType.IsInterface)
        return type.GetInterfacesCache().Concat(type.IsInterface ? new[] { type } : Type.EmptyTypes)
          .Any(i => i.IsGenericType && i.GetGenericTypeDefinitionCache() == generalType);

      return type.IsGenericType && (type.GetGenericTypeDefinitionCache() == generalType
                                    || type.GetGenericTypeDefinitionCache().IsSubclassOf(generalType));
    }

    return generalType.IsAssignableFrom(type);
  }

  private ListElement? FindTypes(Type[] types)
  {
    foreach (var element in _elements)
    {
      var isAssignable = true;

      for (int i = 0, j = 0; i < element.Types.Length; ++i)
      {
        if (i < types.Length)
          j = i;

        if (IsGeneralType(element.Types[i], types[j]))
          continue;

        isAssignable = false;

        break;
      }

      if (isAssignable)
        return element;
    }

    return null;
  }

  private ListElement? FindTypes(Type type)
  {
    foreach (var element in _elements)
    {
      var isAssignable = true;

      foreach (var t in element.Types)
      {
        if (IsGeneralType(t, type))
          continue;

        isAssignable = false;

        break;
      }

      if (isAssignable)
        return element;
    }

    return null;
  }

  private readonly struct ListElement : IEquatable<ListElement>
  {
    public readonly Type[] Types;

    public readonly T Value;

    public ListElement(Type[] types, T value)
    {
      Types = types;
      Value = value;
    }

    public bool Equals(ListElement other)
    {
      return !Types.Where((t, i) => t != other.Types[i]).Any();
    }

    public override bool Equals(object obj)
    {
      return Equals(obj);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Types);
    }
  }
}