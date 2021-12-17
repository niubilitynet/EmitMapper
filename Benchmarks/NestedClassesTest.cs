﻿using System;
using System.Diagnostics;
using AutoMapper;
using EmitMapper;

namespace Benchmarks;

public class NestedClassesTest
{
    private static ObjectsMapper<BenchSource, BenchDestination> emitMapper;
    private static IMapper autoMapper;

    private static long BenchHandwrittenMapper(int mappingsCount)
    {
        var s = new BenchSource();
        var d = new BenchDestination();
        var sw = new Stopwatch();
        sw.Start();
        for (var i = 0; i < mappingsCount; ++i) d = Map(s, d);
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private static long BenchEmitMapper(int mappingsCount)
    {
        var s = new BenchSource();
        var d = new BenchDestination();

        var sw = new Stopwatch();
        sw.Start();
        for (var i = 0; i < mappingsCount; ++i) emitMapper.Map(s, d);
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    private static long BenchAutoMapper(int mappingsCount)
    {
        var s = new BenchSource();
        var d = new BenchDestination();

        var sw = new Stopwatch();
        sw.Start();
        for (var i = 0; i < mappingsCount; ++i) autoMapper.Map(s, d);
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }

    public static void Initialize()
    {
        emitMapper = ObjectMapperManager.DefaultInstance.GetMapper<BenchSource, BenchDestination>();
        var config = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<BenchSource.Int1, BenchDestination.Int1>();
                cfg.CreateMap<BenchSource.Int2, BenchDestination.Int2>();
                cfg.CreateMap<BenchSource, BenchDestination>();
            });
        autoMapper = config.CreateMapper();
    }

    public static void Run()
    {
        var mappingsCount = 100000;
        Console.WriteLine("Auto Mapper (Nested): {0} milliseconds", BenchAutoMapper(mappingsCount));
        Console.WriteLine("Emit Mapper (Nested): {0} milliseconds", BenchEmitMapper(mappingsCount));
        Console.WriteLine("Handwritten Mapper (Nested): {0} milliseconds", BenchHandwrittenMapper(mappingsCount));
    }

    public class BenchSource
    {
        public byte n4;

        public int n2;
        public int n7;
        public int n8;
        public int n9;

        public Int2 i1 = new();
        public Int2 i2 = new();
        public Int2 i3 = new();
        public Int2 i4 = new();
        public Int2 i5 = new();
        public Int2 i6 = new();
        public Int2 i7 = new();
        public Int2 i8 = new();
        public long n3;
        public short n5;

        public string s1 = "1";
        public string s2 = "2";
        public string s3 = "3";
        public string s4 = "4";
        public string s5 = "5";
        public string s6 = "6";
        public string s7 = "7";
        public uint n6;

        public class Int1
        {
            public int i = 10;
            public string str1 = "1";
            public string str2 = null;
        }

        public class Int2
        {
            public Int1 i1 = new();
            public Int1 i2 = new();
            public Int1 i3 = new();
            public Int1 i4 = new();
            public Int1 i5 = new();
            public Int1 i6 = new();
            public Int1 i7 = new();
        }
    }

    public class BenchDestination
    {
        public long n2 = 2;
        public long n3 = 3;
        public long n4 = 4;
        public long n5 = 5;
        public long n6 = 6;
        public long n7 = 7;
        public long n8 = 8;
        public long n9 = 9;

        public string s1;
        public string s2;
        public string s3;
        public string s4;
        public string s5;
        public string s6;
        public string s7;

        public Int2 i1 { get; set; }
        public Int2 i2 { get; set; }
        public Int2 i3 { get; set; }
        public Int2 i4 { get; set; }
        public Int2 i5 { get; set; }
        public Int2 i6 { get; set; }
        public Int2 i7 { get; set; }
        public Int2 i8 { get; set; }

        public class Int1
        {
            public int i;
            public string str1;
            public string str2;
        }

        public class Int2
        {
            public Int1 i1;
            public Int1 i2;
            public Int1 i3;
            public Int1 i4;
            public Int1 i5;
            public Int1 i6;
            public Int1 i7;
        }
    }


    #region Hndwritten mapper

    private static BenchDestination.Int1 Map(BenchSource.Int1 s, BenchDestination.Int1 d)
    {
        if (s == null) return null;
        if (d == null) d = new BenchDestination.Int1();
        d.i = s.i;
        d.str1 = s.str1;
        d.str2 = s.str2;
        return d;
    }

    private static BenchDestination.Int2 Map(BenchSource.Int2 s, BenchDestination.Int2 d)
    {
        if (s == null) return null;

        if (d == null) d = new BenchDestination.Int2();
        d.i1 = Map(s.i1, d.i1);
        d.i2 = Map(s.i2, d.i2);
        d.i3 = Map(s.i3, d.i3);
        d.i4 = Map(s.i4, d.i4);
        d.i5 = Map(s.i5, d.i5);
        d.i6 = Map(s.i6, d.i6);
        d.i7 = Map(s.i7, d.i7);

        return d;
    }

    private static BenchDestination Map(BenchSource s, BenchDestination d)
    {
        if (s == null) return null;
        if (d == null) d = new BenchDestination();
        d.i1 = Map(s.i1, d.i1);
        d.i2 = Map(s.i2, d.i2);
        d.i3 = Map(s.i3, d.i3);
        d.i4 = Map(s.i4, d.i4);
        d.i5 = Map(s.i5, d.i5);
        d.i6 = Map(s.i6, d.i6);
        d.i7 = Map(s.i7, d.i7);
        d.i8 = Map(s.i8, d.i8);

        d.n2 = s.n2;
        d.n3 = s.n3;
        d.n4 = s.n4;
        d.n5 = s.n5;
        d.n6 = s.n6;
        d.n7 = s.n7;
        d.n8 = s.n8;
        d.n9 = s.n9;

        d.s1 = s.s1;
        d.s2 = s.s2;
        d.s3 = s.s3;
        d.s4 = s.s4;
        d.s5 = s.s5;
        d.s6 = s.s6;
        d.s7 = s.s7;

        return d;
    }

    #endregion
}

/*
Auto Mapper (simple): 52238 milliseconds
Emit Mapper (simple): 102 milliseconds
Handwritten Mapper (simple): 97 milliseconds
*/