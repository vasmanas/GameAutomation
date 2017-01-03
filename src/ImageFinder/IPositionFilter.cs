using System;
using System.Drawing;

namespace ImageFinder
{
    interface IPositionFilter<TV> where TV : VisualObject
    {
        Rectangle[] Find(TV plain, TV fragment, Rectangle[] possibleOccurrences = null);
    }
}
