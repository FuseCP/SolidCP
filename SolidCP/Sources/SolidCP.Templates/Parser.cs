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
using System.Globalization;
using System.Diagnostics;

using SolidCP.Templates.AST;

namespace SolidCP.Templates
{
    internal class Parser
    {
        Lexer lexer;
		Token current;
		List<Statement> statements;

		public Parser(Lexer lexer)
		{
			this.lexer = lexer;
			this.statements = new List<Statement>();
		}

		Token Consume()
		{
			Token old = current;
			current = lexer.Next();
			return old;
		}

		Token Consume(TokenType type)
		{
			Token old = current;
			current = lexer.Next();

			if (old.TokenType != type)
				throw new ParserException("Unexpected token: " + current.TokenType.ToString() + ". Was expecting: " + type,
                    current.Line, current.Column);

			return old;
		}

		Token Current
		{
			get { return current; }
		}

		public List<Statement> Parse()
		{
			statements.Clear();

			Consume();

			while (true)
			{
				Statement stm = ParseStatement();
				if (stm == null)
					break;
				else
					statements.Add(stm);
			}

            foreach (Statement stm in statements)
                Debug.Write(stm);

			return statements;
		}


        private Statement ParseStatement()
        {
            switch (Current.TokenType)
            {
                case TokenType.EOF:
                    return null;
                case TokenType.Print:
                    return ParsePrintStatement();
                case TokenType.Set:
                    return ParseSetStatement();
                case TokenType.If:
                    return ParseIfStatement();
                case TokenType.Foreach:
                    return ParseForeachStatement();
                case TokenType.For:
                    return ParseForStatement();
                case TokenType.Template:
                    return ParseTemplateStatement();
                case TokenType.OpenTag:
                    return ParseCallTemplateStatement();
                case TokenType.Text:
                    TextStatement text = new TextStatement(Current.Data, Current.Line, Current.Column);
                    Consume();
                    return text;
                default:
                    throw new ParserException("Invalid token: " + Current.TokenType.ToString(), Current.Line, Current.Column);
            }
        }

        private Statement ParsePrintStatement()
        {
            Consume(TokenType.Print);
            PrintStatement stm = new PrintStatement(current.Line, current.Column);
            stm.PrintExpression = ParseExpression();
            return stm;
        }

        private Statement ParseSetStatement()
        {
            // parse statement declaration
            SetStatement stm = new SetStatement(current.Line, current.Column);
            Consume(TokenType.Set);

            // parse tag attributes
            do
            {
                if (Current.TokenType != TokenType.Attribute)
                    throw new ParserException("Expected tag attribute", stm.Line, stm.Column);

                string attrName = Current.Data;
                Consume(TokenType.Attribute);
                Consume(TokenType.Assign);

                // look for "test" attribute
                if (attrName == "name")
                {
                    stm.Name = Current.Data;
                    Consume(TokenType.String);
                }
                else if (attrName == "value")
                {
                    stm.ValueExpression = ParseExpression();
                }
            }
            while (Current.TokenType == TokenType.Attribute);

            // check statement
            if (stm.Name == null)
                throw new ParserException("\"name\" attribute was not specified", stm.Line, stm.Column);
            if (stm.ValueExpression == null)
                throw new ParserException("\"value\" attribute was not specified", stm.Line, stm.Column);

            // it must be an "empty" tag
            Consume(TokenType.EmptyTag);

            return stm;
        }

        private Statement ParseIfStatement()
        {
            // parse statement declaration
            IfStatement stm = new IfStatement(current.Line, current.Column);
            Consume(TokenType.If);

            if(Current.TokenType != TokenType.Attribute)
                throw new ParserException("Expected tag attribute", stm.Line, stm.Column);

            // look for "test" attribute
            if (Current.Data == "test")
            {
                Consume(TokenType.Attribute);
                Consume(TokenType.Assign);
                stm.Condition = ParseExpression();
            }

            // check statement
            if (stm.Condition == null)
                throw new ParserException("\"test\" attribute was not specified", stm.Line, stm.Column);

            bool falseBlock = false;

            // parse body
            while (true)
            {
                if (Current.TokenType == TokenType.EndIf)
                {
                    break;
                }
                else if (Current.TokenType == TokenType.Else)
                {
                    Consume(); // else
                    falseBlock = true;
                    continue;
                }
                else if (Current.TokenType == TokenType.ElseIf)
                {
                    stm.ElseIfStatements.Add(ParseElseIfStatement());
                }
                else
                {
                    // parse statement
                    Statement blockStm = ParseStatement();
                    if (blockStm == null)
                        throw new ParserException("Expected closing </ad:if> tag", stm.Line, stm.Column);

                    if (falseBlock)
                        stm.FalseStatements.Add(blockStm);
                    else
                        stm.TrueStatements.Add(blockStm);
                }
            }

            Consume(TokenType.EndIf);

            return stm;
        }

        private ElseIfStatement ParseElseIfStatement()
        {
            // parse statement declaration
            ElseIfStatement stm = new ElseIfStatement(current.Line, current.Column);
            Consume(TokenType.ElseIf);

            if (Current.TokenType != TokenType.Attribute)
                throw new ParserException("Expected tag attribute", stm.Line, stm.Column);

            // look for "test" attribute
            if (Current.Data == "test")
            {
                Consume(TokenType.Attribute);
                Consume(TokenType.Assign);
                stm.Condition = ParseExpression();
            }

            // check statement
            if (stm.Condition == null)
                throw new ParserException("\"test\" attribute was not specified", stm.Line, stm.Column);

            // parse body
            while (true)
            {
                if (Current.TokenType == TokenType.ElseIf
                    || Current.TokenType == TokenType.Else
                    || Current.TokenType == TokenType.EndIf)
                {
                    break;
                }
                else
                {
                    Statement blockStm = ParseStatement();
                    if (blockStm == null)
                        throw new ParserException("Expected <ad:elseif>, <ad:else> or </ad:if> tags", stm.Line, stm.Column);

                    // parse statement
                    stm.TrueStatements.Add(blockStm);
                }
            }

            return stm;
        }

        private Statement ParseForeachStatement()
        {
            // parse statement declaration
            ForeachStatement stm = new ForeachStatement(current.Line, current.Column);
            Consume(TokenType.Foreach);

            // parse tag attributes
            do
            {
                if (Current.TokenType != TokenType.Attribute)
                    throw new ParserException("Expected tag attribute", stm.Line, stm.Column);

                string attrName = Current.Data;
                Consume(TokenType.Attribute);
                Consume(TokenType.Assign);

                // look for "test" attribute
                if (attrName == "collection")
                {
                    stm.Collection = ParseExpression();
                }
                else if (attrName == "var")
                {
                    stm.ElementIdentifier = Current.Data;
                    Consume(TokenType.String);
                }
                else if (attrName == "index")
                {
                    stm.IndexIdentifier = Current.Data;
                    Consume(TokenType.String);
                }
            }
            while (Current.TokenType == TokenType.Attribute);

            // check statement
            if (stm.Collection == null)
                throw new ParserException("\"collection\" attribute was not specified", stm.Line, stm.Column);
            if (stm.ElementIdentifier == null)
                throw new ParserException("\"var\" attribute was not specified", stm.Line, stm.Column);

            // parse body
            while (true)
            {
                if (Current.TokenType == TokenType.EndForeach)
                {
                    break;
                }
                else
                {
                    Statement blockStm = ParseStatement();
                    if (blockStm == null)
                        throw new ParserException("Expected {/foreach} statement", stm.Line, stm.Column);

                    // parse statement
                    stm.Statements.Add(blockStm);
                }
            }

            Consume(TokenType.EndForeach);

            return stm;
        }

        private Statement ParseForStatement()
        {
            // parse statement declaration
            ForStatement stm = new ForStatement(current.Line, current.Column);
            Consume(TokenType.For);

            // parse tag attributes
            do
            {
                if (Current.TokenType != TokenType.Attribute)
                    throw new ParserException("Expected tag attribute", stm.Line, stm.Column);

                string attrName = Current.Data;
                Consume(TokenType.Attribute);
                Consume(TokenType.Assign);

                // look for "test" attribute
                if (attrName == "from")
                {
                    stm.StartIndex = ParseExpression();
                }
                else if (attrName == "to")
                {
                    stm.EndIndex = ParseExpression();
                }
                else if (attrName == "index")
                {
                    stm.IndexIdentifier = Current.Data;
                    Consume(TokenType.String);
                }
            }
            while (Current.TokenType == TokenType.Attribute);

            // check statement
            if (stm.StartIndex == null)
                throw new ParserException("\"from\" attribute was not specified", stm.Line, stm.Column);
            if (stm.EndIndex == null)
                throw new ParserException("\"to\" attribute was not specified", stm.Line, stm.Column);
            if (stm.IndexIdentifier == null)
                throw new ParserException("\"index\" attribute was not specified", stm.Line, stm.Column);

            // parse body
            while (true)
            {
                if (Current.TokenType == TokenType.EndFor)
                {
                    break;
                }
                else
                {
                    Statement blockStm = ParseStatement();
                    if (blockStm == null)
                        throw new ParserException("Expected </ad:for> tag", stm.Line, stm.Column);

                    // parse statement
                    stm.Statements.Add(blockStm);
                }
            }

            Consume(TokenType.EndFor);

            return stm;
        }

        private Statement ParseTemplateStatement()
        {
            // parse statement declaration
            TemplateStatement stm = new TemplateStatement(current.Line, current.Column);
            Consume(TokenType.Template);

            // parse tag attributes
            do
            {
                if (Current.TokenType != TokenType.Attribute)
                    throw new ParserException("Expected tag attribute", stm.Line, stm.Column);

                string attrName = Current.Data;
                Consume(TokenType.Attribute);
                Consume(TokenType.Assign);

                // look for "test" attribute
                if (attrName == "name")
                {
                    stm.Name = Current.Data;
                    Consume(TokenType.String);
                }
            }
            while (Current.TokenType == TokenType.Attribute);

            // check statement
            if (stm.Name == null)
                throw new ParserException("\"name\" attribute was not specified", stm.Line, stm.Column);

            // parse body
            while (true)
            {
                if (Current.TokenType == TokenType.EndTemplate)
                {
                    break;
                }
                else
                {
                    Statement blockStm = ParseStatement();
                    if (blockStm == null)
                        throw new ParserException("Expected {/template} tag", stm.Line, stm.Column);

                    // parse statement
                    stm.Statements.Add(blockStm);
                }
            }

            Consume(TokenType.EndTemplate);

            return stm;
        }

        private Statement ParseCallTemplateStatement()
        {
            // parse statement declaration
            CallTemplateStatement stm = new CallTemplateStatement(current.Line, current.Column);
            stm.TemplateName = Current.Data;
            Consume(TokenType.OpenTag);

            // parse tag attributes
            do
            {
                if (Current.TokenType != TokenType.Attribute)
                    throw new ParserException("Expected tag attribute", stm.Line, stm.Column);

                string attrName = Current.Data;
                Consume(TokenType.Attribute);
                Consume(TokenType.Assign);

                // save parameter
                stm.Parameters.Add(attrName, ParseExpression());
            }
            while (Current.TokenType == TokenType.Attribute);

            // parse body
            while (true)
            {
                if (Current.TokenType == TokenType.EmptyTag)
                {
                    // />
                    Consume(TokenType.EmptyTag);
                    break;
                }
                else if (Current.TokenType == TokenType.CloseTag
                    && Current.Data == stm.TemplateName)
                {
                    Consume(TokenType.CloseTag);
                    break;
                }
                else
                {
                    Statement blockStm = ParseStatement();
                    if (blockStm == null)
                        throw new ParserException("Expected closing tag", stm.Line, stm.Column);

                    // parse statement
                    stm.Statements.Add(blockStm);
                }
            }
            return stm; 
        }

        private Expression ParseExpression()
        {
            return ParseLogicalExpression();
        }

        private Expression ParseLogicalExpression()
        {
            Expression lhs = ParseRelationalExpression();
            return ParseLogicalExpressionRest(lhs);
        }

        private Expression ParseLogicalExpressionRest(Expression lhs)
        {
            if (Current.TokenType == TokenType.And || Current.TokenType == TokenType.Or)
            {
                Consume(); // &&, ||
                Expression rhs = ParseRelationalExpression();
                BinaryExpression lhsChild = new BinaryExpression(lhs.Line, lhs.Column, lhs, TokenType.And, rhs);
                return ParseLogicalExpressionRest(lhsChild);
            }
            else
            {
                return lhs;
            }
        }

        private Expression ParseRelationalExpression()
        {
            Expression lhs = ParseAdditiveExpression();

            if (Current.TokenType == TokenType.Less
                || Current.TokenType == TokenType.LessOrEqual
                || Current.TokenType == TokenType.Greater
                || Current.TokenType == TokenType.GreaterOrEqual
                || Current.TokenType == TokenType.Equal
                || Current.TokenType == TokenType.NotEqual)
            {
                Token tok = Consume(); // <, >, <=, >=, ==, !=
                Expression rhs = ParseRelationalExpression(); // recursion
                return new BinaryExpression(lhs.Line, lhs.Column, lhs, tok.TokenType, rhs);
            }

            return lhs;
        }

        private Expression ParseAdditiveExpression()
        {
            Expression lhs = ParseMultiplyExpression();
            return ParseAdditiveExpressionRest(lhs);
        }

        private Expression ParseAdditiveExpressionRest(Expression lhs)
        {
            if (Current.TokenType == TokenType.Plus
                || Current.TokenType == TokenType.Minus)
            {
                Token tok = Consume(); // +, -
                Expression rhs = ParseMultiplyExpression();
                BinaryExpression lhsChild = new BinaryExpression(lhs.Line, lhs.Column, lhs, tok.TokenType, rhs);
                return ParseAdditiveExpressionRest(lhsChild);
            }
            else
            {
                return lhs;
            }
        }

        private Expression ParseMultiplyExpression()
        {
            Expression lhs = ParseUnaryExpression();
            return ParseMultiplyExpressionRest(lhs);
        }

        private Expression ParseMultiplyExpressionRest(Expression lhs)
        {
            if (Current.TokenType == TokenType.Mult
                || Current.TokenType == TokenType.Div
                || Current.TokenType == TokenType.Mod)
            {
                Token tok = Consume(); // *, /, %
                Expression rhs = ParseUnaryExpression();
                BinaryExpression lhsChild = new BinaryExpression(lhs.Line, lhs.Column, lhs, tok.TokenType, rhs);
                return ParseMultiplyExpressionRest(lhsChild);
            }

            return lhs;
        }

        private Expression ParseUnaryExpression()
        {
            if (Current.TokenType == TokenType.Minus ||
                Current.TokenType == TokenType.Not)
            {
                Token tok = Consume(); // -, !
                Expression rhs = ParseUnaryExpression();
                UnaryExpression exp = new UnaryExpression(Current.Line, Current.Column, tok.TokenType, rhs);
                return exp;
            }

            return ParsePrimaryExpression();
        }

        private Expression ParsePrimaryExpression()
        {
            if (Current.TokenType == TokenType.String)
            {
                LiteralExpression literal = new LiteralExpression(Current.Line, Current.Column, Current.Data);
                Consume();
                return literal;
            }
            else if (Current.TokenType == TokenType.Null)
            {
                LiteralExpression literal = new LiteralExpression(Current.Line, Current.Column, null);
                Consume();
                return literal;
            }
            else if (Current.TokenType == TokenType.True)
            {
                LiteralExpression literal = new LiteralExpression(Current.Line, Current.Column, true);
                Consume();
                return literal;
            }
            else if (Current.TokenType == TokenType.False)
            {
                LiteralExpression literal = new LiteralExpression(Current.Line, Current.Column, false);
                Consume();
                return literal;
            }
            else if (Current.TokenType == TokenType.Integer)
            {
                LiteralExpression literal = new LiteralExpression(Current.Line, Current.Column, Int32.Parse(Current.Data));
                Consume();
                return literal;
            }
            else if (Current.TokenType == TokenType.Decimal)
            {
                LiteralExpression literal = new LiteralExpression(Current.Line, Current.Column,
                    Decimal.Parse(Current.Data, NumberFormatInfo.InvariantInfo));
                Consume();
                return literal;
            }
            else if (Current.TokenType == TokenType.Identifier)
            {
                IdentifierExpression id = new IdentifierExpression(Current.Line, Current.Column);
                while (Current.TokenType == TokenType.Identifier)
                {
                    id.Parts.Add(ParseIdentifierPart());

                    if (Current.TokenType != TokenType.Dot)
                        break;
                    else
                    {
                        Consume(); // .
                        if(Current.TokenType != TokenType.Identifier)
                            throw new ParserException("Wrong usage of period. Identifier was expected.", Current.Line, Current.Column);
                    }
                }

                return id;
            }
            else if (Current.TokenType == TokenType.LParen)
            {
                Consume(); // eat (
                Expression exp = ParseExpression();
                Consume(TokenType.RParen); // eat )

                return exp;
            }
            else
                throw new ParserException("Invalid token in expression: " + Current.TokenType + ". Was expecting identifier, string or number", Current.Line, Current.Column);
        }

        private IdentifierPart ParseIdentifierPart()
        {
            IdentifierPart part = new IdentifierPart(Current.Data, Current.Line, Current.Column);
            Consume(); // consume identifier

            // check for indexer
            if (Current.TokenType == TokenType.LBracket)
            {
                Consume(); // [
                part.Index = ParseExpression();
                Consume(TokenType.RBracket); // ]
            }
            // check for method call
            else if (Current.TokenType == TokenType.LParen)
            {
                Consume(); // (
                part.IsMethod = true;

                // parse parameters
                while (Current.TokenType != TokenType.RParen)
                {
                    part.MethodParameters.Add(ParseExpression());
                    if (Current.TokenType == TokenType.RParen)
                        break; // )

                    Consume(TokenType.Comma); // ,
                }
                Consume(TokenType.RParen); // )
            }

            return part;
        }
    }
}
