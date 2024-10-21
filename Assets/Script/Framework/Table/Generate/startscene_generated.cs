﻿// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace lzengine
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct StartsceneTable : ITableObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public ITableObject InitTable(ByteBuffer _bb) { return StartsceneTable.GetRoot( _bb, this); }
  public static StartsceneTable GetRoot(ByteBuffer _bb) { return GetRoot(_bb, new StartsceneTable()); }
  public static StartsceneTable GetRoot(ByteBuffer _bb, StartsceneTable obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool Verify(ByteBuffer _bb) {Google.FlatBuffers.Verifier verifier = new Google.FlatBuffers.Verifier(_bb); return verifier.VerifyBuffer("", false, StartsceneTableVerify.Verify); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public StartsceneTable __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public lzengine.StartsceneItem? Datas(int j) { int o = __p.__offset(4); return o != 0 ? (lzengine.StartsceneItem?)(new lzengine.StartsceneItem()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int DatasLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
  public lzengine.StartsceneItem? DatasByKey(int key) { int o = __p.__offset(4); return o != 0 ? lzengine.StartsceneItem.__lookup_by_key(__p.__vector(o), key, __p.bb) : null; }

  public static Offset<lzengine.StartsceneTable> Create(FlatBufferBuilder builder,
      VectorOffset datasOffset = default(VectorOffset)) {
    builder.StartTable(1);
    StartsceneTable.AddDatas(builder, datasOffset);
    return StartsceneTable.End(builder);
  }

  public static void Start(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddDatas(FlatBufferBuilder builder, VectorOffset datasOffset) { builder.AddOffset(0, datasOffset.Value, 0); }
  public static VectorOffset CreateDatasVector(FlatBufferBuilder builder, Offset<lzengine.StartsceneItem>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateDatasVectorBlock(FlatBufferBuilder builder, Offset<lzengine.StartsceneItem>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateDatasVectorBlock(FlatBufferBuilder builder, ArraySegment<Offset<lzengine.StartsceneItem>> data) { builder.StartVector(4, data.Count, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateDatasVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<Offset<lzengine.StartsceneItem>>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartDatasVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<lzengine.StartsceneTable> End(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<lzengine.StartsceneTable>(o);
  }
  public static void Finish(FlatBufferBuilder builder, Offset<lzengine.StartsceneTable> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixed(FlatBufferBuilder builder, Offset<lzengine.StartsceneTable> offset) { builder.FinishSizePrefixed(offset.Value); }
}


static public class StartsceneTableVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyVectorOfTables(tablePos, 4 /*Datas*/, lzengine.StartsceneItemVerify.Verify, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}
public struct StartsceneItem : ITableObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public ITableObject InitTable(ByteBuffer _bb) { return StartsceneItem.GetRoot( _bb, this); }
  public static StartsceneItem GetRoot(ByteBuffer _bb) { return GetRoot(_bb, new StartsceneItem()); }
  public static StartsceneItem GetRoot(ByteBuffer _bb, StartsceneItem obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public StartsceneItem __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int _Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int _Process { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int _Zone { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public string _Scenetype { get { int o = __p.__offset(10); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> Get_ScenetypeBytes() { return __p.__vector_as_span<byte>(10, 1); }
#else
  public ArraySegment<byte>? Get_ScenetypeBytes() { return __p.__vector_as_arraysegment(10); }
#endif
  public byte[] Get_ScenetypeArray() { return __p.__vector_as_array<byte>(10); }
  public string _Name { get { int o = __p.__offset(12); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> Get_NameBytes() { return __p.__vector_as_span<byte>(12, 1); }
#else
  public ArraySegment<byte>? Get_NameBytes() { return __p.__vector_as_arraysegment(12); }
#endif
  public byte[] Get_NameArray() { return __p.__vector_as_array<byte>(12); }
  public int _Outerport { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<lzengine.StartsceneItem> Create(FlatBufferBuilder builder,
      int _id = 0,
      int _process = 0,
      int _zone = 0,
      StringOffset _scenetypeOffset = default(StringOffset),
      StringOffset _nameOffset = default(StringOffset),
      int _outerport = 0) {
    builder.StartTable(6);
    StartsceneItem.Add_Outerport(builder, _outerport);
    StartsceneItem.Add_Name(builder, _nameOffset);
    StartsceneItem.Add_Scenetype(builder, _scenetypeOffset);
    StartsceneItem.Add_Zone(builder, _zone);
    StartsceneItem.Add_Process(builder, _process);
    StartsceneItem.Add_Id(builder, _id);
    return StartsceneItem.End(builder);
  }

  public static void Start(FlatBufferBuilder builder) { builder.StartTable(6); }
  public static void Add_Id(FlatBufferBuilder builder, int _Id) { builder.AddInt(0, _Id, 0); }
  public static void Add_Process(FlatBufferBuilder builder, int _Process) { builder.AddInt(1, _Process, 0); }
  public static void Add_Zone(FlatBufferBuilder builder, int _Zone) { builder.AddInt(2, _Zone, 0); }
  public static void Add_Scenetype(FlatBufferBuilder builder, StringOffset _ScenetypeOffset) { builder.AddOffset(3, _ScenetypeOffset.Value, 0); }
  public static void Add_Name(FlatBufferBuilder builder, StringOffset _NameOffset) { builder.AddOffset(4, _NameOffset.Value, 0); }
  public static void Add_Outerport(FlatBufferBuilder builder, int _Outerport) { builder.AddInt(5, _Outerport, 0); }
  public static Offset<lzengine.StartsceneItem> End(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<lzengine.StartsceneItem>(o);
  }

  public static VectorOffset CreateSortedVectorOfStartsceneItem(FlatBufferBuilder builder, Offset<StartsceneItem>[] offsets) {
    Array.Sort(offsets,
      (Offset<StartsceneItem> o1, Offset<StartsceneItem> o2) =>
        new StartsceneItem().__assign(builder.DataBuffer.Length - o1.Value, builder.DataBuffer)._Id.CompareTo(new StartsceneItem().__assign(builder.DataBuffer.Length - o2.Value, builder.DataBuffer)._Id));
    return builder.CreateVectorOfTables(offsets);
  }

  public static StartsceneItem? __lookup_by_key(int vectorLocation, int key, ByteBuffer bb) {
    StartsceneItem obj_ = new StartsceneItem();
    int span = bb.GetInt(vectorLocation - 4);
    int start = 0;
    while (span != 0) {
      int middle = span / 2;
      int tableOffset = Table.__indirect(vectorLocation + 4 * (start + middle), bb);
      obj_.__assign(tableOffset, bb);
      int comp = obj_._Id.CompareTo(key);
      if (comp > 0) {
        span = middle;
      } else if (comp < 0) {
        middle++;
        start += middle;
        span -= middle;
      } else {
        return obj_;
      }
    }
    return null;
  }
}


static public class StartsceneItemVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyField(tablePos, 4 /*_Id*/, 4 /*int*/, 4, false)
      && verifier.VerifyField(tablePos, 6 /*_Process*/, 4 /*int*/, 4, false)
      && verifier.VerifyField(tablePos, 8 /*_Zone*/, 4 /*int*/, 4, false)
      && verifier.VerifyString(tablePos, 10 /*_Scenetype*/, false)
      && verifier.VerifyString(tablePos, 12 /*_Name*/, false)
      && verifier.VerifyField(tablePos, 14 /*_Outerport*/, 4 /*int*/, 4, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}