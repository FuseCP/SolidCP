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
using System.Diagnostics;

using SolidCP.Templates.AST;

namespace SolidCP.Templates
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = LoadTestTemplate("cf3.txt");

            //TestLexer(data);
            TestTemplates(data);
        }

        private static void TestTemplates(string data)
        {
            Template tmp1 = new Template(data);
            tmp1["a"] = 2;
            tmp1["b"] = 5;
            tmp1["d"] = 2;
            tmp1["str"] = "string 111";

            tmp1["str1"] = "Hello, World";
            tmp1["str2"] = "";
            tmp1["str3"] = null;

            tmp1["arr"] = new string[] { "s1", "s2", "s3" };
            tmp1["customer"] = new Customer()
            {
                OrderNumbers = new string[] { "order-1", "order-2", "order-3" }
            };
            tmp1["integersDict"] = new Dictionary<int, string>()
            {
                {101, "str1"},
                {102, "str2"},
                {103, "str3"}
            };

            tmp1["customersDict"] = new Dictionary<string, Customer>()
            {
                {"customer1", new Customer()
                    {
                        Name = "John Smith",
                        OrderNumbers = new string[] { "o-1", "o-2" }
                    } },
                {"customer2", new Customer()
                    {
                        Name = "Jack Brown",
                        OrderNumbers = new string[] { "order-1", "order-2", "order-3", "order-4" }
                    }}
            };

            tmp1["customersList"] = new List<Customer>()
            {
                new Customer()
                    {
                        Name = "John Smith",
                        OrderNumbers = new string[] { "o-1", "o-2" }
                    },
                new Customer()
                    {
                        Name = "Jack Brown",
                        OrderNumbers = new string[] { "o-1", "o-2" }
                    }
            };


            // check syntax
            //tmp1.CheckSyntax();

            string result = tmp1.Evaluate();

            Console.Write(result);

            Console.ReadKey();
        }

        private static void TestLexer(string data)
        {
            List<Token> tokens = new List<Token>();
            Lexer lex = new Lexer(data);

            while (true)
            {
                Token token = lex.Next();
                tokens.Add(token);

                Debug.WriteLine(String.Format("{0} [{1},{2}]: {3}", token.TokenType, token.Line, token.Column, token.Data));

                if (token.TokenType == TokenType.EOF)
                    break;
            }

            Console.ReadKey();
        }

        private static string LoadTestTemplate(string name)
        {
            return File.ReadAllText("../Tests/" + name);
        }
    }

    internal class Customer
    {
        public string Name { get; set; }
        public string[] OrderNumbers { get; set; }
    }
}
