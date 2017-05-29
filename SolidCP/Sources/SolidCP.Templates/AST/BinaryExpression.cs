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
    internal class BinaryExpression : Expression
    {
        Expression lhs;
        Expression rhs;
        TokenType op;

        public BinaryExpression(int line, int column, Expression lhs, TokenType op, Expression rhs)
            : base(line, column)
        {
            this.lhs = lhs;
            this.op = op;
            this.rhs = rhs;
        }

        public TokenType Operator
        {
            get { return op; }
        }

        Expression LeftExpression
        {
            get { return lhs; }
            set { lhs = value; }
        }

        Expression RightExpression
        {
            get { return rhs; }
            set { rhs = value; }
        }

        public override object Eval(TemplateContext context)
        {
            // evaluate both parts
            object lv = LeftExpression.Eval(context);
            object rv = RightExpression.Eval(context);

            // equality/not-equality
            if (op == TokenType.Equal)
            {
                if (lv == null && rv == null)
                    return true;
                else if (lv != null)
                    return lv.Equals(rv);
            }
            else if (op == TokenType.NotEqual)
            {
                if (lv == null && rv == null)
                    return false;
                else if (lv != null)
                    return !lv.Equals(rv);
            }

            // arithmetic operation
            else if (op == TokenType.Mult
                || op == TokenType.Div
                || op == TokenType.Mod
                || op == TokenType.Plus
                || op == TokenType.Minus
                || op == TokenType.Less
                || op == TokenType.LessOrEqual
                || op == TokenType.Greater
                || op == TokenType.GreaterOrEqual)
            {
                if(!((lv is Decimal || lv is Int32) && (rv is Decimal || rv is Int32)))
                    throw new ParserException("Arithmetic and logical operations can be applied to operands or integer and decimal types only.",
                        Line, Column);
                    
                
                bool dec = lv is Decimal || rv is Decimal;
                object val = null;
                if(op == TokenType.Mult)
                    val = dec ? (Decimal)lv * (Decimal)rv : (Int32)lv * (Int32)rv;
                else if (op == TokenType.Div)
                    val = dec ? (Decimal)lv / (Decimal)rv : (Int32)lv / (Int32)rv;
                else if (op == TokenType.Mod)
                    val = dec ? (Decimal)lv % (Decimal)rv : (Int32)lv % (Int32)rv;
                else if (op == TokenType.Plus)
                    val = dec ? (Decimal)lv + (Decimal)rv : (Int32)lv + (Int32)rv;
                else if (op == TokenType.Minus)
                    val = dec ? (Decimal)lv - (Decimal)rv : (Int32)lv - (Int32)rv;
                else if (op == TokenType.Less)
                    val = dec ? (Decimal)lv < (Decimal)rv : (Int32)lv < (Int32)rv;
                else if (op == TokenType.LessOrEqual)
                    val = dec ? (Decimal)lv <= (Decimal)rv : (Int32)lv <= (Int32)rv;
                else if (op == TokenType.Greater)
                    val = dec ? (Decimal)lv > (Decimal)rv : (Int32)lv > (Int32)rv;
                else if (op == TokenType.GreaterOrEqual)
                    val = dec ? (Decimal)lv >= (Decimal)rv : (Int32)lv >= (Int32)rv;

                if (val is Boolean)
                {
                    bool ret = Convert.ToBoolean(val);
                    return ret;
                }
                else if (dec)
                {
                    decimal ret = Convert.ToDecimal(val);
                    return ret;
                }
                else
                {
                    int ret = Convert.ToInt32(val);
                    return ret;
                }
            }
            else if (op == TokenType.Or || op == TokenType.And)
            {
                if (!(lv is Boolean && rv is Boolean))
                    throw new ParserException("Logical operation can be applied to operands of boolean type only", Line, Column);

                if (op == TokenType.Or)
                    return (Boolean)lv || (Boolean)rv;
                else if (op == TokenType.And)
                    return (Boolean)lv && (Boolean)rv;
            }

            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("(")
                .Append(LeftExpression.ToString()).Append(" ")
                .Append(Operator).Append(" ")
                .Append(RightExpression.ToString()).Append(")");
            return sb.ToString();
        }
    }
}
