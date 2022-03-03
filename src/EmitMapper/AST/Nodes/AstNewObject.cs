﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EmitMapper.AST.Helpers;
using EmitMapper.AST.Interfaces;
using EmitMapper.Utils;

namespace EmitMapper.AST.Nodes;

internal class AstNewObject : IAstRef
{
  public IAstStackItem[] ConstructorParams;

  public Type ObjectType;

  public AstNewObject()
  {
  }

  public AstNewObject(Type objectType, IAstStackItem[] constructorParams)
  {
    ObjectType = objectType;
    ConstructorParams = constructorParams;
  }

  public Type ItemType => ObjectType;

  public void Compile(CompilationContext context)
  {
    if (ReflectionHelper.IsNullable(ObjectType))
    {
      IAstRefOrValue underlyingValue;
      var underlyingType = ObjectType.GetUnderlyingTypeCache();

      if (ConstructorParams == null || ConstructorParams.Length == 0)
      {
        var temp = context.ILGenerator.DeclareLocal(underlyingType);
        new AstInitializeLocalVariable(temp).Compile(context);
        underlyingValue = AstBuildHelper.ReadLocalRV(temp);
      }
      else
      {
        underlyingValue = (IAstValue)ConstructorParams[0];
      }

      var constructor = ObjectType.GetConstructor(
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance,
        null,
        new[] { underlyingType },
        null);

      underlyingValue.Compile(context);
      context.EmitNewObject(constructor);
    }
    else
    {
      IEnumerable<Type> types = Type.EmptyTypes;

      if (ConstructorParams != null && ConstructorParams.Length > 0)
      {
        types = ConstructorParams.Select(c => c.ItemType);

        foreach (var p in ConstructorParams)
          p.Compile(context);
      }

      var ci = ObjectType.GetConstructor(types.ToArray());

      if (ci != null)
      {
        context.EmitNewObject(ci);
      }
      else if (ObjectType.IsValueType)
      {
        var temp = context.ILGenerator.DeclareLocal(ObjectType);
        new AstInitializeLocalVariable(temp).Compile(context);
        AstBuildHelper.ReadLocalRV(temp).Compile(context);
      }
      else
      {
        throw new Exception($"Constructor for types [{types.ToCsv(",")}] not found in {ObjectType.FullName}");
      }
    }
  }
}