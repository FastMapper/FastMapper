﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FastMapper.Utils
{
    public class ParameterRenamer : ExpressionVisitor
    {
        private ParameterExpression _parameterExpression;
        private string _expName;

        public Expression Rename(Expression expression, ParameterExpression parameterExpression)
        {
            _parameterExpression = parameterExpression;
            return Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_expName == null)
                _expName = node.Name;

            return node.Name == _expName && node.Type == _parameterExpression.Type ? _parameterExpression : node;
        }
    }
}
