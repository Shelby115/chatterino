﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chatterino.Common;

namespace Chatterino.Controls
{
    public class ToolTip : Form
    {
        private string tooltip = null;

        public string TooltipText
        {
            get { return tooltip; }
            set
            {
                if (tooltip != value)
                {
                    tooltip = value;
                    redraw();
                }
            }
        }

        private LazyLoadedImage _image;

        public LazyLoadedImage Image
        {
            get { return _image; }
            set
            { 
                _image = value;
                redraw();
            }
        }

        void calcSize()
        {
            if (tooltip != null)
            {
                Image image = _image?.Image;
                var size = CreateGraphics().MeasureString(tooltip, Font, 1000, format);
                Size = new Size(Math.Max( Padding.Left + (image?.Width ?? 0) + Padding.Right, Padding.Left + (int)size.Width + Padding.Right), (image?.Height ?? -8) + 8 + Padding.Top + (int)size.Height + Padding.Bottom);
            }
        }
        
        public void redraw() {
            calcSize();
            Invalidate();
            Update();
        }
        
        public ToolTip()
        {
            Font = Fonts.GetFont(Common.FontType.Small);

            Fonts.FontChanged += (s, e) => Font = Fonts.GetFont(Common.FontType.Small);

            FormBorderStyle = FormBorderStyle.None;
            Opacity = 0.8;

            Padding = new Padding(8, 4, 8, 4);
            ShowInTaskbar = false;

            StartPosition = FormStartPosition.Manual;

            //SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //Win32.EnableWindowBlur(Handle);
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        private const int WS_EX_TOPMOST = 0x00000008;
        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.ExStyle |= WS_EX_TOPMOST;
                return createParams;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            TooltipText = TooltipText;

            base.OnFontChanged(e);
        }

        static StringFormat format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        //Color bgcolor = Color.FromArgb(127,64,64,64);

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    e.Graphics.Clear(bgcolor);

        //    //base.OnPaintBackground(e);
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.FillRectangle(App.ColorScheme.TooltipBackground, e.ClipRectangle);
            Image image = _image?.Image;

            if (image != null)
            {
                e.Graphics.DrawImage(image, 4, 4, image.Width, image.Height);
            }

            if (tooltip != null)
            {
                e.Graphics.DrawString(tooltip, Font, App.ColorScheme.TooltipText, new Rectangle(0, image?.Height ?? 0, Width, Height - (image?.Height ?? 0)), format);
            }
        }
    }
}
