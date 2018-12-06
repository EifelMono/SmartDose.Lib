namespace Rowa.Lib.Wcf.Setup
{
    /// <summary>
    /// Enumeration of pre-defined user accounts that can host a WCF service.
    /// </summary>
    public enum WcfServiceUser
    {
        /// <summary>
        /// The built-in network service user of Windows.
        /// </summary>
        NetworkService,

        /// <summary>
        /// The built-in everyone user of Windows.
        /// </summary>
        Everyone
    }
}
