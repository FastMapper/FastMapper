using System;
using System.Collections.Generic;

using FastMapper.Utils;

namespace FastMapper.Adapters
{
    public class PrimitiveAdapter<TSource, TDestination>
    {
        #region Members

        public static readonly FastInvokeHandler _converter = CreateConverter();

        #endregion

        #region Public Static Methods

        public static TDestination Adapt(TSource source, TDestination destination)
        {
            if (source == null)
                return default(TDestination);

            if (_converter == null)
                return (TDestination)Convert.ChangeType(source, typeof(TDestination));

            return (TDestination)_converter(null, new object[] { source });
        }

        public static TDestination Adapt(TSource source)
        {
            return Adapt(source, default(TDestination));
        }

        #endregion

        #region Private Methods

        private static FastInvokeHandler CreateConverter()
        {
            return ReflectionUtils.CreatePrimitiveConverter(typeof(TSource), typeof(TDestination));
        }

        #endregion
    }
}
