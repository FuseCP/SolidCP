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

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SolidCP.Import.Enterprise
{
	/// <summary>
	/// 3D line box.
	/// </summary>
	[ToolboxItem(true)]
	public partial class LineBox : Control
	{
		/// <summary>
		/// Initializes a new instance of the LineBox class.
		/// </summary>
		public LineBox() : base()
		{
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.FixedHeight, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.StandardClick, false);
			SetStyle(ControlStyles.Selectable, false);
			this.TabStop = false;
		}
		
		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			Rectangle rectangle = this.ClientRectangle;
			Pen lightPen = new Pen(ControlPaint.Light(this.BackColor, 1));
			Pen darkPen = new Pen(ControlPaint.Dark(this.BackColor, 0));
			graphics.DrawLine(darkPen, rectangle.X, rectangle.Y, rectangle.X+rectangle.Width, rectangle.Y);
			graphics.DrawLine(lightPen, rectangle.X, rectangle.Y+1, rectangle.X+rectangle.Width, rectangle.Y+1);
			base.OnPaint(e);
		}

		/// <summary>
		/// Gets the default size of the control.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(10, 2);
			}
		}
 
		
		/// <summary>
		/// Performs the work of setting the specified bounds of this control.
		/// </summary>
		/// <param name="x">The new Left property value of the control.</param>
		/// <param name="y">The new Right property value of the control.</param>
		/// <param name="width">The new Width property value of the control.</param>
		/// <param name="height">The new Height property value of the control.</param>
		/// <param name="specified">A bitwise combination of the BoundsSpecified values.</param>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, 2, specified);
		} 
	}
}
