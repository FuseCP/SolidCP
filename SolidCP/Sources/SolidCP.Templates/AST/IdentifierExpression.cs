// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Templates.AST
{
    internal class IdentifierExpression : Expression
    {
        List<IdentifierPart> parts = new List<IdentifierPart>();

        public IdentifierExpression(int line, int column)
            : base(line, column)
        {
        }

        public List<IdentifierPart> Parts
        {
            get { return parts; }
        }

        public override object Eval(TemplateContext context)
        {
            object val = null;
            for (int i = 0; i < parts.Count; i++)
            {
                // get variable from context
                if (i == 0 && !parts[i].IsMethod)
                {
                    val = EvalContextVariable(parts[i], context);
                }
                // call built-in function
                else if (i == 0 && parts[i].IsMethod)
                {
                    BuiltinFunctions target = new BuiltinFunctions();
                    target.Context = context;
                    target.Line = parts[i].Line;
                    target.Column = parts[i].Column;
                    val = EvalMethod(target, parts[i], context);
                }
                // call public property
                else if(i > 0 && !parts[i].IsMethod) // property
                {
                    val = EvalProperty(val, parts[i], context);
                }
                // call public method
                else if (i > 0 && parts[i].IsMethod) // property
                {
                    val = EvalMethod(val, parts[i], context);
                }
            }

            return val;
        }

        private object EvalContextVariable(IdentifierPart variable, TemplateContext context)
        {
            object val = null;
            TemplateContext lookupContext = context;

            while (lookupContext != null)
            {
                if (lookupContext.Variables.ContainsKey(variable.Name))
                {
                    val = lookupContext.Variables[variable.Name];
                    break; // found local scope var - stop looking
                }

                // look into parent scope
                lookupContext = lookupContext.ParentContext;
            }

            // get from context
            if (val == null)
                return null;

            // evaluate index expression if required
            val = EvalIndexer(val, variable, context);

            return val;
        }

        private object EvalIndexer(object target, IdentifierPart indexer, TemplateContext context)
        {
            if (indexer.Index == null)
                return target;

            object index = null;
            index = indexer.Index.Eval(context);

            if (index == null)
                throw new ParserException("Indexer expression evaluated to NULL", Line, Column);

            if (target == null)
                throw new ParserException("Cannot call indexer on NULL object", Line, Column);

            // get from index if required
            if(target is IDictionary)
            {
                // dictionary
                return ((IDictionary)target)[index];
            }
            else if (target is Array)
            {
                // array
                if(index is Int32)
                    return ((Array)target).GetValue((Int32)index);
                else
                    throw new ParserException("Array index expression must evaluate to integer.", Line, Column);
            }
            else if (target is IList)
            {
                // list
                if(index is Int32)
                    return ((IList)target)[(Int32)index];
                else
                    throw new ParserException("IList index expression must evaluate to integer.", Line, Column);
            }
            else
            {
                // call "get_Item" method
                MethodInfo methodInfo = target.GetType().GetMethod("get_Item",
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo != null)
                    return methodInfo.Invoke(target, new object[] { index });
            }
            throw new ParserException("Cannot call indexer", Line, Column);
        }

        private object EvalProperty(object target, IdentifierPart property, TemplateContext context)
        {
            object val = target;

            // check for null
            if (val == null)
                throw new ParserException("Cannot read property value of NULL object", Line, Column);

            // get property
            string propName = property.Name;
            PropertyInfo prop = val.GetType().GetProperty(propName,
                BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

            if (prop == null)
            {
                // build the list of available properties
                StringBuilder propsList = new StringBuilder();
                int vi = 0;
                PropertyInfo[] props = val.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo p in props)
                {
                    if (vi++ > 0)
                        propsList.Append(",");
                    propsList.Append(p.Name);
                }
                throw new ParserException("Public property could not be found: " + propName + "."
                    + " Supported properties: " + propsList.ToString(), Line, Column);
            }

            // read property
            try
            {
                val = prop.GetValue(val, null);
            }
            catch (Exception ex)
            {
                throw new ParserException("Cannot read property value: " + ex.Message, Line, Column);
            }

            // evaluate index expression if required
            val = EvalIndexer(val, property, context);

            return val;
        }

        private object EvalMethod(object target, IdentifierPart method, TemplateContext context)
        {
            object val = target;

            // check for null
            if (val == null)
                throw new ParserException("Cannot perform method of NULL object", Line, Column);

            // evaluate method parameters
            object[] prms = new object[method.MethodParameters.Count];
            Type[] prmTypes = new Type[method.MethodParameters.Count];

            for (int i = 0; i < prms.Length; i++)
            {
                prms[i] = method.MethodParameters[i].Eval(context);
                if (prms[i] != null)
                    prmTypes[i] = prms[i].GetType();
                else
                    prmTypes[i] = typeof(object); // "null" parameter was specified
            }

            // find method
            string methodName = method.Name;
            MethodInfo methodInfo = val.GetType().GetMethod(methodName,
                BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public,
                null, prmTypes, null);

            if (methodInfo == null)
            {
                // failed to find exact signature
                // try to iterate
                methodInfo = val.GetType().GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            }

            if (methodInfo == null)
            {
                // build the list of available methods
                StringBuilder methodsList = new StringBuilder();
                int vi = 0;
                MethodInfo[] methods = val.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
                foreach (MethodInfo mi in methods)
                {
                    if (vi++ > 0)
                        methodsList.Append(",");
                    methodsList.Append(mi.Name);
                }
                throw new ParserException("Public method could not be found: " + methodName + "."
                    + " Available methods: " + methodsList.ToString(), Line, Column);
            }

            // call method
            try
            {
                val = methodInfo.Invoke(val, prms);
            }
            catch (Exception ex)
            {
                throw new ParserException("Cannot call method: " + ex.Message, Line, Column);
            }

            return val;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Parts.Count; i++)
            {
                if (i > 0)
                    sb.Append(".");
                sb.Append(Parts[i]);
            }
            return sb.ToString();
        }
    }
}
