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
        public Object MoveObject { get; set; }
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
            MouseRightButtonDown += new MouseButtonEventHandler(MyVisualHost_MouseRightButtonDown);
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
        /// Callback
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public HitTestResultBehavior Callback(HitTestResult result)
        {
            MoveObject = result.VisualHit;
            return HitTestResultBehavior.Stop;
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

        /// <summary>
        /// MyVisualHost_MouseRightButtonDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyVisualHost_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MoveObject = null;
            Point pt = e.GetPosition((UIElement)sender);
            VisualTreeHelper.HitTest(this, null, new HitTestResultCallback(Callback), new PointHitTestParameters(pt));
        }

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// AddDot
        /// </summary>
        /// <param name="p"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public void AddDot(Point p,double size, Brush color)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawEllipse(color, null, p, size, size);
            drawingContext.Close();
            children.Add(drawingVisual);
        }

        /// <summary>
        /// MoveDot
        /// </summary>
        /// <param name="p"></param>
        public void MoveDot(Point p, double size, Brush color)
        {
            if (MoveObject == null) return;
            DrawingContext dc = ((DrawingVisual)MoveObject).RenderOpen();
            dc.DrawEllipse(color, null, p, size, size);
            dc.Close();
        }

        /// <summary>
        /// Clear;
        /// </summary>
        public void Clear()
        {
            children.Clear();
        }

        #endregion
    }
}
