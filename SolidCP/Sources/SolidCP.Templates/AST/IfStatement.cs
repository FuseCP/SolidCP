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
using System.Collections.Generic;
using System.Text;

namespace SolidCP.Templates.AST
{
    internal class IfStatement : Statement
    {
        Expression condition;
        List<ElseIfStatement> elseIfStatements = new List<ElseIfStatement>();
        List<Statement> trueStatements = new List<Statement>();
        List<Statement> falseStatements = new List<Statement>();

        public IfStatement(int line, int column)
            : base(line, column)
        {
        }

        public Expression Condition
        {
            get { return condition; }
            set { condition = value; }
        }

        public List<ElseIfStatement> ElseIfStatements
        {
            get { return elseIfStatements; }
        }

        public List<Statement> TrueStatements
        {
            get { return trueStatements; }
        }

        public List<Statement> FalseStatements
        {
            get { return falseStatements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            // evaluate test condition
            bool result = EvalCondition(Condition, context);

            if (result)
            {
                foreach (Statement stm in TrueStatements)
                    stm.Eval(context, writer);
                return;
            }
            else
            {
                // process else if statements
                foreach (ElseIfStatement stm in ElseIfStatements)
                {
                    if (EvalCondition(stm.Condition, context))
                    {
                        stm.Eval(context, writer);
                        return;
                    }
                }

                // process else
                foreach (Statement stm in FalseStatements)
                    stm.Eval(context, writer);
            }
        }

        private bool EvalCondition(Expression expr, TemplateContext context)
        {
            object val = expr.Eval(context);
            if (val is Boolean)
                return (Boolean)val;
            else if (val is Int32)
                return ((Int32)val) != 0;
            else if (val is Decimal)
                return ((Decimal)val) != 0;
            else if (val != null)
                return true;
            else
                return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{if ")
                .Append(Condition.ToString()).Append("}");

            // true statements
            foreach (Statement stm in TrueStatements)
                sb.Append(stm);

            // elseif statements
            foreach (Statement stm in ElseIfStatements)
                sb.Append(stm);

            // false statements
            if(FalseStatements.Count > 0)
            {
                sb.Append("{else}");
                foreach (Statement stm in FalseStatements)
                    sb.Append(stm);
            }

            sb.Append("{/if}");
            return sb.ToString();
        }
    }
}
