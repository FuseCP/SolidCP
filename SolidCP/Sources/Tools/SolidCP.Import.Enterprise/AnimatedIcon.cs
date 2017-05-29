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

namespace SolidCP.Import.Enterprise
{
	/// <summary>
	/// Animated Icon.
	/// </summary>
	[ToolboxItem(true)]
	public class AnimatedIcon : System.Windows.Forms.UserControl
	{
		private Thread thread = null;
		//private ImageList	imageList		= null;
		private List<Image> images;
		private int currentFrame = 0;
		private int delayInterval = 50;
		private int pause = 0;
		private int loopCount = 0;
		private int currentLoop = 0;
		private int firstFrame = 0;
		private int lastFrame = 0;

		private System.ComponentModel.Container components = null;

		
		/// <summary>Gets the <see cref="List"/> that contains 
		/// the <see cref="Image"/> objects used to display animation.</summary>
		/// <value>The <see cref="List"/> that contains the <see cref="Image"/> 
		/// objects used to display animated frames.</value>
		[Browsable(false)]
		public List<Image> Images
		{
			get
			{
				return images;
			}
		}

		/// <summary>Gets or sets delay in milliseconds between the frames.</summary>
		/// <value>The default value is 50 milliseconds.</value>
		[Category("Animated Icon")]
		[DefaultValue(50), Description("Delay in milliseconds between the frames.")]

		public int Delay
		{
			get { return delayInterval; }
			set 
			{
				if( value != delayInterval )
				{
					delayInterval = Math.Abs( value );
				}
			}
		}

		/// <summary>Gets or sets pause in milliseconds after every loop.</summary>
		/// <value>The default value is 0 milliseconds.</value>
		[Category("Animated Icon")]
		[DefaultValue(0), Description("Pause in milliseconds after the loop ends.")]

		public int Pause
		{
			get { return pause; }
			set 
			{
				if( value != pause )
				{
					pause = Math.Abs( value );
				}
			}
		}

		/// <summary>Gets or sets loop counter.</summary>
		/// <value>The default value is 0, i.e. infinitive loop.</value>
		[Category("Animated Icon")]
		[DefaultValue(0), Description("Loop counter. 0 means infinitive loop.")]

		public int LoopCount
		{
			get { return loopCount; }
			set 
			{
				if( value != loopCount )
				{
					loopCount = Math.Abs( value );
				}
			}
		}

		/// <summary>Gets or sets the first frame of the animation.</summary>
		/// <value>The default value is 0.</value>
		[Category("Animated Icon")]
		[DefaultValue(0), Description("First frame of the animation.")]

		public int FirstFrame
		{
			get { return firstFrame; }
			set 
			{
				if( value != firstFrame )
				{
					firstFrame = Math.Abs( value );
				}
			}
		}

		/// <summary>Gets or sets the last frame of the animation.</summary>
		/// <value>The default value is 0.</value>
		[Category("Animated Icon")]
		[DefaultValue(0), Description("Last frame of the animation.")]

		public int LastFrame
		{
			get { return lastFrame; }
			set 
			{
				if( value != lastFrame )
				{
					lastFrame = Math.Abs( value );
				}
			}
		}


		/// <summary>Initializes a new instance of the <b>AnimatedIcon</b> class.
		/// </summary>
		public AnimatedIcon()
		{
			CheckForIllegalCrossThreadCalls = false;
			InitializeComponent();
			images = new List<Image>();

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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();

				if( thread != null )
					thread.Abort();
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
		/// <summary>Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// AnimatedIcon
			// 
			this.Name = "AnimatedIcon";
			this.Size = new System.Drawing.Size(16, 16);
		}
		#endregion

		/// <summary>Starts animation from the beginning.
		/// </summary>
		public void StartAnimation()
		{
			StopAnimation();
			CheckRange();			// Check the first and the last frames

			thread = new Thread( new ThreadStart( threadFunc ) );
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>Stops animation not changing current frame number.
		/// </summary>
		public void StopAnimation()
		{
			if( thread != null )
			{
				thread.Abort();
				thread = null;
			}
			currentLoop = 0;
		}

		/// <summary>Displays the specified frame.</summary>
		/// <param name="frame">An index of the image stored in the <see cref="ImageList"/>.</param>
		public void ShowFrame(int frame)
		{
			StopAnimation();

			if( frame >= 0 && frame < images.Count )
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

			if( images == null ||
				currentFrame < 0 || 
				currentFrame >= images.Count )
			{
				if( this.Size.Width == 0 || this.Size.Height == 0 )
					return;

				Pen pen = new Pen( SystemColors.ControlText );
				e.Graphics.DrawRectangle( pen, 0, 0, this.Size.Width-1, this.Size.Height-1 );
				e.Graphics.DrawLine( pen, 0, 0, this.Size.Width, this.Size.Height );
				e.Graphics.DrawLine( pen, 0, this.Size.Height-1, this.Size.Width-1, 0 );
				pen.Dispose();
			}
			else
			{
				// Draw the current frame

				e.Graphics.DrawImage( images[currentFrame], 0, 0, this.Size.Width, this.Size.Height );
			}
		}

		/// <summary>The method to be invoked when the thread begins executing.
		/// </summary>
		private void threadFunc()
		{
			bool wasPause = false;
			currentFrame = firstFrame;

			while( thread != null && thread.IsAlive )
			{
				Refresh();						// Redraw the current frame
				wasPause = false;

				if( images != null )
				{
					currentFrame++;
					if( currentFrame > lastFrame ||
						currentFrame >= images.Count )
					{
						if( pause > 0 )			// Sleep after every loop
						{
							Thread.Sleep( pause );
							wasPause = true;
						}

						currentFrame = firstFrame;
						if( loopCount != 0 )	// 0 is infinitive loop
						{
							currentLoop++;
						}
					}

					if( loopCount != 0 && currentLoop >= loopCount )
					{
						StopAnimation();		// The loop is completed
					}
				}
				if( !wasPause )					// That prevents summation (pause + delayInterval)
					Thread.Sleep( delayInterval );
			}
		}

		/// <summary>Check if the last frame is no less than the first one. 
		/// Otherwise, swap them.</summary>
		private void CheckRange()
		{
			if( lastFrame < firstFrame )
			{
				int tmp = firstFrame;
				firstFrame = lastFrame;
				lastFrame = tmp;
			}
		}
	}
}
