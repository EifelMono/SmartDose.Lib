using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Rowa.Lib.Wcf.Native
{
    /// <summary>
    /// Class which contains all function import definitions of the advapi32.dll. 
    /// </summary>
    internal static class Advapi32
    {
        #region Constants

        /// <summary>
        /// Required access right to query an access token.
        /// </summary>
        public const uint TOKEN_QUERY = 0x0008;

        /// <summary>
        /// The well known SID of the network service account.
        /// </summary>
        public const string NetworkServiceSid = "S-1-5-20";

        /// <summary>
        /// The  well known SID of the everyone account.
        /// </summary>
        public const string EveryoneSid = "S-1-1-0";

        #endregion

        #region Enums

        /// <summary>
        /// The TOKEN_INFORMATION_CLASS enumeration contains values that specify 
        /// the type of information being assigned to or retrieved from an access token.
        /// </summary>
        public enum TOKEN_INFORMATION_CLASS : int
        {
            TokenUser = 1,
            TokenGroups,
            TokenPrivileges,
            TokenOwner,
            TokenPrimaryGroup,
            TokenDefaultDacl,
            TokenSource,
            TokenType,
            TokenImpersonationLevel,
            TokenStatistics,
            TokenRestrictedSids,
            TokenSessionId,
            TokenGroupsAndPrivileges,
            TokenSessionReference,
            TokenSandBoxInert,
            TokenAuditPolicy,
            TokenOrigin,
            TokenElevationType,
            TokenLinkedToken,
            TokenElevation,
            TokenHasRestrictions,
            TokenAccessInformation,
            TokenVirtualizationAllowed,
            TokenVirtualizationEnabled,
            TokenIntegrityLevel,
            TokenUIAccess,
            TokenMandatoryPolicy,
            TokenLogonSid,
            TokenIsAppContainer,
            TokenCapabilities,
            TokenAppContainerSid,
            TokenAppContainerNumber,
            TokenUserClaimAttributes,
            TokenDeviceClaimAttributes,
            TokenRestrictedUserClaimAttributes,
            TokenRestrictedDeviceClaimAttributes,
            TokenDeviceGroups,
            TokenRestrictedDeviceGroups,
            TokenSecurityAttributes,
            TokenIsRestricted,
            MaxTokenInfoClass
        }

        #endregion

        #region Function Imports

        /// <summary>
        /// The ConvertStringSidToSid function converts a string-format security identifier (SID) into a valid, functional SID.
        /// </summary>
        /// <param name="StringSid">
        /// A pointer to a null-terminated string containing the string-format SID to convert. 
        /// The SID string can use either the standard S-R-I-S-S… format for SID strings, or the SID string constant format, 
        /// such as "BA" for built-in administrators. For more information about SID string notation, see SID Components.
        /// </param>
        /// <param name="pSid">A pointer to a variable that receives a pointer to the converted SID. To free the returned buffer, call the LocalFree function.</param>
        /// <returns>If the function succeeds, the return value is false.</returns>
        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern bool ConvertStringSidToSid(string StringSid, out IntPtr pSid);

        /// <summary>
        /// The LookupAccountSid function accepts a security identifier (SID) as input. 
        /// It retrieves the name of the account for this SID and the name of the first domain on which this SID is found.
        /// </summary>
        /// <param name="lpSystemName">
        /// A pointer to a null-terminated character string that specifies the target computer. 
        /// This string can be the name of a remote computer. If this parameter is NULL, the account name translation begins on the local system. 
        /// If the name cannot be resolved on the local system, this function will try to resolve the name using domain controllers trusted by the local system. 
        /// Generally, specify a value for lpSystemName only when the account is in an untrusted domain and the name of a computer in that domain is known.
        /// </param>
        /// <param name="pSid">A pointer to the SID to look up.</param>
        /// <param name="lpName">A pointer to a buffer that receives a null-terminated string that contains the account name that corresponds to the lpSid parameter.</param>
        /// <param name="cchName">
        /// On input, specifies the size, in TCHARs, of the lpName buffer. 
        /// If the function fails because the buffer is too small or if cchName is zero, cchName receives the required buffer size, including the terminating null character.
        /// </param>
        /// <param name="lpReferencedDomainName">
        /// A pointer to a buffer that receives a null-terminated string that contains the name of the domain where the account name was found.
        /// On a server, the domain name returned for most accounts in the security database of the local computer is the name of the domain for which the server is a domain controller.
        /// On a workstation, the domain name returned for most accounts in the security database of the local computer is the name of the computer as of the last start of the system 
        /// (backslashes are excluded). If the name of the computer changes, the old name continues to be returned as the domain name until the system is restarted.
        /// Some accounts are predefined by the system. The domain name returned for these accounts is BUILTIN.
        /// </param>
        /// <param name="cchReferencedDomainName">
        /// On input, specifies the size, in TCHARs, of the lpReferencedDomainName buffer. 
        /// If the function fails because the buffer is too small or if cchReferencedDomainName is zero, cchReferencedDomainName receives the required buffer size, including the terminating null character.
        /// </param>
        /// <param name="peUse">A pointer to a variable that receives a SID_NAME_USE value that indicates the type of the account.</param>
        /// <returns>If the function succeeds, the function returns false.</returns>
        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern bool LookupAccountSid(string lpSystemName,
                                                   IntPtr pSid,
                                                   StringBuilder lpName,
                                                   ref uint cchName,
                                                   StringBuilder lpReferencedDomainName,
                                                   ref uint cchReferencedDomainName,
                                                   out int peUse);

        /// <summary>
        /// The OpenProcessToken function opens the access token associated with a process.
        /// </summary>
        /// <param name="hProcessHandle">
        /// A handle to the process whose access token is opened. 
        /// The process must have the PROCESS_QUERY_INFORMATION access permission.
        /// </param>
        /// <param name="dwDesiredAccess">
        /// Specifies an access mask that specifies the requested types of access to the access token. 
        /// These requested access types are compared with the discretionary access control list (DACL) 
        /// of the token to determine which accesses are granted or denied. 
        /// </param>
        /// <param name="hTokenHandle">
        /// A pointer to a handle that identifies the newly opened access token when the function returns.
        /// </param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern bool OpenProcessToken(IntPtr hProcessHandle,
                                                   uint dwDesiredAccess,
                                                   out IntPtr hTokenHandle);

        /// <summary>
        /// The GetTokenInformation function retrieves a specified type of information about an access token. 
        /// The calling process must have appropriate access rights to obtain the information.
        /// </summary>
        /// <param name="hTokenHandle">
        /// A handle to an access token from which information is retrieved. If TokenInformationClass specifies 
        /// TokenSource, the handle must have TOKEN_QUERY_SOURCE access. For all other TokenInformationClass values, 
        /// the handle must have TOKEN_QUERY access.
        /// </param>
        /// <param name="tokenInformationClass">
        /// Specifies a value from the TOKEN_INFORMATION_CLASS enumerated type to identify the type of information 
        /// the function retrieves. Any callers who check the TokenIsAppContainer and have it return 0 should also 
        /// verify that the caller token is not an identify level impersonation token. If the current token is not 
        /// an app container but is an identity level token, you should return AccessDenied.
        /// </param>
        /// <param name="pTokenInformation">
        /// A pointer to a buffer the function fills with the requested information. The structure put into this 
        /// buffer depends upon the type of information specified by the TokenInformationClass parameter.
        /// </param>
        /// <param name="dwTokenInformationLength">
        /// Specifies the size, in bytes, of the buffer pointed to by the TokenInformation parameter. 
        /// If TokenInformation is NULL, this parameter must be zero.
        /// </param>
        /// <param name="dwReturnLength">
        /// A pointer to a variable that receives the number of bytes needed for the buffer pointed to by the 
        /// TokenInformation parameter. If this value is larger than the value specified in the TokenInformationLength 
        /// parameter, the function fails and stores no data in the buffer.
        /// If the value of the TokenInformationClass parameter is TokenDefaultDacl and the token has no default DACL, 
        /// the function sets the variable pointed to by ReturnLength to sizeof(TOKEN_DEFAULT_DACL) and sets the 
        /// DefaultDacl member of the TOKEN_DEFAULT_DACL structure to NULL.
        /// </param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern bool GetTokenInformation(IntPtr hTokenHandle,
                                                      TOKEN_INFORMATION_CLASS tokenInformationClass,
                                                      out uint pTokenInformation,
                                                      uint dwTokenInformationLength,
                                                      out uint dwReturnLength);

        /// <summary>
        /// The GetTokenInformation function retrieves a specified type of information about an access token. 
        /// The calling process must have appropriate access rights to obtain the information.
        /// </summary>
        /// <param name="hTokenHandle">
        /// A handle to an access token from which information is retrieved. If TokenInformationClass specifies 
        /// TokenSource, the handle must have TOKEN_QUERY_SOURCE access. For all other TokenInformationClass values, 
        /// the handle must have TOKEN_QUERY access.
        /// </param>
        /// <param name="tokenInformationClass">
        /// Specifies a value from the TOKEN_INFORMATION_CLASS enumerated type to identify the type of information 
        /// the function retrieves. Any callers who check the TokenIsAppContainer and have it return 0 should also 
        /// verify that the caller token is not an identify level impersonation token. If the current token is not 
        /// an app container but is an identity level token, you should return AccessDenied.
        /// </param>
        /// <param name="pTokenInformation">
        /// A pointer to a buffer the function fills with the requested information. The structure put into this 
        /// buffer depends upon the type of information specified by the TokenInformationClass parameter.
        /// </param>
        /// <param name="dwTokenInformationLength">
        /// Specifies the size, in bytes, of the buffer pointed to by the TokenInformation parameter. 
        /// If TokenInformation is NULL, this parameter must be zero.
        /// </param>
        /// <param name="dwReturnLength">
        /// A pointer to a variable that receives the number of bytes needed for the buffer pointed to by the 
        /// TokenInformation parameter. If this value is larger than the value specified in the TokenInformationLength 
        /// parameter, the function fails and stores no data in the buffer.
        /// If the value of the TokenInformationClass parameter is TokenDefaultDacl and the token has no default DACL, 
        /// the function sets the variable pointed to by ReturnLength to sizeof(TOKEN_DEFAULT_DACL) and sets the 
        /// DefaultDacl member of the TOKEN_DEFAULT_DACL structure to NULL.
        /// </param>
        /// <returns>
        /// <c>true</c> if successful; <c>false</c> otherwise.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass")]
        public static extern bool GetTokenInformation(IntPtr hTokenHandle,
                                                      TOKEN_INFORMATION_CLASS tokenInformationClass,
                                                      IntPtr pTokenInformation,
                                                      uint dwTokenInformationLength,
                                                      out uint dwReturnLength);


        /// <summary>
        /// Gets the localized name of the network service account.
        /// </summary>
        /// <value>
        /// The name of the network service account.
        /// </value>
        public static string GetNetworkServiceName()
        {
            return GetNameBySid(NetworkServiceSid);
        }

        /// <summary>
        /// Gets the localized name of the everyone account.
        /// </summary>
        /// <value>
        /// The name of the everyone account.
        /// </value>
        public static string GetEveryOneName()
        {
            return GetNameBySid(EveryoneSid);
        }

        /// <summary>
        /// Gets the localized name of the account with the specified sid.
        /// </summary>
        /// <param name="sid">The account sid to get the name for.</param>
        /// <returns>Localized name of the account with the specified sid.</returns>
        private static string GetNameBySid(string sid)
        {
            IntPtr pSid = IntPtr.Zero;

            try
            {
                if (ConvertStringSidToSid(sid, out pSid) == false)
                {
                    return string.Empty;
                }

                uint cchName = 1024;
                uint cchDomainName = 1024;
                StringBuilder lpName = new StringBuilder((int)cchName);
                StringBuilder lpDomainName = new StringBuilder((int)cchDomainName);
                int euse = 0;

                if (LookupAccountSid(null, pSid, lpName, ref cchName, lpDomainName, ref cchDomainName, out euse) == false)
                {
                    return string.Empty;
                }

                return lpName.ToString();
            }
            finally
            {
                if (pSid != IntPtr.Zero)
                {
                    Kernel32.LocalFree(pSid);
                }
            }
        }

        #endregion
    }
}
