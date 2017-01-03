using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using InputDevicesSimulator.Actions;
using InputDevicesSimulator.Common;
using InputDevicesSimulator.Simulation;

namespace Visualizer.Utils
{
    public class VisualPlayer : ISignalChannelInput
    {
        private int mousePosX = -1;
        private int mousePosY = -1;

        private int leftDownX = -1;
        private int leftDownY = -1;

        private int rightDownX = -1;
        private int rightDownY = -1;

        private readonly ControlLense window;

        public VisualPlayer()
        {
            this.window = new ControlLense();
        }

        // TODO: Stop should hide window
        public void Send(IEnumerable<InputAction> actions)
        {
            if (actions == null)
            {
                return;
            }

            this.window.Show();

            foreach (dynamic action in actions)
            {
                this.Execute(action);
            }
        }

        private void Execute(MouseKeyDown action)
        {
            switch (action.Button)
            {
                case MouseButton.Left:
                    {
                        this.PlaceCircle(this.mousePosX, this.mousePosY, Brushes.LightPink);

                        this.leftDownX = this.mousePosX;
                        this.leftDownY = this.mousePosY;

                        break;
                    }

                case MouseButton.Right:
                    {
                        this.PlaceCircle(this.mousePosX, this.mousePosY, Brushes.LightBlue);

                        this.rightDownX = this.mousePosX;
                        this.rightDownY = this.mousePosY;

                        break;
                    }
            }
        }

        private void Execute(MouseKeyUp action)
        {
            switch (action.Button)
            {
                case MouseButton.Left:
                    {
                        this.PlaceCircle(this.mousePosX, this.mousePosY, Brushes.DarkRed);

                        this.PlaceLine(this.leftDownX, this.leftDownY, this.mousePosX, this.mousePosY, Brushes.Green);

                        break;
                    }

                case MouseButton.Right:
                    {
                        this.PlaceCircle(this.mousePosX, this.mousePosY, Brushes.DarkBlue);

                        this.PlaceLine(this.rightDownX, this.rightDownY, this.mousePosX, this.mousePosY, Brushes.Yellow);

                        break;
                    }
            }
        }

        private void Execute(MouseMoveTo action)
        {
            if (this.mousePosX != -1)
            {
                this.PlaceLine(this.mousePosX, this.mousePosY, action.X, action.Y, Brushes.LightGray);
            }

            this.mousePosX = action.X;
            this.mousePosY = action.Y;
        }

        private void Execute(KeyDown action)
        {
            Debug.WriteLine("KeyDown: code - {0}", action.KeyCode);
        }

        private void Execute(KeyUp action)
        {
            Debug.WriteLine("KeyUp: code - {0}", action.KeyCode);
        }

        private void Execute(WaitFor action)
        {
            Debug.WriteLine("WaitFor: miliseconds - {0}", action.Miliseconds);
        }

        private void Execute(CompositeInputAction action)
        {
            var actions = action.Translate();

            if (actions == null)
            {
                return;
            }

            foreach (dynamic act in actions)
            {
                this.Execute(act);
            }
        }

        private void Execute(InputAction action)
        {
            Debug.WriteLine(string.Format("Generic action: type: {0}", action.GetType().Name));
        }

        private void PlaceLine(int xFrom, int yFrom, int xTo, int yTo, Brush stroke)
        {
            var line = new Line();
            line.Stroke = stroke;
            line.StrokeThickness = 1;
            line.Opacity = 0.3;

            this.window.Board.Children.Add(line);

            var fp = this.window.Board.PointFromScreen(new Point(xFrom, yFrom));
            var tp = this.window.Board.PointFromScreen(new Point(xTo, yTo));

            line.X1 = fp.X;
            line.Y1 = fp.Y;
            line.X2 = tp.X;
            line.Y2 = tp.Y;
        }

        private void PlaceCircle(int x, int y, Brush fill)
        {
            //var a = System.Windows.SystemParameters.PrimaryScreenWidth;
            //var b = System.Windows.SystemParameters.PrimaryScreenHeight;

            var shape = new Ellipse();
            shape.Width = 7;
            shape.Height = 7;
            shape.Fill = fill;
            shape.Opacity = 0.3;

            this.window.Board.Children.Add(shape);

            var cp = this.window.Board.PointFromScreen(new Point(x - 4, y - 4));

            Canvas.SetLeft(shape, cp.X);
            Canvas.SetTop(shape, cp.Y);

            //var mouse = new MouseSimulator();
            //mouse.MoveMouseTo(x, y);
        }
    }
}
