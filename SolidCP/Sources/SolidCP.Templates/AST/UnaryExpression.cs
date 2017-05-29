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
    internal class UnaryExpression : Expression
    {
        Expression rhs;
        TokenType op;

        public UnaryExpression(int line, int column, TokenType op, Expression rhs)
            : base(line, column)
        {
            this.op = op;
            this.rhs = rhs;
        }

        public TokenType Operator
        {
            get { return op; }
        }

        Expression RightExpression
        {
            get { return rhs; }
            set { rhs = value; }
        }

        public override object Eval(TemplateContext context)
        {
            // evaluate right side
            object val = rhs.Eval(context);

            if (op == TokenType.Minus && val is Decimal)
                return -(Decimal)val;
            else if (op == TokenType.Minus && val is Int32)
                return -(Int32)val;
            else if (op == TokenType.Not && val is Boolean)
                return !(Boolean)val;

            throw new ParserException("Unary operator can be applied to integer, decimal and boolean expressions.", Line, Column);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Operator).Append("(").Append(RightExpression).Append(")");
            
            return sb.ToString();
        }
    }
}
