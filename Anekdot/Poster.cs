using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace AnekdotApp
{
    public class Poster
    {
        private string _text;
        private string _pathToBackground;
        private readonly int _dpi = 96;
        private readonly int _gap = 20;
        private RenderTargetBitmap _bitmap;
        private int _minFontSize = 18;
        private int _maxFontSize = 50;
        private int _minCharCount = 1;
        private int _maxCharCount = 500;
        private BitmapFrame _image;
        private bool RemoveNewLine { get; set; }
        private bool AutoSize { get; set; }
        private int FontSize { get; set; }
        private Brush PosterBrush { get; set; }
        private Typeface PosterFont { get; set; }

        public Poster()
        {            
            RemoveNewLine = true;
            AutoSize = true;
            FontSize = 25;
            PosterFont = new Typeface(new FontFamily("Sweet Mavka Script"),
                                      FontStyles.Normal,
                                      FontWeights.Bold,
                                      FontStretches.SemiCondensed);
            PosterBrush = Brushes.Black;
        }
        //  Перегрузка для указания своего шрифта и цвета
        public Poster(Typeface font, int fontSize, Brush color, bool autoSize, bool removeNewLine)
        {
            PosterFont = font;
            PosterBrush = color;
            FontSize = fontSize;
            AutoSize = autoSize;
            RemoveNewLine = removeNewLine;
        }
        public void Create(string text, string path)
        {
            _text = text;
            _pathToBackground = path;
            //  Если длинна текста > 500 символов, то пропускаем его,
            //  потому шо будет слишком мелкий шрифт
            if (_text.Length > 500)
                return;
            //  Для удаления лишних переносов строк
            if(RemoveNewLine)
                RemoveExtraNewLine();
            if (AutoSize)
                SetGrateFontSize();

            _image = BitmapFrame.Create(new Uri(_pathToBackground));
            //  Настройка форамтирования текста
            var formattedText =
                new FormattedText(_text, System.Globalization.CultureInfo.CurrentCulture,
                                  FlowDirection.LeftToRight, PosterFont, FontSize, PosterBrush, _dpi)
                {
                    MaxTextWidth = _image.Width - _gap * 2,
                    TextAlignment = TextAlignment.Center,
                    MaxTextHeight = _image.PixelHeight
            };
            var drawingVisual = new DrawingVisual();

            using(var drawingContext = drawingVisual.RenderOpen())
            {
                //  Создаем прямоугольник с фоном и помещаем в него текст
                drawingContext.DrawImage(_image,
                                        new Rect(0, 0, _image.Width, _image.Height));
                drawingContext.DrawText(formattedText, new Point(_gap, _gap));
            }

            _bitmap = new RenderTargetBitmap(_image.PixelWidth, _image.PixelHeight,
                                                _dpi, _dpi, PixelFormats.Pbgra32);
            _bitmap.Render(drawingVisual);
        }

        public void SaveRender(string pathToSaveRender)
        {
            if (_text.Length > 500)
                return;
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_bitmap));
            using (var stream = File.Create(pathToSaveRender))
            {
                encoder.Save(stream);
            }
        }
        private void SetGrateFontSize()
        {   
            //  С помощью линейной интерполяции получаем значение размера шрифта
            //  относительно количества текста
            int currentCountChar = _text.Length;
            int interpolatedSize = ((currentCountChar - _maxCharCount) * (_maxFontSize - _minFontSize)
                / (_minCharCount - _maxCharCount)) + _minFontSize;
            FontSize = interpolatedSize;
        }

        private void RemoveExtraNewLine()
        {
            _text = _text.Replace(Environment.NewLine, " ");
            _text.Trim();
        }
    }
}
