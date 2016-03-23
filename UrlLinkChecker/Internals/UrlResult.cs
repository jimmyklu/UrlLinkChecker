namespace UrlLinkChecker.Internals
{
    internal class UrlResult
    {
        internal static readonly string ResultOk = RscLiterals.Result_Ok;
        internal static readonly string ResultFail = RscLiterals.Result_Fail;
        private static readonly string Splitter = RscLiterals.Result_Splitter;

        public UrlResult(bool success, string err = null, int redirectCount = 0)
        {
            this.Success = success;
            this.Status = (success) ? ResultOk : ResultFail;
            this.Error = err;
            this.RedirectCount = redirectCount;
        }

        public UrlResult(string statusAndError)
        {
            string[] parts = statusAndError.Split(Splitter[0]);

            this.Status = parts != null ? parts[0] : string.Empty;
            this.Error = parts != null && parts.Length > 1 ? parts[1] : string.Empty;
        }

        public int RedirectCount { get; set; }
        public bool IsDuplicate { get; set; }
        public bool Success { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            return this.Status + Splitter + this.Error;
        }

        public UrlResult Clone()
        {
            return new UrlResult(this.Success, this.Error);
        }
    }
}
