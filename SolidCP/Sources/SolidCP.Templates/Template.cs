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
using System.IO;
using System.Collections.Generic;
using System.Text;
using SolidCP.Templates.AST;
using System.Collections;

namespace SolidCP.Templates
{
    /// <summary>
    /// Template allows to process block of texts containing special directives such as variables,
    /// conditional statements and loops.
    /// </summary>
    public class Template
    {
        string data;
        Lexer lexer = null;
        Parser parser = null;
        List<AST.Statement> statements = null;
        TemplateContext context = new TemplateContext();

        /// <summary>
        /// Initializes a new instance of Template with specified template text.
        /// </summary>
        /// <param name="data">Template text.</param>
        public Template(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.data = data;
        }

        /// <summary>
        /// Initializes a new instance of Template with specified StringReader containing template text.
        /// </summary>
        /// <param name="reader">StringReader containing template text.</param>
        public Template(StringReader reader)
        {
            if(reader == null)
                throw new ArgumentNullException("reader");

            this.data = reader.ReadToEnd();
        }

        /// <summary>
        /// Verifies template syntax and throws <see cref="ParserException">ParserException</see> when error is found.
        /// </summary>
        /// <exception cref="ParserException">Thrown when error is found.</exception>
        public void CheckSyntax()
        {
            // create lexer
            lexer = new Lexer(data);

            // create parser
            parser = new Parser(lexer);

            // parse template
            parser.Parse();
        }

        /// <summary>
        /// Evaluates template and returns the result as a string.
        /// </summary>
        /// <returns>String containing evaluated template.</returns>
        public string Evaluate()
        {
            // create writer to hold result
            StringWriter writer = new StringWriter();

            // evaluate
            Evaluate(writer);

            // return result
            return writer.ToString();
        }

        /// <summary>
		/// Evaluates template and returns the result as a string.
		/// </summary>
		/// <returns>String containing evaluated template.</returns>
		public string Evaluate(Hashtable items)
		{
			// copy items from hashtable
			foreach (string keyName in items.Keys)
			{
				this[keyName] = items[keyName];
			}

			// evaluate
			return Evaluate();
		}

		/// <summary>
		/// Evaluates template to the StringWriter.
        /// </summary>
        /// <param name="writer">StringWriter to write evaluation results.</param>
        public void Evaluate(StringWriter writer)
        {
            if(writer == null)
                throw new ArgumentNullException("writer");

            if (lexer == null)
            {
                // create lexer
                lexer = new Lexer(data);

                // create parser
                parser = new Parser(lexer);

                // parse template
                statements = parser.Parse();
            }

            // index custom templates
            int i = 0;
            while (i < statements.Count)
            {
                TemplateStatement tmpStatement = statements[i] as TemplateStatement;
                if (tmpStatement != null)
                {
                    context.Templates.Add(tmpStatement.Name, tmpStatement);
                    statements.RemoveAt(i);
                    continue;
                }
                i++;
            }

            // evaluate template statements
            foreach (AST.Statement stm in statements)
            {
                // eval next statement
                stm.Eval(context, writer);
            }
        }

        /// <summary>
        /// Gets or sets the value of template context variable.
        /// </summary>
        /// <param name="name">The name of the context variable. Variable names are case-insensitive.</param>
        /// <returns>Returns the value of the context variable.</returns>
        public object this[string name]
        {
            get { return context.Variables.ContainsKey(name) ? context.Variables[name] : null; }
            set { context.Variables[name] = value; }
        }
    }
}
