namespace JsonSrcGen.PropertyHashing
{
    public class ColumnCollisions
    {
        readonly int _columnIndex;
        int _numberOfCollisions = 0;

        public ColumnCollisions(int columnIndex)
        {
            _columnIndex = columnIndex;
        }

        public int ColumnIndex => _columnIndex;

        public int NumberOfCollisions => _numberOfCollisions;

        public void AddCollision()
        {
            _numberOfCollisions++;
        }
    }
}