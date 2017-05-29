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

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SolidCP.Templates.AST
{
    internal class CallTemplateStatement : Statement
    {
        public string templateName;
        public Dictionary<string, Expression> parameters = new Dictionary<string, Expression>();
        List<Statement> statements = new List<Statement>();

        public CallTemplateStatement(int line, int column)
            : base(line, column)
        {
        }

        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        public Dictionary<string, Expression> Parameters
        {
            get { return parameters; }
        }

        public List<Statement> Statements
        {
            get { return statements; }
        }

        public override void Eval(TemplateContext context, System.IO.StringWriter writer)
        {
            // locate template
            if (!context.Templates.ContainsKey(templateName))
                throw new ParserException(String.Format("Custom template \"{0}\" is not defined", templateName), Line, Column);

            TemplateStatement tmp = context.Templates[templateName];

            // create template-specific context
            TemplateContext tmpContext = new TemplateContext();
            tmpContext.ParentContext = context;
            tmpContext.Templates = context.Templates;

            // evaluate inner statements
            StringWriter innerWriter = new StringWriter();
            foreach (Statement stm in Statements)
                stm.Eval(context, innerWriter);
            tmpContext.Variables["innerText"] = innerWriter.ToString();

            // set context variables
            foreach (string name in parameters.Keys)
                tmpContext.Variables[name] = parameters[name].Eval(context);

            // evaluate template statements
            foreach (Statement stm in tmp.Statements)
                stm.Eval(tmpContext, writer);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{").Append(templateName);

            foreach (string name in parameters.Keys)
                sb.Append(" ").Append(name).Append("=\"").Append(parameters[name].ToString()).Append("\"");

            sb.Append(" /}");
            return sb.ToString();
        }
    }
}
