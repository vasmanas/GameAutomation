namespace ImageFinder.SimilarityChecks
{
    public partial class ColorSum
    {
        public static int Match(ColorSum cs1, ColorSum cs2)
        {
            int result = 0;

            foreach (var col in cs1)
            {
                var sum2 = cs2.Get(col.Key);

                if (col.Value < sum2)
                {
                    result += col.Value;
                }
                else
                {
                    result += sum2;
                }
            }

            return result;
        }
    }
}
