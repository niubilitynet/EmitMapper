﻿using System;
using System.Collections.Generic;
using System.Linq;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations.Interfaces;
using Shouldly;
using Xunit;

namespace EmitMapper.Tests;

////[TestFixture]
public class IgnoreByAttributes
{
  [Fact]
  public void Test()
  {
    var mapper =
      Mapper.Default.GetMapper<IgnoreByAttributesSrc, IgnoreByAttributesDst>(new MyConfigurator());

    var dst = mapper.Map(new IgnoreByAttributesSrc());
    dst.Str1.ShouldBe("IgnoreByAttributesDst::str1");
    dst.Str2.ShouldBe("IgnoreByAttributesSrc::str2");
  }

  public class IgnoreByAttributesDst
  {
    public string Str1 = "IgnoreByAttributesDst::str1";
    public string Str2 = "IgnoreByAttributesDst::str2";
  }

  public class IgnoreByAttributesSrc
  {
    [MyIgnore] public string Str1 = "IgnoreByAttributesSrc::str1";
    public string Str2 = "IgnoreByAttributesSrc::str2";
  }

  public class MyConfigurator : DefaultMapConfig
  {
    public override IEnumerable<IMappingOperation> GetMappingOperations(Type from, Type to)
    {
      IgnoreMembers<object, object>(GetIgnoreFields(from).Concat(GetIgnoreFields(to)).ToArray());

      return base.GetMappingOperations(from, to);
    }

    private IEnumerable<string> GetIgnoreFields(Type type)
    {
      return type.GetFields().Where(f => f.GetCustomAttributes(typeof(MyIgnoreAttribute), false).Any())
        .Select(f => f.Name).Concat(
          type.GetProperties().Where(p => p.GetCustomAttributes(typeof(MyIgnoreAttribute), false).Any())
            .Select(p => p.Name));
    }
  }

  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
  public class MyIgnoreAttribute : Attribute
  {
  }
}