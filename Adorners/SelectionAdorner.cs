using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace BeDesigner.Adorners
{
    internal class SelectionAdorner : Adorner
    {
        public SelectionAdorner(UIElement adornedEIeent) : base(adornedEIeent) { }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Rect adornerRect = new Rect(AdornedElement.DesiredSize);
            SolidColorBrush renderBrush = Brushes.Transparent;
            Pen render = new Pen(new SolidColorBrush(Colors.OrangeRed), 1);
            render.DashStyle = new DashStyle(new List<double>() { 4, 2 }, 2);
            drawingContext.DrawRectangle(renderBrush, render, new Rect(adornerRect.TopLeft.X, adornerRect.TopLeft.Y, adornerRect.Width, adornerRect.Height));

            MouseDown += SelectionAdorner_MouseDown;
            MouseMove += SelectionAdorner_MouseMove;
            MouseUp += SelectionAdorner_MouseUp;

            ContextMenu = FindResource("AdornerRightMenu") as ContextMenu;
            Tag = CanvasPanel.GetParentObject<CanvasPanel>(AdornedElement);

            this.Focus();
            this.SelectionAdorner_MouseDown(this, null);
            this.SelectionAdorner_MouseMove(this, null);
        }

        private void SelectionAdorner_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            CanvasPanel.GetParentObject<CanvasPanel>(AdornedElement).ClearAlignLine();
        }

        Point lastPoint = new Point();
        double tempX = 0d;
        double tempY = 0d;
        double movePx = 0d;
        private void SelectionAdorner_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (lastPoint.X == 0 && lastPoint.Y == 0)
                {
                    return;
                }

                CaptureMouse();
                var nowPoint = Mouse.GetPosition(CanvasPanel.GetParentObject<CanvasPanel>(AdornedElement));
                double offsetX = nowPoint.X - lastPoint.X;
                double offsetY = nowPoint.Y - lastPoint.Y;
                lastPoint = nowPoint;

                tempX += offsetX;
                tempY += offsetY;

                var canvas = CanvasPanel.GetParentObject<CanvasPanel>(AdornedElement);
                movePx = canvas.GridPxiel;

                if (Math.Abs(tempX) >= movePx)
                {
                    offsetX = Math.Round(tempX / movePx) * movePx;
                    tempX -= offsetX;
                    canvas.MoveControls(offsetX, 0);
                }

                if (Math.Abs(tempY) >= movePx)
                {
                    offsetY = Math.Round(tempY / movePx) * movePx;
                    tempY -= offsetY;
                    canvas.MoveControls(0, offsetY);
                }

            }
            else if (Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                CaptureMouse();
                var nowPoint = Mouse.GetPosition(CanvasPanel.GetParentObject<CanvasPanel>(AdornedElement));
                int offsetX = (int)(nowPoint.X - lastPoint.X);
                int offsetY = (int)(nowPoint.Y - lastPoint.Y);

                CanvasPanel.GetParentObject<CanvasPanel>(AdornedElement).ZoomControls(offsetX, offsetY);
                lastPoint = nowPoint;
            }
        }

        private void SelectionAdorner_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var canv = CanvasPanel.GetParentObject<CanvasPanel>(AdornedElement);
            lastPoint = Mouse.GetPosition(canv);
            Keyboard.Focus(canv);

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (canv.SelectedItems.Contains(AdornedElement))
                {
                    canv.SelectedItems.Remove(AdornedElement as FrameworkElement);
                    canv.RefreshSelection();
                }
            }
        }
    }
}
