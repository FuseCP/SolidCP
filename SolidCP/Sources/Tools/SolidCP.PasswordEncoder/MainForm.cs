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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolidCP.PasswordEncoder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            if(CryptoKeyEntered() && ValueEntered())
                Result.Text = CryptoUtils.Encrypt(Value.Text.Trim(), CryptoKey.Text.Trim());
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            if (CryptoKeyEntered() && ValueEntered())
                Result.Text = CryptoUtils.Decrypt(Value.Text.Trim(), CryptoKey.Text.Trim());
        }

        private void Sha1Button_Click(object sender, EventArgs e)
        {
            if (ValueEntered())
                Result.Text = CryptoUtils.SHA1(Value.Text.Trim());
        }

        private bool CryptoKeyEntered()
        {
            if (CryptoKey.Text.Trim() == "")
            {
                MessageBox.Show("Enter Crypto Key");
                CryptoKey.Focus();
                return false;
            }
            return true;
        }

        private bool ValueEntered()
        {
            if (Value.Text.Trim() == "")
            {
                MessageBox.Show("Enter Value");
                Value.Focus();
                return false;
            }
            return true;
        }
    }
}
