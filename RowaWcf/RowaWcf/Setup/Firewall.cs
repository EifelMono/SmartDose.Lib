using Rowa.Lib.Wcf.Logging;
using Rowa.Lib.Wcf.Native;
using System;
using System.Diagnostics;
using System.IO;

namespace Rowa.Lib.Wcf.Setup
{
    /// <summary>
    /// Class which provides system firewall configuration logic to enable
    /// non administrative users to self-host HTTP(S) based WCF services.
    /// </summary>
    public static class Firewall
    {
        #region Constants

        /// <summary>
        /// Command line of the windows firewall configuration utility.
        /// </summary>
        private const string CmdConfigureFirewall = "netsh.exe";

        /// <summary>
        /// Command to enable WCF self hosting for a specific port and a specific user.
        /// </summary>
        private const string CmdLineAddWcfFirewallRule = "http add urlacl url=http://+:{0}/ user=\"{1}\"";

        /// <summary>
        /// Command to enable WCF self hosting for a specific port and a specific user.
        /// </summary>
        private const string CmdLineAddWcfUrlFirewallRule = "http add urlacl url={0} user=\"{1}\"";

        /// <summary>
        /// Command to disable WCF self hosting for a specific port.
        /// </summary>
        private const string CmdLineDeleteWcfFirewallRule = "http delete urlacl url=http://+:{0}/";

        /// <summary>
        /// Command to disable WCF self hosting for a specific port.
        /// </summary>
        private const string CmdLineDeleteWcfUrlFirewallRule = "http delete urlacl url={0}";


        #endregion

        #region Methods

        /// <summary>
        /// Adds a firewall access rule for the specified WCF port and service user.
        /// </summary>
        /// <param name="wcfPort">The WCF port to add the firewall rule for.</param>
        /// <param name="wcfServiceUser">The WCF service user to add the firewall rule for.</param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        public static bool AddRule(ushort wcfPort, WcfServiceUser wcfServiceUser)
        {
            var userName = string.Empty;

            switch (wcfServiceUser)
            {
                case WcfServiceUser.NetworkService:
                    userName = Advapi32.GetNetworkServiceName();
                    break;

                case WcfServiceUser.Everyone:
                    userName = Advapi32.GetEveryOneName();
                    break;

                default:
                    throw new ArgumentException(nameof(wcfServiceUser));                    
            }

            return AddRule(wcfPort, userName);
        }

        /// <summary>
        /// Adds a firewall access rule for the specified WCF url and service user.
        /// </summary>
        /// <param name="wcfUrl">The WCF url to add the firewall rule for.</param>
        /// <param name="wcfServiceUser">The WCF service user to add the firewall rule for.</param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        public static bool AddRule(string wcfUrl, WcfServiceUser wcfServiceUser)
        {
            var userName = string.Empty;

            switch (wcfServiceUser)
            {
                case WcfServiceUser.NetworkService:
                    userName = Advapi32.GetNetworkServiceName();
                    break;

                case WcfServiceUser.Everyone:
                    userName = Advapi32.GetEveryOneName();
                    break;

                default:
                    throw new ArgumentException(nameof(wcfServiceUser));
            }

            return AddRule(wcfUrl, userName);
        }

        /// <summary>
        /// Adds a firewall access rule for the specified WCF port and service user.
        /// </summary>
        /// <param name="wcfPort">The WCF port to add the firewall rule for.</param>
        /// <param name="wcfServiceUserName">The WCF service user to add the firewall rule for.</param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        public static bool AddRule(ushort wcfPort, string wcfServiceUserName)
        {
            if (wcfPort == 0)
            {
                throw new ArgumentException(nameof(wcfPort));
            }

            if (string.IsNullOrEmpty(wcfServiceUserName))
            {
                throw new ArgumentException(nameof(wcfServiceUserName));
            }

            RunFirewallConfiguration(string.Format(CmdLineDeleteWcfFirewallRule, wcfPort));
            return RunFirewallConfiguration(string.Format(CmdLineAddWcfFirewallRule, wcfPort, wcfServiceUserName));
        }

        /// <summary>
        /// Adds a firewall access rule for the specified WCF url and service user.
        /// </summary>
        /// <param name="wcfUrl">The WCF url to add the firewall rule for.</param>
        /// <param name="wcfServiceUserName">The WCF service user to add the firewall rule for.</param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        public static bool AddRule(string wcfUrl, string wcfServiceUserName)
        {
            if (string.IsNullOrEmpty(wcfUrl))
            {
                throw new ArgumentException(nameof(wcfUrl));
            }

            if (string.IsNullOrEmpty(wcfServiceUserName))
            {
                throw new ArgumentException(nameof(wcfServiceUserName));
            }

            RunFirewallConfiguration(string.Format(CmdLineDeleteWcfUrlFirewallRule, wcfUrl));
            return RunFirewallConfiguration(string.Format(CmdLineAddWcfUrlFirewallRule, wcfUrl, wcfServiceUserName));
        }

        /// <summary>
        /// Removes a firewall access rule or the specified WCF port.
        /// </summary>
        /// <param name="wcfPort">The WCF port to add the firewall rule for.</param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        public static bool RemoveRule(ushort wcfPort)
        {
            if (wcfPort == 0)
            {
                throw new ArgumentException(nameof(wcfPort));
            }

            return RunFirewallConfiguration(string.Format(CmdLineDeleteWcfFirewallRule, wcfPort));
        }

        /// <summary>
        /// Removes a firewall access rule or the specified WCF url.
        /// </summary>
        /// <param name="wcfPort">The WCF url to add the firewall rule for.</param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        public static bool RemoveRule(string wcfUrl)
        {
            if (string.IsNullOrEmpty(wcfUrl))
            {
                throw new ArgumentException(nameof(wcfUrl));
            }

            return RunFirewallConfiguration(string.Format(CmdLineDeleteWcfUrlFirewallRule, wcfUrl));
        }

        /// <summary>
        /// Runs the netsh firewall configuration command with the specified command line parameters.
        /// </summary>
        /// <param name="commandLine">The command line arguments to pass to netsh.</param>
        /// <returns>
        /// <c>true</c> if netsh returned without error; <c>false</c> otherwise.
        /// </returns>
        private static bool RunFirewallConfiguration(string commandLine)
        {
            Process proc = null;

            var logger = LogManager.GetLogger(typeof(Firewall));
            var netshFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), CmdConfigureFirewall);

            try
            {

                logger.Info("Configure firewall with '{0} {1}' ...", netshFileName, commandLine);

                proc = new Process();
                proc.StartInfo.FileName = netshFileName;
                proc.StartInfo.Arguments = commandLine;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.ErrorDialog = false;

                proc.Start();
                proc.WaitForExit();

                return (proc.ExitCode == 0);
            }
            catch (System.Exception ex)
            {
                logger.Error(ex, "Configure firewall failed.");
            }
            finally
            {
                if (proc != null)
                    proc.Close();
            }

            return false;
        }

        #endregion
    }
}
