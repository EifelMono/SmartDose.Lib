namespace SmartDose.WcfLib
{
    public static class ServiceResultStatus
    {
        #region Ok, OK
        public static int Ok { get; set; } = 0;
        #endregion

        #region Error
        public static int Error { get; set; } = -1;

        public static int ErrorConnection { get; set; } = -2;

        public static int Exception { get; set; } = -3;

        public static int ErrorNotImplemented { get; set; } = -4;

        public static int ErrorInvalidateArgs { get; set; } = -10;
        #endregion
    }
}
