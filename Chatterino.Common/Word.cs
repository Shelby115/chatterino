﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatterino.Common
{
    public class Word
    {
        public SpanType Type { get; set; }
        public object Value { get; set; }
        public HSLColor? Color { get; set; }
        public Link Link { get; set; }
        public string Tooltip { get; set; }
        public string TooltipImageUrl { get; set; }
        public LazyLoadedImage TooltipImage { get; set; }
        public string CopyText { get; set; } = null;
        public bool Highlighted { get; set; } = false;

        public FontType Font { get; set; }
        public int Height { get; set; } = 16;
        public int Width { get; set; } = 16;
        public int X { get; set; }
        public int Y { get; set; }

        public bool HasTrailingSpace { get; set; } = true;

        public Tuple<string, CommonRectangle>[] SplitSegments { get; set; } = null;
        public int[] CharacterWidths { get; set; } = null;

        public bool Intersects(Word word)
        {
            if (X < word.X)
            {
                if (word.X < X + Width)
                {
                    if (Y < word.Y)
                    {
                        if (word.Y < Y + Height)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Y < word.Y + word.Height)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (X < word.X + word.Width)
                {
                    if (Y < word.Y)
                    {
                        if (word.Y < Y + Height)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Y < word.Y + word.Height)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

    public enum SpanType
    {
        Text,
        LazyLoadedImage
    }
}
