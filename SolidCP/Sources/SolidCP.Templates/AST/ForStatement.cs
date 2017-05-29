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
    internal class ForStatement : Statement
    {
        string indexIdentifier;
        Expression startIndex;
        Expression endIndex;
        List<Statement> statements = new List<Statement>();

        public ForStatement(int line, int column)
            : base(line, column)
        {
        }

        public string IndexIdentifier
        {
            get { return indexIdentifier; }
            set { indexIdentifier = value; }
        }

        public Expression StartIndex
        {
            get { return startIndex; }
            set { startIndex = value; }
        }

        public Expression EndIndex
        {
            get { return endIndex; }
            set { endIndex = value; }
        }

        public List<Statement> Statements
        {
            get { return statements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            // evaluate indicies
            object objStartIdx = StartIndex.Eval(context);
            object objEndIdx = EndIndex.Eval(context);

            // check indicies
            if (!(objStartIdx is Int32))
                throw new ParserException("Start index expression should evaluate to integer.", StartIndex.Line, StartIndex.Column);
            if (!(objEndIdx is Int32))
                throw new ParserException("End index expression should evaluate to integer.", StartIndex.Line, StartIndex.Column);

            int startIdx = Convert.ToInt32(objStartIdx);
            int endIdx = Convert.ToInt32(objEndIdx);
            int step = startIdx < endIdx ? 1 : -1;
            endIdx += step;

            int i = startIdx;
            do
            {
                context.Variables[IndexIdentifier] = i;

                // evaluate statements
                foreach (Statement stm in Statements)
                    stm.Eval(context, writer);

                i += step;
            }
            while (i != endIdx);

            // cleanup vars
            context.Variables.Remove(IndexIdentifier);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{for ")
                .Append(IndexIdentifier).Append(" = ")
                .Append(StartIndex).Append(" to ").Append(EndIndex).Append("}");
            foreach (Statement stm in Statements)
            {
                sb.Append(stm);
            }
            sb.Append("{/for}");
            return sb.ToString();
        }
    }
}
