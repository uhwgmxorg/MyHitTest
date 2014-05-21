using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MyHitTest
{
    public class MyVisualHost : FrameworkElement
    {
        public delegate void HitEventHandler();
        public event HitEventHandler Hit;

        private VisualCollection children;
        public VisualCollection Children {get;set;}
        protected override int VisualChildrenCount
        {
            get { return children.Count; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyVisualHost()
        {
            children = new VisualCollection(this);

            MouseLeftButtonDown += new MouseButtonEventHandler(MyVisualHost_MouseLeftButtonDown);
        }

        /******************************/
        /*           Events           */
        /******************************/
        #region Events

        /// <summary>
        /// GetVisualChild
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return children[index];
        }

        /// <summary>
        /// MyVisualHost_MouseLeftButtonUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyVisualHost_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Hit != null)
                Hit();
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        public void AddDot(Point p,double size, Brush color)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawEllipse(color, null, p, size, size);
            drawingContext.Close();
            children.Add(drawingVisual);
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            children.Clear();
        }

        #endregion
    }
}
