namespace RowaLogFileBehaviorTester
{
    public class LogErrorInfo
    {
        private string _errorType;
        private int _count;

        public string ErrorType { get => _errorType; set => _errorType = value; }
        public int Count { get => _count; set => _count = value; }

        public override string ToString()
        {
            return $"{ErrorType} ({Count})";
        }
    }
}