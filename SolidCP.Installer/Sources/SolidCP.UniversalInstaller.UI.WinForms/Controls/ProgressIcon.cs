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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Threading;

namespace SolidCP.UniversalInstaller.Controls
{
	/// <summary>
	/// Animated Icon.
	/// </summary>
	[ToolboxItem(true)]
	public class ProgressIcon : System.Windows.Forms.UserControl
	{
		private Thread thread = null;
		private bool threadAbort = false;
		private int currentFrame = 0;
		private int delayInterval = 50;
		private int pause = 0;
		private int loopCount = 0;
		private int currentLoop = 0;
		private int firstFrame = 0;
		private int lastFrame = 13;
		private ImageList images;
		private IContainer components;

		/// <summary>Initializes a new instance of the <b>AnimatedIcon</b> class.
		/// </summary>
		public ProgressIcon()
		{
			CheckForIllegalCrossThreadCalls = false;
			InitializeComponent();

			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
		}

		#region Dispose
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing"><see langword="true"/> to release both managed 
		/// and unmanaged resources; <see langword="false"/> to release 
		/// only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();

				if (thread != null)
				{
					//thread.Abort();
					threadAbort = true;
				}
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Component Designer generated code
		/// <summary>Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressIcon));
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// images
			// 
			this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			this.images.Images.SetKeyName(0, "ProgressImage00.bmp");
			this.images.Images.SetKeyName(1, "ProgressImage01.bmp");
			this.images.Images.SetKeyName(2, "ProgressImage02.bmp");
			this.images.Images.SetKeyName(3, "ProgressImage03.bmp");
			this.images.Images.SetKeyName(4, "ProgressImage04.bmp");
			this.images.Images.SetKeyName(5, "ProgressImage05.bmp");
			this.images.Images.SetKeyName(6, "ProgressImage06.bmp");
			this.images.Images.SetKeyName(7, "ProgressImage07.bmp");
			this.images.Images.SetKeyName(8, "ProgressImage08.bmp");
			this.images.Images.SetKeyName(9, "ProgressImage09.bmp");
			this.images.Images.SetKeyName(10, "ProgressImage10.bmp");
			this.images.Images.SetKeyName(11, "ProgressImage11.bmp");
			this.images.Images.SetKeyName(12, "ProgressImage12.bmp");
			// 
			// ProgressIcon
			// 
			this.Name = "ProgressIcon";
			this.Size = new System.Drawing.Size(30, 30);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>Starts animation from the beginning.
		/// </summary>
		public void StartAnimation()
		{
			StopAnimation();
			CheckRange();           // Check the first and the last frames

			thread = new Thread(new ThreadStart(threadFunc));
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>Stops animation not changing current frame number.
		/// </summary>
		public void StopAnimation()
		{
			if (thread != null)
			{
				//thread.Abort();
				threadAbort = true;
				thread = null;
			}
			currentLoop = 0;
		}

		/// <summary>Displays the specified frame.</summary>
		/// <param name="frame">An index of the image stored in the <see cref="ImageList"/>.</param>
		public void ShowFrame2(int frame)
		{
			StopAnimation();

			if (frame >= 0 && frame < images.Images.Count)
				currentFrame = frame;
			else
				currentFrame = 0;

			Refresh();
		}

		/// <summary>Occurs when the control is redrawn.</summary>
		/// <param name="e">A <see cref="PaintEventArgs"/> that contains 
		/// the event data.</param>
		/// <remarks>The <b>OnPaint</b> method draws current image from 
		/// the <see cref="ImageList"/> if exists.</remarks>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Draw a crossed rectangle if there is no frame to display

			if (images == null ||
				currentFrame < 0 ||
				currentFrame >= images.Images.Count)
			{
				if (this.Size.Width == 0 || this.Size.Height == 0)
					return;

				Pen pen = new Pen(SystemColors.ControlText);
				e.Graphics.DrawRectangle(pen, 0, 0, this.Size.Width - 1, this.Size.Height - 1);
				e.Graphics.DrawLine(pen, 0, 0, this.Size.Width, this.Size.Height);
				e.Graphics.DrawLine(pen, 0, this.Size.Height - 1, this.Size.Width - 1, 0);
				pen.Dispose();
			}
			else
			{
				// Draw the current frame

				e.Graphics.DrawImage(images.Images[currentFrame], 0, 0, this.Size.Width, this.Size.Height);
			}
		}

		/// <summary>The method to be invoked when the thread begins executing.
		/// </summary>
		private void threadFunc()
		{
			bool wasPause = false;
			currentFrame = firstFrame;

			while (thread != null && thread.IsAlive && !threadAbort)
			{
				Refresh();                      // Redraw the current frame
				wasPause = false;

				if (images != null)
				{
					currentFrame++;
					if (currentFrame > lastFrame ||
						currentFrame >= images.Images.Count)
					{
						if (pause > 0)          // Sleep after every loop
						{
							Thread.Sleep(pause);
							wasPause = true;
						}

						currentFrame = firstFrame;
						if (loopCount != 0) // 0 is infinitive loop
						{
							currentLoop++;
						}
					}

					if (loopCount != 0 && currentLoop >= loopCount)
					{
						StopAnimation();        // The loop is completed
					}
				}
				if (!wasPause)                  // That prevents summation (pause + delayInterval)
					Thread.Sleep(delayInterval);
			}
		}

		/// <summary>Check if the last frame is no less than the first one. 
		/// Otherwise, swap them.</summary>
		private void CheckRange()
		{
			if (lastFrame < firstFrame)
			{
				int tmp = firstFrame;
				firstFrame = lastFrame;
				lastFrame = tmp;
			}
		}
	}
}
