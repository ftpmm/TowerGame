using System;

namespace lzengine
{
    public class EventData<T> : IEventData
    {
        public T Value { get; private set; }
        
        public EventData(T value)
        {
            Value = value;
        }
    }

    public class EventData<T1, T2> : IEventData
    {
        public T1 Value1 { get; private set; }
        public T2 Value2 { get; private set; }
        
        public EventData(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }

    public class EventData<T1, T2, T3> : IEventData
    {
        public T1 Value1 { get; private set; }
        public T2 Value2 { get; private set; }
        public T3 Value3 { get; private set; }
        
        public EventData(T1 value1, T2 value2, T3 value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }
    }

    public class EventData<T1, T2, T3, T4> : IEventData
    {
        public T1 Value1 { get; private set; }
        public T2 Value2 { get; private set; }
        public T3 Value3 { get; private set; }
        public T4 Value4 { get; private set; }
        
        public EventData(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }
    }
}
