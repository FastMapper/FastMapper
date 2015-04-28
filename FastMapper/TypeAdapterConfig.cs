using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FastMapper.Models;
using FastMapper.Utils;

namespace FastMapper
{
    internal class BaseTypeAdapterConfig
    {
        public BaseTypeAdapterConfig()
        {
            NewInstanceForSameType = true;
        }

        /// <summary>
        /// This property only use TypeAdapter.Adapt() method. Project().To() not use this property. Default: true
        /// </summary>
        internal bool NewInstanceForSameType { get; set; }
    }

    public class TypeAdapterConfig
    {
        #region Constructor

        private TypeAdapterConfig()
        {

        }

        #endregion

        #region Members

        internal static readonly BaseTypeAdapterConfig Configuration = new BaseTypeAdapterConfig();

        #endregion

        #region Public Static Methods

        public static TypeAdapterConfig NewConfig()
        {
            Configuration.NewInstanceForSameType = true;

            return new TypeAdapterConfig();
        }

        #endregion

        #region Public Methods

        public TypeAdapterConfig NewInstanceForSameType(bool newInstanceForSameType)
        {
            Configuration.NewInstanceForSameType = newInstanceForSameType;

            return this;
        }

        #endregion
    }

    internal class BaseTypeAdapterConfig<TSource>
    {
        #region Members

        internal readonly List<string> IgnoreMembers = new List<string>();

        internal readonly List<InvokerModel<TSource>> Resolvers = new List<InvokerModel<TSource>>();

        internal int MaxDepth { get; set; }

        /// <summary>
        /// This property only use TypeAdapter.Adapt() method. Project().To() not use this property. Default: true
        /// </summary>
        internal bool? NewInstanceForSameType { get; set; }

        /// <summary>
        /// This property only use TypeAdapter.Adapt() method. Project().To() not use this property. Default: false
        /// </summary>
        internal bool? IgnoreNullValues { get; set; }

        #endregion
    }

    public class TypeAdapterConfig<TSource, TDestination>
    {
        #region Members

        internal static readonly BaseTypeAdapterConfig<TSource> Configuration = new BaseTypeAdapterConfig<TSource>();

        private readonly ProjectionConfig<TSource, TDestination> _Projection = ProjectionConfig<TSource, TDestination>.NewConfig();

        #endregion

        #region Constructor

        private TypeAdapterConfig()
        {

        }

        #endregion

        #region Public Static Methods

        public static TypeAdapterConfig<TSource, TDestination> NewConfig()
        {
            Configuration.IgnoreMembers.Clear();
            Configuration.Resolvers.Clear();

            return new TypeAdapterConfig<TSource, TDestination>();
        }

        #endregion

        #region Public Methods

        public TypeAdapterConfig<TSource, TDestination> IgnoreMember(params Expression<Func<TDestination, object>>[] members)
        {
            _Projection.IgnoreMember(members);

            if (members != null && members.Length > 0)
            {
                var config = Configuration;
                for (int i = 0; i < members.Length; i++)
                {
                    var memberExp = ReflectionUtils.GetMemberInfo(members[i]);
                    if (memberExp != null)
                    {
                        config.IgnoreMembers.Add(memberExp.Member.Name);
                    }
                }
            }

            return this;
        }

        public TypeAdapterConfig<TSource, TDestination> IgnoreMember(params string[] members)
        {
            _Projection.IgnoreMember(members);

            if (members != null && members.Length > 0)
            {
                members = typeof(TDestination).GetProperties().Where(p => members.Contains(p.Name)).Select(p => p.Name).ToArray();

                if (members != null && members.Length > 0)
                {
                    var config = Configuration;
                    for (int i = 0; i < members.Length; i++)
                    {
                        config.IgnoreMembers.Add(members[i]);
                    }
                }
            }

            return this;
        }

        public TypeAdapterConfig<TSource, TDestination> MapFrom<TKey>(Expression<Func<TDestination, TKey>> member, Expression<Func<TSource, TKey>> sourceExpression)
        {
            _Projection.MapFrom<TKey>(member, sourceExpression);

            if (sourceExpression == null)
                return this;

            var func = sourceExpression.Compile();

            if (func != null)
            {
                var config = Configuration;
                Func<TSource, object> resolver = (source) => func(source);

                var memberExp = member.Body as MemberExpression;
                if (memberExp != null)
                    config.Resolvers.Add(new InvokerModel<TSource>() { MemberName = memberExp.Member.Name, Invoker = resolver });
            }

            return this;
        }

        public TypeAdapterConfig<TSource, TDestination> NewInstanceForSameType(bool newInstanceForSameType)
        {
            Configuration.NewInstanceForSameType = newInstanceForSameType;

            return this;
        }

        public TypeAdapterConfig<TSource, TDestination> IgnoreNullValues(bool ignoreNullValues)
        {
            Configuration.IgnoreNullValues = ignoreNullValues;

            return this;
        }

        public TypeAdapterConfig<TSource, TDestination> MaxDepth(int maxDepth)
        {
            Configuration.MaxDepth = maxDepth;

            _Projection.MaxDepth(maxDepth);

            return this;
        }

        #endregion
    }
}
