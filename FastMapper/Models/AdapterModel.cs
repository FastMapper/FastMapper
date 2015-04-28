using System;
using FastMapper.Utils;

namespace FastMapper.Models
{
    /// <summary>
    /// Getter : FieldInfo.GetValue
    /// Setter : FieldInfo.SetValue
    /// </summary>
    public class FieldModel
    {
        public FastInvokeHandler Getter;
        public FastInvokeHandler Setter;
    }

    public class PropertyModel<TSource, TDestination>
    {
        public PropertyCaller<TSource>.GenGetter Getter;
        public PropertyCaller<TDestination>.GenSetter Setter;
        public object DefaultDestinationValue;
        public FastInvokeHandler AdaptInvoker;
        public GenericGetter[] FlatteningInvokers;
        public Func<TSource, object> CustomResolver;

        // Use byte, because byte performance better than enum
        public byte ConvertType; //Primitive = 1, FlatteningGetMethod = 2, FlatteningDeep = 3, Adapter = 4, CustomResolve = 5;
    }

    public class AdapterModel<TSource, TDestination>
    {
        public FieldModel[] Fields;
        public PropertyModel<TSource, TDestination>[] Properties;
    }

    public class CollectionAdapterModel
    {
        public bool IsPrimitive;
        public FastInvokeHandler AdaptInvoker;
    }

    public class InvokerModel<TSource>
    {
        public string MemberName;
        public Func<TSource, object> Invoker;
    }

    public class ExpressionModel
    {
        public string DestinationMemberName;
        public System.Linq.Expressions.Expression SourceExpression;
    }
}
