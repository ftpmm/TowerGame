﻿// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

namespace lzengine
{

using global::System;
using global::System.Collections.Generic;
using global::Google.FlatBuffers;

public struct HeroTable : ITableObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public ITableObject InitTable(ByteBuffer _bb) { return HeroTable.GetRoot( _bb, this); }
  public static HeroTable GetRoot(ByteBuffer _bb) { return GetRoot(_bb, new HeroTable()); }
  public static HeroTable GetRoot(ByteBuffer _bb, HeroTable obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool Verify(ByteBuffer _bb) {Google.FlatBuffers.Verifier verifier = new Google.FlatBuffers.Verifier(_bb); return verifier.VerifyBuffer("", false, HeroTableVerify.Verify); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public HeroTable __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public lzengine.HeroItem? Datas(int j) { int o = __p.__offset(4); return o != 0 ? (lzengine.HeroItem?)(new lzengine.HeroItem()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int DatasLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
  public lzengine.HeroItem? DatasByKey(int key) { int o = __p.__offset(4); return o != 0 ? lzengine.HeroItem.__lookup_by_key(__p.__vector(o), key, __p.bb) : null; }

  public static Offset<lzengine.HeroTable> Create(FlatBufferBuilder builder,
      VectorOffset datasOffset = default(VectorOffset)) {
    builder.StartTable(1);
    HeroTable.AddDatas(builder, datasOffset);
    return HeroTable.End(builder);
  }

  public static void Start(FlatBufferBuilder builder) { builder.StartTable(1); }
  public static void AddDatas(FlatBufferBuilder builder, VectorOffset datasOffset) { builder.AddOffset(0, datasOffset.Value, 0); }
  public static VectorOffset CreateDatasVector(FlatBufferBuilder builder, Offset<lzengine.HeroItem>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateDatasVectorBlock(FlatBufferBuilder builder, Offset<lzengine.HeroItem>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateDatasVectorBlock(FlatBufferBuilder builder, ArraySegment<Offset<lzengine.HeroItem>> data) { builder.StartVector(4, data.Count, 4); builder.Add(data); return builder.EndVector(); }
  public static VectorOffset CreateDatasVectorBlock(FlatBufferBuilder builder, IntPtr dataPtr, int sizeInBytes) { builder.StartVector(1, sizeInBytes, 1); builder.Add<Offset<lzengine.HeroItem>>(dataPtr, sizeInBytes); return builder.EndVector(); }
  public static void StartDatasVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<lzengine.HeroTable> End(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<lzengine.HeroTable>(o);
  }
  public static void Finish(FlatBufferBuilder builder, Offset<lzengine.HeroTable> offset) { builder.Finish(offset.Value); }
  public static void FinishSizePrefixed(FlatBufferBuilder builder, Offset<lzengine.HeroTable> offset) { builder.FinishSizePrefixed(offset.Value); }
}


static public class HeroTableVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyVectorOfTables(tablePos, 4 /*Datas*/, lzengine.HeroItemVerify.Verify, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}
public struct HeroItem : ITableObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static void ValidateVersion() { FlatBufferConstants.FLATBUFFERS_23_5_26(); }
  public ITableObject InitTable(ByteBuffer _bb) { return HeroItem.GetRoot( _bb, this); }
  public static HeroItem GetRoot(ByteBuffer _bb) { return GetRoot(_bb, new HeroItem()); }
  public static HeroItem GetRoot(ByteBuffer _bb, HeroItem obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p = new Table(_i, _bb); }
  public HeroItem __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public int _Id { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public float _Baseatk { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float _Sp { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float _Hp { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float _Attackdistance { get { int o = __p.__offset(12); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public float _Attackinterval { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<lzengine.HeroItem> Create(FlatBufferBuilder builder,
      int _id = 0,
      float _baseatk = 0.0f,
      float _sp = 0.0f,
      float _hp = 0.0f,
      float _attackdistance = 0.0f,
      float _attackinterval = 0.0f) {
    builder.StartTable(6);
    HeroItem.Add_Attackinterval(builder, _attackinterval);
    HeroItem.Add_Attackdistance(builder, _attackdistance);
    HeroItem.Add_Hp(builder, _hp);
    HeroItem.Add_Sp(builder, _sp);
    HeroItem.Add_Baseatk(builder, _baseatk);
    HeroItem.Add_Id(builder, _id);
    return HeroItem.End(builder);
  }

  public static void Start(FlatBufferBuilder builder) { builder.StartTable(6); }
  public static void Add_Id(FlatBufferBuilder builder, int _Id) { builder.AddInt(0, _Id, 0); }
  public static void Add_Baseatk(FlatBufferBuilder builder, float _Baseatk) { builder.AddFloat(1, _Baseatk, 0.0f); }
  public static void Add_Sp(FlatBufferBuilder builder, float _Sp) { builder.AddFloat(2, _Sp, 0.0f); }
  public static void Add_Hp(FlatBufferBuilder builder, float _Hp) { builder.AddFloat(3, _Hp, 0.0f); }
  public static void Add_Attackdistance(FlatBufferBuilder builder, float _Attackdistance) { builder.AddFloat(4, _Attackdistance, 0.0f); }
  public static void Add_Attackinterval(FlatBufferBuilder builder, float _Attackinterval) { builder.AddFloat(5, _Attackinterval, 0.0f); }
  public static Offset<lzengine.HeroItem> End(FlatBufferBuilder builder) {
    int o = builder.EndTable();
    return new Offset<lzengine.HeroItem>(o);
  }

  public static VectorOffset CreateSortedVectorOfHeroItem(FlatBufferBuilder builder, Offset<HeroItem>[] offsets) {
    Array.Sort(offsets,
      (Offset<HeroItem> o1, Offset<HeroItem> o2) =>
        new HeroItem().__assign(builder.DataBuffer.Length - o1.Value, builder.DataBuffer)._Id.CompareTo(new HeroItem().__assign(builder.DataBuffer.Length - o2.Value, builder.DataBuffer)._Id));
    return builder.CreateVectorOfTables(offsets);
  }

  public static HeroItem? __lookup_by_key(int vectorLocation, int key, ByteBuffer bb) {
    HeroItem obj_ = new HeroItem();
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


static public class HeroItemVerify
{
  static public bool Verify(Google.FlatBuffers.Verifier verifier, uint tablePos)
  {
    return verifier.VerifyTableStart(tablePos)
      && verifier.VerifyField(tablePos, 4 /*_Id*/, 4 /*int*/, 4, false)
      && verifier.VerifyField(tablePos, 6 /*_Baseatk*/, 4 /*float*/, 4, false)
      && verifier.VerifyField(tablePos, 8 /*_Sp*/, 4 /*float*/, 4, false)
      && verifier.VerifyField(tablePos, 10 /*_Hp*/, 4 /*float*/, 4, false)
      && verifier.VerifyField(tablePos, 12 /*_Attackdistance*/, 4 /*float*/, 4, false)
      && verifier.VerifyField(tablePos, 14 /*_Attackinterval*/, 4 /*float*/, 4, false)
      && verifier.VerifyTableEnd(tablePos);
  }
}

}
