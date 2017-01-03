using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace ImageFinder.SimilarityChecks
{
    public partial class ColorSum : IEnumerable<KeyValuePair<Color, int>>
    {
        private Dictionary<Color, int> values = new Dictionary<Color, int>();

        public int Get(Color color)
        {
            if (this.values.ContainsKey(color))
            {
                return this.values[color];
            }

            return 0;
        }

        public void Add(Color color, int count = 1)
        {
            if (color == Color.Empty)
            {
                throw new ArgumentNullException("color");
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "Must be greather than zero");
            }

            if (this.values.ContainsKey(color))
            {
                this.values[color] += count;
            }
            else
            {
                this.values.Add(color, count);
            }
        }

        public void Remove(Color color)
        {
            if (color == Color.Empty)
            {
                return;
            }

            this.values.Remove(color);
        }

        public int Total()
        {
            int result = 0;
            foreach (var col in this.values)
            {
                result += col.Value;
            }

            return result;
        }

        public IEnumerator<KeyValuePair<Color, int>> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
    }
}
