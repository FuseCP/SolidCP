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
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SolidCP.Templates
{
    internal class Lexer
    {
        // error codes
        const string LEXER_ERROR_UNEXPECTED_SYMBOL      = "TEMPLATES_LEXER_UNEXPECTED_SYMBOL";
        const string LEXER_ERROR_UNKNOWN_ESCAPE         = "TEMPLATES_LEXER_UNKNOWN_ESCAPE";
        const string LEXER_ERROR_UNTERMINATED_STRING    = "TEMPLATES_LEXER_UNTERMINATED_STRING";
        const string LEXER_ERROR_UNKNOWN_CLOSING_TAG    = "TEMPLATES_LEXER_UNKNOWN_TAG";

        // other constants
        const char EOF = (char)0;

        // private fields
        static Dictionary<string, TokenType> keywords;
        static Dictionary<string, TokenType> startTags;
        static Dictionary<string, TokenType> closingTags;
        string data;
        int pos;
        int column;
        int line;
        int savePos;
        int saveColumn;
        int saveLine;

        bool printStart; // inside #...# print statement
        bool tagOpen;
        bool tagClose;
        bool tagBody; // inside <ad:..   > tag
        bool expressionBody; // inside #...# expression
        bool attributeBody; // inside "..." tag attribute

        static Lexer()
        {
            keywords = new Dictionary<string, TokenType>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"null", TokenType.Null},
                {"true", TokenType.True},
                {"false", TokenType.False},
                {"empty", TokenType.Empty},
                {"or", TokenType.Or},
                {"and", TokenType.And},
                {"not", TokenType.Not},
                {"is", TokenType.Equal},
                {"isnot", TokenType.NotEqual},
                {"lt", TokenType.Less},
                {"lte", TokenType.LessOrEqual},
                {"gt", TokenType.Greater},
                {"gte", TokenType.GreaterOrEqual}
            };

            startTags = new Dictionary<string, TokenType>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"if", TokenType.If},
                {"elseif", TokenType.ElseIf},
                {"else", TokenType.Else},
                {"foreach", TokenType.Foreach},
                {"for", TokenType.For},
                {"set", TokenType.Set},
                {"template", TokenType.Template},
            };

            closingTags = new Dictionary<string, TokenType>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"if", TokenType.EndIf},
                {"foreach", TokenType.EndForeach},
                {"for", TokenType.EndFor},
                {"template", TokenType.EndTemplate}
            };
        }

        public Lexer(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            this.data = reader.ReadToEnd();

            Reset();
        }

        public Lexer(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.data = data;

            Reset();
        }

        private void Reset()
        {
            pos = 0;
            column = 1;
            line = 1;
            tagBody = false;
        }

        public Token Next()
        {
            StartRead();

            if (expressionBody)
                return ReadExpression();
            else if (tagOpen)
                return ReadStartTag();
            else if (tagClose)
                return ReadClosingTag();
            else if (tagBody)
                return ReadTagBody();
            else
                return ReadText();
        }

        private Token ReadText()
        {
            StartRead();
        start:
            switch (LA(0))
            {
                case EOF:
                    if (savePos == pos)
                        return CreateToken(TokenType.EOF);
                    else
                        break;
                case '#':
                    if (LA(1) == '#')
                    {
                        // ## escape
                        Consume(2);
                        goto start;
                    }
                    else if (printStart)
                    {
                        Consume(); // eat #
                        printStart = false;
                        expressionBody = true;
                        return CreateToken(TokenType.Print);
                    }
                    else
                    {
                        // START reading "print" statement
                        printStart = true;
                        break;// return CreateToken(TokenType.Print);
                    }
                case '<':
                    if (LA(1) == 'a' && LA(2) == 'd' && LA(3) == ':')
                    {
                        // opening tag
                        tagOpen = true;
                        break;
                    }
                    else if (LA(1) == '/' && LA(2) == 'a' && LA(3) == 'd' && LA(4) == ':')
                    {
                        // closing tag
                        tagClose = true;
                        break;
                    }
                    Consume(); // eat <
                    goto start;
                case '\n':
                case '\r':
                    ReadWhitespace();
                    goto start;
                default:
                    Consume();
                    goto start;
            }

            return CreateToken(TokenType.Text);
        }

        private Token ReadStartTag()
        {
            Consume(4); // eat "<ad:"
            tagOpen = false;
            tagBody = true;

            // read tag name
            string name = ReadTagName();

            // check for standard tags
            if (startTags.ContainsKey(name))
                return CreateToken(startTags[name], name);

            return CreateToken(TokenType.OpenTag, name);
        }

        private Token ReadClosingTag()
        {
            Consume(5); // eat "</ad:"
            tagClose = false;

            // read tag name
            string name = ReadTagName();

            if (LA(0) == '>')
            {
                Consume();
                if (closingTags.ContainsKey(name))
                    return CreateToken(closingTags[name], name);
                else
                    return CreateToken(TokenType.CloseTag, name);
            }

            return CreateToken(TokenType.Text);
        }

        private string ReadTagName()
        {
            int startPos = pos; // save current position

            Consume(); // consume first character of the word

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetterOrDigit(ch))
                    Consume();
                else
                    break;
            }

            return data.Substring(startPos, pos - startPos);
        }

        private Token ReadTagBody()
        {
            StartRead();
        start:
            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return CreateToken(TokenType.EOF);
                case '>':
                    Consume();
                    tagBody = false;
                    return Next();
                case '=':
                    Consume();
                    return CreateToken(TokenType.Assign);
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    ReadWhitespace();
                    goto start;

                case '"':
                    if (attributeBody)
                    {
                        // end of attribute
                        Consume(); // eat "
                        attributeBody = false;
                    }
                    else
                    {
                        // start of attribute
                        attributeBody = true;
                        if (LA(1) == '#')
                        {
                            Consume(2); // eat "#
                            expressionBody = true;
                            return Next();
                        }
                        else
                        {
                            attributeBody = false;

                            if (Char.IsNumber(LA(1)))
                            {
                                Consume(); // eat "
                                Token num = ReadNumber();
                                Consume(); // eat "
                                return num;
                            }
                            else
                                return ReadString();
                        }
                    }
                    goto start;
                case '\'':
                    return ReadString();

                default:
                    if (ch == '/' && LA(1) == '>')
                    {
                        // "empty" tag
                        Consume(2);
                        tagBody = false;
                        return CreateToken(TokenType.EmptyTag);
                    }
                    else if (Char.IsLetter(ch) || ch == '_')
                    {
                        return ReadAttribute();
                    }
                    else
                    {
                        string symbol = ch.ToString();
                        ParserException ex = new ParserException(String.Format("Unexpected symbol: '{0}'", symbol),
                            saveLine, saveColumn, pos, 1);
                        ex.Data["code"] = LEXER_ERROR_UNEXPECTED_SYMBOL;
                        ex.Data["symbol"] = symbol;
                        throw ex;
                    }
            }
        }

        private Token ReadExpression()
        {
            StartRead();
        start:
            char ch = LA(0);
            switch (ch)
            {
                case EOF:
                    return CreateToken(TokenType.EOF);
                case ':':
                    Consume();
                    return CreateToken(TokenType.Colon);
                case '.':
                    Consume();
                    return CreateToken(TokenType.Dot);
                case ',':
                    Consume();
                    return CreateToken(TokenType.Comma);
                case '(':
                    Consume();
                    return CreateToken(TokenType.LParen);
                case ')':
                    Consume();
                    return CreateToken(TokenType.RParen);
                case '[':
                    Consume();
                    return CreateToken(TokenType.LBracket);
                case ']':
                    Consume();
                    return CreateToken(TokenType.RBracket);
                case '#': // end of expression
                    Consume();
                    printStart = false;
                    expressionBody = false;
                    return Next();
                case '+':
                    Consume();
                    return CreateToken(TokenType.Plus);
                case '-':
                    Consume();
                    return CreateToken(TokenType.Minus);
                case '*':
                    Consume();
                    return CreateToken(TokenType.Mult);
                case '/':
                    Consume();
                    return CreateToken(TokenType.Div);
                case '%':
                    Consume();
                    return CreateToken(TokenType.Mod);
                case '<':
                    if (LA(1) == '=')
                    {
                        Consume(2);
                        return CreateToken(TokenType.LessOrEqual);
                    }
                    else
                    {
                        Consume();
                        return CreateToken(TokenType.Less);
                    }
                case '>':
                    if (LA(1) == '=')
                    {
                        Consume(2);
                        return CreateToken(TokenType.GreaterOrEqual);
                    }
                    else
                    {
                        Consume();
                        return CreateToken(TokenType.Greater);
                    }
                case '=':
                    if (LA(1) == '=')
                    {
                        Consume(2);
                        return CreateToken(TokenType.Equal);
                    }
                    else
                    {
                        Consume();
                        return CreateToken(TokenType.Assign);
                    }
                case '!':
                    if (LA(1) == '=')
                    {
                        Consume(2);
                        return CreateToken(TokenType.NotEqual);
                    }
                    else
                    {
                        Consume();
                        return CreateToken(TokenType.Not);
                    }
                case '|':
                    if (LA(1) == '|')
                    {
                        Consume(2);
                        return CreateToken(TokenType.Or);
                    }
                    else
                    {
                        Consume();
                        return CreateToken(TokenType.BinOr);
                    }
                case '&':
                    if (LA(1) == '&')
                    {
                        Consume(2);
                        return CreateToken(TokenType.And);
                    }
                    else
                    {
                        Consume();
                        return CreateToken(TokenType.BinAnd);
                    }

                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    ReadWhitespace();
                    goto start;

                case '"':
                case '\'':
                    return ReadString();

                default:
                    if (Char.IsNumber(ch))
                        return ReadNumber();
                    else if (Char.IsLetter(ch) || ch == '_')
                        return ReadIdentifier();
                    else
                    {
                        string symbol = ch.ToString();
                        ParserException ex = new ParserException(String.Format("Unexpected symbol: '{0}'", symbol),
                            saveLine, saveColumn, pos, 1);
                        ex.Data["code"] = LEXER_ERROR_UNEXPECTED_SYMBOL;
                        ex.Data["symbol"] = symbol;
                        throw ex;
                    }
            }
        }

        private Token ReadAttribute()
        {
            StartRead();

            Consume(); // consume first character of the word

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetterOrDigit(ch) || ch == '_')
                    Consume();
                else
                    break;
            }

            string tokenData = data.Substring(savePos, pos - savePos);
            return CreateToken(TokenType.Attribute, tokenData);
        }

        private Token ReadIdentifier()
        {
            StartRead();

            Consume(); // consume first character of the word

            while (true)
            {
                char ch = LA(0);
                if (Char.IsLetterOrDigit(ch) || ch == '_')
                    Consume();
                else
                    break;
            }

            string tokenData = data.Substring(savePos, pos - savePos);

            if (keywords.ContainsKey(tokenData))
                return CreateToken(keywords[tokenData]);
            else
                return CreateToken(TokenType.Identifier, tokenData);
        }

        private Token ReadNumber()
        {
            StartRead();
            Consume(); // consume first digit or -

            bool hasDot = false;

            while (true)
            {
                char ch = LA(0);
                if (Char.IsNumber(ch))
                    Consume();

                // if "." and didn't see "." yet, and next char
                // is number, than starting to read decimal number
                else if (ch == '.' && !hasDot && Char.IsNumber(LA(1)))
                {
                    Consume();
                    hasDot = true;
                }
                else
                    break;
            }

            return CreateToken(hasDot ? TokenType.Decimal : TokenType.Integer);
        }

        private Token ReadString()
        {
            StartRead();

            char startSymbol = LA(0);
            Consume(); // consume first string character

            while (true)
            {
                char ch = LA(0);
                if (ch == startSymbol)
                {
                    Consume();
                    string tokenData = data.Substring(savePos + 1, pos - savePos - 2);
                    return CreateToken(TokenType.String, tokenData);
                }
                else if(ch == '\\')
                {
                    if (LA(1) == 'n' || LA(1) == 'r' || LA(1) == '\'' || LA(1) == '"')
                    {
                        // string escape
                        Consume(2);
                        continue;
                    }
                    else
                    {
                        ParserException ex = new ParserException("Unknown escape sequence. Supported escapes: \\n, \\r, \\\", \\'",
                            line, column, pos, 2);
                        ex.Data["code"] = LEXER_ERROR_UNKNOWN_ESCAPE;
                        throw ex;
                    }
                }
                else if (ch == EOF)
                {
                    ParserException ex = new ParserException("Unterminated string constant", saveLine, saveColumn,
                        savePos, pos - savePos);
                    ex.Data["code"] = LEXER_ERROR_UNTERMINATED_STRING;
                    throw ex;
                }
                else
                {
                    // consume next string symbol
                    Consume();
                }
            }
        }

        private void ReadWhitespace()
        {
            while (true)
            {
                char ch = LA(0);
                switch (ch)
                {
                    case ' ':
                    case '\t':
                        Consume();
                        break;
                    case '\n':
                        Consume();
                        NewLine();
                        break;

                    case '\r':
                        Consume();
                        if (LA(0) == '\n')
                            Consume();
                        NewLine();
                        break;
                    default:
                        return;
                }
            }
        }

        #region Helper Methods
        private char LA(int count)
        {
            if (pos + count >= data.Length)
                return EOF;
            else
            {
                //Debug.Write(data[pos + count]);
                return data[pos + count];
            }
        }

        private char Consume()
        {
            char ret = data[pos];
            pos++;
            column++;

            return ret;
        }

        private char Consume(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count", "'count' cannot be less than 0");

            char ret = ' ';
            while (count > 0)
            {
                ret = Consume();
                count--;
            }
            return ret;
        }

        private void NewLine()
        {
            line++;
            column = 1;
        }

        private void StartRead()
        {
            saveLine = line;
            saveColumn = column;
            savePos = pos;
        }

        private Token CreateToken(TokenType type, string value)
        {
            return new Token(type, value, saveLine, saveColumn, savePos, value.Length);
        }

        private Token CreateToken(TokenType type)
        {
            string tokenData = data.Substring(savePos, pos - savePos);

            if (type == TokenType.String)
            {
                // process escapes
                tokenData = tokenData.Replace("\\n", "\n")
                            .Replace("\\r", "\r")
                            .Replace("\\\"", "\"")
                            .Replace("\\'", "'");
            }
            else if (type == TokenType.Text)
            {
                tokenData = tokenData.Replace("##", "#");
            }

            return new Token(type, tokenData, saveLine, saveColumn, savePos, tokenData.Length);
        }
        #endregion
    }
}
