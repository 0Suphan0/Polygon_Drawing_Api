namespace PolygonApi.Model
{
    public class Drawing
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }
}
