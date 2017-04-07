﻿using System;
using System.Text;

namespace AmpsBoxSdk.Commands
{
    internal struct RawResult
    {
    //    public static readonly RawResult EmptyArray = new RawResult(new RawResult[0]);
    //    public static readonly RawResult Nil = new RawResult();
    //    private static readonly byte[] emptyBlob = new byte[0];
    //    private readonly int offset, count;
    //    private Array arr;
    //    public RawResult(ResultType resultType, byte[] buffer, int offset, int count)
    //    {
    //        switch (resultType)
    //        {
    //            case ResultType.SimpleString:
    //            case ResultType.Error:
    //            case ResultType.Integer:
    //            case ResultType.BulkString:
    //                break;
    //            default:
    //                throw new ArgumentOutOfRangeException(nameof(resultType));
    //        }
    //        Type = resultType;
    //        arr = buffer;
    //        this.offset = offset;
    //        this.count = count;
    //    }

    //    public bool HasValue => Type != ResultType.None;

    //    public bool IsError => Type == ResultType.Error;

    //    public ResultType Type { get; }

    //    internal bool IsNull => arr == null;

    //    public override string ToString()
    //    {
    //        if (arr == null)
    //        {
    //            return "(null)";
    //        }
    //        switch (Type)
    //        {
    //            case ResultType.SimpleString:
    //            case ResultType.Integer:
    //            case ResultType.Error:
    //                return $"{Type}: {GetString()}";
    //            default:
    //                return "(unknown)";
    //        }
    //    }


    //    internal unsafe bool IsEqual(byte[] expected)
    //    {
    //        if (expected == null) throw new ArgumentNullException(nameof(expected));
    //        if (expected.Length != count) return false;
    //        var actual = arr as byte[];
    //        if (actual == null) return false;

    //        int octets = count / 8, spare = count % 8;
    //        fixed (byte* actual8 = &actual[offset])
    //        fixed (byte* expected8 = expected)
    //        {
    //            long* actual64 = (long*)actual8;
    //            long* expected64 = (long*)expected8;

    //            for (int i = 0; i < octets; i++)
    //            {
    //                if (actual64[i] != expected64[i]) return false;
    //            }
    //            int index = count - spare;
    //            while (spare-- != 0)
    //            {
    //                if (actual8[index] != expected8[index]) return false;
    //            }
    //        }
    //        return true;
    //    }

    //    internal bool AssertStarts(byte[] expected)
    //    {
    //        if (expected == null) throw new ArgumentNullException(nameof(expected));
    //        if (expected.Length > count) return false;
    //        var actual = arr as byte[];
    //        if (actual == null) return false;

    //        for (int i = 0; i < expected.Length; i++)
    //        {
    //            if (expected[i] != actual[offset + i]) return false;
    //        }
    //        return true;
    //    }
    //    internal byte[] GetBlob()
    //    {
    //        var src = (byte[])arr;
    //        if (src == null) return null;

    //        if (count == 0) return emptyBlob;

    //        byte[] copy = new byte[count];
    //        Buffer.BlockCopy(src, offset, copy, 0, count);
    //        return copy;
    //    }

    //    internal bool GetBoolean()
    //    {
    //        if (count != 1) throw new InvalidCastException();
    //        byte[] actual = arr as byte[];
    //        if (actual == null) throw new InvalidCastException();
    //        switch (actual[offset])
    //        {
    //            case (byte)'1': return true;
    //            case (byte)'0': return false;
    //            default: throw new InvalidCastException();
    //        }
    //    }

    //    internal RawResult[] GetItems()
    //    {
    //        return (RawResult[])arr;
    //    }


    //    static readonly string[] NilStrings = new string[0];
    //    internal string[] GetItemsAsStrings()
    //    {
    //        RawResult[] items = GetItems();
    //        if (items == null)
    //        {
    //            return null;
    //        }
    //        else if (items.Length == 0)
    //        {
    //            return NilStrings;
    //        }
    //        else
    //        {
    //            var arr = new string[items.Length];
    //            for (int i = 0; i < arr.Length; i++)
    //            {
    //                arr[i] = (string)(items[i].AsRedisValue());
    //            }
    //            return arr;
    //        }
    //    }

    //    internal RawResult[] GetItemsAsRawResults()
    //    {
    //        return GetItems();
    //    }


    //    // returns an array of RawResults
    //    internal RawResult[] GetArrayOfRawResults()
    //    {
    //        if (arr == null)
    //        {
    //            return null;
    //        }
    //        else if (arr.Length == 0)
    //        {
    //            return new RawResult[0];
    //        }
    //        else
    //        {
    //            var rawResultArray = new RawResult[arr.Length];
    //            for (int i = 0; i < arr.Length; i++)
    //            {
    //                var rawResult = (RawResult)arr.GetValue(i);
    //                rawResultArray.SetValue(rawResult, i);
    //            }
    //            return rawResultArray;
    //        }
    //    }

    //    internal string GetString()
    //    {
    //        if (arr == null) return null;
    //        var blob = (byte[])arr;
    //        if (blob.Length == 0) return "";
    //        return Encoding.UTF8.GetString(blob, offset, count);
    //    }

    //    internal bool TryGetDouble(out double val)
    //    {
    //        if (arr == null)
    //        {
    //            val = 0;
    //            return false;
    //        }
    //        long i64;
    //        if (TryGetInt64(out i64))
    //        {
    //            val = i64;
    //            return true;
    //        }
    //        return Format.TryParseDouble(GetString(), out val);
    //    }

    //    internal bool TryGetInt64(out long value)
    //    {
    //        if (arr == null)
    //        {
    //            value = 0;
    //            return false;
    //        }
    //        return RedisValue.TryParseInt64(arr as byte[], offset, count, out value);
    //    }
    }
}