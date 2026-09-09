namespace SomethingDownThere
{
    // Published only after the worker completes; diagnostic data never enters a save.
    public sealed class SaveWriteMetrics
    {
        public double EncodeMilliseconds { get; internal set; }
        public double WriteMilliseconds { get; internal set; }
        public double FlushMilliseconds { get; internal set; }
        public double ReplaceMilliseconds { get; internal set; }
        public long EncodedBytes { get; internal set; }
    }
}
