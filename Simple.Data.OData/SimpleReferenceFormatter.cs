﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;
using Simple.NExtLib;
using Simple.OData.Client;

namespace Simple.Data.OData
{
    public class SimpleReferenceFormatter
    {
        private readonly FunctionNameConverter _functionNameConverter = new FunctionNameConverter();
        private readonly Func<string, Table> _findTable;

        public SimpleReferenceFormatter(Func<string, Table> findTable)
        {
            _findTable = findTable;
        }

        public string FormatColumnClause(SimpleReference reference)
        {
            var formatted = TryFormatAsObjectReference(reference as ObjectReference)
                            ??
                            TryFormatAsFunctionReference(reference as FunctionReference)
                            ??
                            TryFormatAsMathReference(reference as MathReference);

            if (formatted != null) return formatted;

            throw new InvalidOperationException("SimpleReference type not supported.");
        }

        private string FormatObject(object value)
        {
            var reference = value as SimpleReference;
            if (reference != null)
                return FormatColumnClause(reference);
            return value is string ? string.Format("'{0}'", value) : value is DateTime ? ((DateTime)value).ToIso8601String() : value.ToString();
        }

        private string TryFormatAsMathReference(MathReference mathReference)
        {
            if (ReferenceEquals(mathReference, null)) return null;

            return string.Format("{0} {1} {2}", FormatObject(mathReference.LeftOperand),
                                 MathOperatorToString(mathReference.Operator), FormatObject(mathReference.RightOperand));
        }

        private static string MathOperatorToString(MathOperator @operator)
        {
            switch (@operator)
            {
                case MathOperator.Add:
                    return "add";
                case MathOperator.Subtract:
                    return "sub";
                case MathOperator.Multiply:
                    return "mul";
                case MathOperator.Divide:
                    return "div";
                case MathOperator.Modulo:
                    return "mod";
                default:
                    throw new InvalidOperationException("Invalid MathOperator specified.");
            }
        }

        private string TryFormatAsFunctionReference(FunctionReference functionReference)
        {
            if (ReferenceEquals(functionReference, null)) return null;

            var odataName = _functionNameConverter.ConvertToODataName(functionReference.Name);
            var columnArgument = FormatColumnClause(functionReference.Argument);

            if (functionReference.AdditionalArguments.Any())
            {
                var additionalArguments = FormatAdditionalArguments(functionReference.AdditionalArguments);
                if (odataName == "substringof")
                    return string.Format("{0}({1},{2})", odataName, additionalArguments, columnArgument);
                else
                    return string.Format("{0}({1},{2})", odataName, columnArgument, additionalArguments);
            }
            else
            {
                return string.Format("{0}({1})", odataName, columnArgument);
            }
        }

        private string FormatAdditionalArguments(IEnumerable<object> additionalArguments)
        {
            return string.Join(",", additionalArguments.Select(new ValueFormatter().FormatQueryStringValue));
        }

        private string TryFormatAsObjectReference(ObjectReference objectReference)
        {
            if (ReferenceEquals(objectReference, null)) return null;

            if (_findTable == null)
            {
                return objectReference.GetName();
            }
            else
            {
                string associationPath;
                Table table = GetOwnerTable(objectReference, out associationPath);
                return FormatObjectPath(associationPath, table.FindColumn(objectReference.GetName()).ActualName);
            }
        }

        private Table GetOwnerTable(ObjectReference objectReference, out string associationPath)
        {
            associationPath = string.Empty;

            var owner = objectReference.GetOwner();
            if (ReferenceEquals(owner, null))
                return null;

            if (ReferenceEquals(owner.GetOwner(), null))
                return _findTable(owner.GetName());

            var table = GetOwnerTable(owner, out associationPath);
            var association = table.FindAssociation(owner.GetName());
            associationPath = FormatObjectPath(associationPath, association.ActualName);
            return _findTable(association.ReferenceTableName);
        }

        private string FormatObjectPath(string pathPrefix, string objectName)
        {
            if (string.IsNullOrEmpty(pathPrefix))
                return objectName;
            else
                return string.Join("/", pathPrefix, objectName);
        }
    }
}
