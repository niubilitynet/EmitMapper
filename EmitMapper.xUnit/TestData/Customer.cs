﻿namespace EmitMapper.Tests.TestData;

using System.Collections.Generic;

public class Customer : ITestObject

{
  public Address Address { get; set; }

  public Address[] Addresses { get; set; }

  public decimal? Credit { get; set; }

  public Address HomeAddress { get; set; }

  public int Id { get; set; }

  public string Name { get; set; }

  public List<Address> WorkAddresses { get; set; }
}