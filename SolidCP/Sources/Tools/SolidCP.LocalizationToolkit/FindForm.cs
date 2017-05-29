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
using System.Text;
using System.Windows.Forms;

namespace SolidCP.LocalizationToolkit
{
	public partial class FindForm : Form
	{
		private DataGridView grid;
		
		public FindForm(DataGridView grid)
		{
			InitializeComponent();
			this.grid = grid;
			txtFind.Focus();
			cbSearch.SelectedIndex = 0;
			InvalidateForm();
		}

		private void OnCloseClick(object sender, EventArgs e)
		{
			//this.Visible = false;
			this.Hide();
		}

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				FindNext();
			}
		}

		private void OnFindNext(object sender, EventArgs e)
		{
			FindNext();
		}

		private void FindNext()
		{
			string text = txtFind.Text;
			if (text.Trim().Length > 0)
			{
				if (grid.CurrentCell == null)
				{
					grid.CurrentCell = grid[2, 0];
				}
				DataGridViewCell cell = grid.CurrentCell;
				DataGridViewCell startCell = cell;

				StringComparison comparison = (chkMatchCase.Checked) ? StringComparison.InvariantCulture :
					StringComparison.InvariantCultureIgnoreCase;

				string value = null;
				while (true)
				{
					cell = GetNextCell(cell);
					if (cell.RowIndex == startCell.RowIndex &&
						cell.ColumnIndex == startCell.ColumnIndex)
					{
						MessageBox.Show("Text not found.", "Localization Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						break;
					}
					if (cell.FormattedValue == null)
						continue;

					value = cell.FormattedValue.ToString();
					if (chkMatchWord.Checked)
					{
						if (string.Equals(value, text, comparison))
						{
							grid.CurrentCell = cell;
							break;
						}
					}
					else
					{
						if (chkMatchCase.Checked)
						{
							if (value.Contains(text))
							{
								grid.CurrentCell = cell;
								break;
							}
						}
						else
						{
							if (value.ToLower().Contains(text.ToLower()))
							{
								grid.CurrentCell = cell;
								break;
							}
						}
					}
				}
			}
		}

		private DataGridViewCell GetNextCell(DataGridViewCell cell)
		{
			int maxRow = grid.Rows.Count;
			int maxColumn = grid.Columns.Count;

			int rowIndex = cell.RowIndex;
			int columnIndex = cell.ColumnIndex;

			if (cbSearch.SelectedIndex == 0)
			{
				//find by rows
				rowIndex++;
			}
			else
			{
				//find by columns
				columnIndex++; 
			}

			if (columnIndex >= maxColumn)
			{
				columnIndex = 0;
				rowIndex++;
			}

			if (rowIndex >= maxRow)
				rowIndex = 0;

			DataGridViewCell ret = grid[columnIndex, rowIndex];
			return ret;
		}


		private void InvalidateForm()
		{
			bool gridPopulated = IsGridPopulated();
			bool textPopulated = (txtFind.Text.Trim().Length > 0);
			btnFindNext.Enabled = textPopulated && gridPopulated;
		}

		private bool IsGridPopulated()
		{
			bool ret = true;
			if (grid == null || grid.Rows.Count == 0)
				ret = false;
			return ret;
		}

		private void OnSearchTextChanged(object sender, EventArgs e)
		{
			InvalidateForm();
		}

		private void OnFormVisibleChanged(object sender, EventArgs e)
		{
			InvalidateForm();
		}
	}
}
