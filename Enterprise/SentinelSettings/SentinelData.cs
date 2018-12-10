using System;
using System.Linq;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;

namespace SentinelSettings
{
    public class SentinelData
    {
        public static Dictionary<string, string> vendorCode = new Dictionary<string, string> { { "DEMOMA", "AzIceaqfA1hX5wS+M8cGnYh5ceevUnOZIzJBbXFD6dgf3tBkb9cvUF/Tkd/iKu2fsg9wAysYKw7RMAsVvIp4KcXle/v1RaXrLVnNBJ2H2DmrbUMOZbQUFXe698qmJsqNpLXRA367xpZ54i8kC5DTXwDhfxWTOZrBrh5sRKHcoVLumztIQjgWh37AzmSd1bLOfUGI0xjAL9zJWO3fRaeB0NS2KlmoKaVT5Y04zZEc06waU2r6AU2Dc4uipJqJmObqKM+tfNKAS0rZr5IudRiC7pUwnmtaHRe5fgSI8M7yvypvm+13Wm4Gwd4VnYiZvSxf8ImN3ZOG9wEzfyMIlH2+rKPUVHI+igsqla0Wd9m7ZUR9vFotj1uYV0OzG7hX0+huN2E/IdgLDjbiapj1e2fKHrMmGFaIvI6xzzJIQJF9GiRZ7+0jNFLKSyzX/K3JAyFrIPObfwM+y+zAgE1sWcZ1YnuBhICyRHBhaJDKIZL8MywrEfB2yF+R3k9wFG1oN48gSLyfrfEKuB/qgNp+BeTruWUk0AwRE9XVMUuRbjpxa4YA67SKunFEgFGgUfHBeHJTivvUl0u4Dki1UKAT973P+nXy2O0u239If/kRpNUVhMg8kpk7s8i6Arp7l/705/bLCx4kN5hHHSXIqkiG9tHdeNV8VYo5+72hgaCx3/uVoVLmtvxbOIvo120uTJbuLVTvT8KtsOlb3DxwUrwLzaEMoAQAFk6Q9bNipHxfkRQER4kR7IYTMzSoW5mxh3H9O8Ge5BqVeYMEW36q9wnOYfxOLNw6yQMf8f9sJN4KhZty02xm707S7VEfJJ1KNq7b5pP/3RjE0IKtB2gE6vAPRvRLzEohu0m7q1aUp8wAvSiqjZy7FLaTtLEApXYvLvz6PEJdj4TegCZugj7c8bIOEqLXmloZ6EgVnjQ7/ttys7VFITB3mazzFiyQuKf4J6+b/a/Y" } };

        public static XDocument errors = new XDocument();

        /*public static XDocument defaultErrors = XDocument.Parse(
  "<ErrorCodes>" + 
    "<!-- Sentinel EMS Errors -->" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Some unexpected error occurred in getting pending updates</language>" +
      "<language type=\"translate\" name=\"En\">Some unexpected error occurred in getting pending updates.Unable to retrieve Protection Key for current Key ID.Probably there is no key with such Key ID in the Sentinel EMS database.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">No pending update.</language>" +
      "<language type=\"translate\" name=\"En\">Have no pending update for download.</language>" +
    "</error>" +
    "<!-- Licensing API Errors -->" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">StatusOk</language>" +
      "<language type=\"translate\" name=\"En\">Request was successfully completed!</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidAddress</language>" +
      "<language type=\"translate\" name=\"En\">Request exceeds the Sentinel protection key memory range.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidFeature</language>" +
      "<language type=\"translate\" name=\"En\">Legacy HASP HL Run - time API: Unknown / Invalid Feature ID option.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">NotEnoughMemory</language>" +
      "<language type=\"translate\" name=\"En\">System is out of memory.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TooManyOpenFeatures</language>" +
      "<language type=\"translate\" name=\"En\">Too many open sessions exist.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">AccessDenied</language>" +
      "<language type=\"translate\" name=\"En\">Access to Feature was denied.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">IncompatibleFeature</language>" +
      "<language type=\"translate\" name=\"En\">Legacy decryption method cannot work on Feature.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">ContainerNotFound</language>" +
      "<language type=\"translate\" name=\"En\">Sentinel protection key is no longer available.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">BufferTooShort</language>" +
      "<language type=\"translate\" name=\"En\">Encrypted / decrypted data length too short to execute method call.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidHandle</language>" +
      "<language type=\"translate\" name=\"En\">Invalid handle was passed to method.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidFile</language>" +
      "<language type=\"translate\" name=\"En\">Specified File ID is not recognized by API.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">DriverTooOld</language>" +
      "<language type=\"translate\" name=\"En\">Installed driver is too old to execute method.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">NoTime</language>" +
      "<language type=\"translate\" name=\"En\">Real - time clock(RTC) not available.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">SystemError</language>" +
      "<language type=\"translate\" name=\"En\">Generic error from host system call.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">DriverNotFound</language>" +
      "<language type=\"translate\" name=\"En\">Required driver is not installed.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidFormat</language>" +
      "<language type=\"translate\" name=\"En\">File format for update is not recognized.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">RequestNotSupported</language>" +
      "<language type=\"translate\" name=\"En\">Unable to execute method in this context.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidUpdateObject</language>" +
      "<language type=\"translate\" name=\"En\">Binary data that was passed to method does not contain an update.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">KeyIdNotFound</language>" +
      "<language type=\"translate\" name=\"En\">Sentinel protection key was not found.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidUpdateData</language>" +
      "<language type=\"translate\" name=\"En\">" +
        "Required XML tags were not found." +
        "OR" +
        "Contents in binary data are missing or invalid." +
      "</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UpdateNotSupported</language>" +
      "<language type=\"translate\" name=\"En\">Update request is not supported by Sentinel protection key.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidUpdateCounter</language>" +
      "<language type=\"translate\" name=\"En\">Update counter is not set correctly.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidVendorCode</language>" +
      "<language type=\"translate\" name=\"En\">Invalid Vendor Code was passed.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">EncryptionNotSupported</language>" +
      "<language type=\"translate\" name=\"En\">Sentinel protection key does not support encryption type.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidTime</language>" +
      "<language type=\"translate\" name=\"En\">The time value that was passed is outside the supported value range.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">NoBatteryPower</language>" +
      "<language type=\"translate\" name=\"En\">The real - time clock battery has run out of power.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UpdateNoAckSpace</language>" +
      "<language type=\"translate\" name=\"En\">Acknowledge data that was requested by the update ack_data parameter is NULL.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TerminalServiceDetected</language>" +
      "<language type=\"translate\" name=\"En\">Program is running on a terminal server.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">FeatureNoteImplemented</language>" +
      "<language type=\"translate\" name=\"En\">Requested Feature type is not implemented.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UnknownAlgorithm</language>" +
      "<language type=\"translate\" name=\"En\">Unknown algorithm used in V2C or V2CP file.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidSignature</language>" +
      "<language type=\"translate\" name=\"En\">Signature verification operation failed.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">FeatureNotFound</language>" +
      "<language type=\"translate\" name=\"En\">Requested Feature not found.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">NoLog</language>" +
      "<language type=\"translate\" name=\"En\">Access log not enabled.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">LocalComErr</language>" +
      "<language type=\"translate\" name=\"En\">Communication error occurred between the API and the local Sentinel License Manager.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UnknownVcode</language>" +
      "<language type=\"translate\" name=\"En\">Vendor Code is not recognized by API.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidXmlSpec</language>" +
      "<language type=\"translate\" name=\"En\">Invalid XML specification exists.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidXmlScope</language>" +
      "<language type=\"translate\" name=\"En\">Invalid XML scope exists.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TooManyKeys</language>" +
      "<language type=\"translate\" name=\"En\">Too many Sentinel protection keys are currently connected.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TooManyUsers</language>" +
      "<language type=\"translate\" name=\"En\">Too many users are currently connected.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">BrokenSession</language>" +
      "<language type=\"translate\" name=\"En\">" +
        "Session was interrupted.This can occur when certain updates are applied to the license while a session is active.For example:" +
        "-A Feature required by the session was deleted." +
        "- The license was canceled." +
        "- The network(remote license) support setting for a required Feature was changed. (In this case, all sessions will be interrupted, including local sessions.)" +
      "</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">RemoteCommErr</language>" +
      "<language type=\"translate\" name=\"En\">Communication error occurred between local and remote Sentinel License Managers.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">FeatureExpired</language>" +
      "<language type=\"translate\" name=\"En\">Feature expired or no executions remain.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TooOldLM</language>" +
      "<language type=\"translate\" name=\"En\">Sentinel License Manager version too old.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">DeviceError</language>" +
      "<language type=\"translate\" name=\"En\">" +
        "For a Sentinel SL key, an input / output error occurred in the secure storage area." +
        "OR" +
        "For a Sentinel HL key, a USB communication error occurred." +
      "</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UpdateBlocked</language>" +
      "<language type=\"translate\" name=\"En\">Update installation not permitted.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TimeError</language>" +
      "<language type=\"translate\" name=\"En\">System time has been tampered with.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">SecureChannelError</language>" +
      "<language type=\"translate\" name=\"En\">Communication error occurred in the secure channel.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">CorruptStorage</language>" +
      "<language type=\"translate\" name=\"En\">Corrupt data exists in secure storage area of Sentinel protection key.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">VendorLibNotFound</language>" +
      "<language type=\"translate\" name=\"En\">The customized vendor library(haspvlib.vendorID.*) cannot be located.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidVendorLib</language>" +
      "<language type=\"translate\" name=\"En\">Unable to load Vendor library.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">EmptyScopeResults</language>" +
      "<language type=\"translate\" name=\"En\">Unable to locate any Feature that matches the scope.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">VMDetected</language>" +
      "<language type=\"translate\" name=\"En\">" +
        "Protected application is running on a virtual machine, but one or more Features are not enabled for virtual machines." +
        "OR" +
        "The user attempted to rehost a protection key from a physical machine to a virtual machine.However, none of the Features contained in the protection key are enabled for virtual machines." +
      "</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">HardwareModified</language>" +
      "<language type=\"translate\" name=\"En\">" +
       "Sentinel SL key incompatible with machine hardware.Sentinel SL key locked to different hardware." +
       "OR" +
       "In the case of a V2C or V2CP file, conflict between Sentinel SL key data and machine hardware data.Sentinel SL key locked to different hardware." +
      "</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UserDenied</language>" + 
      "<language type=\"translate\" name=\"En\">Login denied because of user restrictions.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UpdateTooOld</language>" +
      "<language type=\"translate\" name=\"En\">Trying to install a V2C or V2CP file with an update counter that is out of sequence with update counter in the Sentinel protection key. Values of update counter in file are lower than those in Sentinel protection key.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UpdateTooNew</language>" +
      "<language type=\"translate\" name=\"En\">Trying to install a V2C or V2CP file with an update counter that is out of sequence with the update counter in the Sentinel protection key. First value in file is more-than-1 greater than value in Sentinel protection key.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">VendorLibOld</language>" +
      "<language type=\"translate\" name=\"En\">Vendor library is too old.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UploadError</language>" +
      "<language type=\"translate\" name=\"En\">Check in of a file (such as  V2C, H2R) using Admin Control Center failed, possibly because of illegal format.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidRecipient</language>" +
      "<language type=\"translate\" name=\"En\">Invalid XML RECIPIENT parameter.</language>" +
      "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidDetachAction</language>" +
      "<language type=\"translate\" name=\"En\">Invalid XML action parameter.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TooManyProducts</language>" +
      "<language type=\"translate\" name=\"En\">The scope specified in the Transfer method does not specify a unique Product.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidProduct</language>" +
      "<language type=\"translate\" name=\"En\">Invalid Product information.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UnknownRecipient</language>" +
      "language type=\"translate\" name=\"En\">Update can only be applied to recipient machine specified in the Detach method, not to this machine.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidDuration</language>" +
      "<language type=\"translate\" name=\"En\"> " +
          "Invalid detached license duration period specified.Duration must be less than or equal to maximum allowed for this license." +
          "OR" +
          "Duration extension is to a date earlier than the expiration date of the current detached license." +
      "</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">CloneDetected</language>" +
      "<language type=\"translate\" name=\"En\">Cloned Sentinel SL storage was detected.Feature is unavailable.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">UpdateAlreadyAdded</language>" +
      "<language type=\"translate\" name=\"En\">The specified V2C or or V2CP update was already installed in the License Manager service.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">HaspInactive</language>" +
      "<language type=\"translate\" name=\"En\">Specified Key ID is in Inactive state.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">NoDetachableFeature</language>" +
      "<language type=\"translate\" name=\"En\">No detachable Feature exists in the specified key from which the detached license is requested.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">TooManyHosts</language>" +
      "language type=\"translate\" name=\"En\">The specified scope does not specify a unique host.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">RehostNotAllowed</language>" +
      "language type=\"translate\" name=\"En\">Rehost action is not allowed for the specified Key ID.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">LicenseRehosted</language>" +
      "language type=\"translate\" name=\"En\">Original license has been transferred to another machine.Therefore, the license cannot be returned to the source machine.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">RehostAlreadyApplied</language>" +
      "language type=\"translate\" name=\"En\">Old rehost license cannot be applied.A rehost-counter mismatch occurred.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">CannotReadFile</language>" +
      "<language type=\"translate\" name=\"En\">A V2C or V2CP file was not found, or access was denied.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">ExtensionNotAllowed</language>" +
      "<language type=\"translate\" name=\"En\">The license cannot be extended because the number of detached licenses is greater than the number of concurrent licenses allowed.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">DetachDisabled</language>" +
      "<language type=\"translate\" name=\"En\">The user attempted to detach a Product from a network license hosted on a virtual machine.However, none of the Features included in the Product are enabled for virtual machines.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">RehostDisabled</language>" +
      "<language type=\"translate\" name=\"En\">The user attempted to rehost a protection key from a virtual machine.However, none of the Features contained in the protection key are Enabled for virtual machines.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">DetachedLicenseFound</language>" +
      "<language type=\"translate\" name=\"En\">The user attempted to format an SL-AdminMode key or to migrate an SL-Legacy key to an SL-AdminMode key. However, a Product is currently detached from the key.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">RecipientOldLm</language>" +
      "<language type=\"translate\" name=\"En\">For a rehost operation: The fingerprint of the target machine was collected using tools (RUS utility or Licensing API) earlier than Sentinel LDK v.7.0.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">SecureStoreIdMismatch</language>" +
      "<language type=\"translate\" name=\"En\">A secure storage ID mismatch occurred.</language>" +
      "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Duplicatehostname</language>" +
      "<language type=\"translate\" name=\"En\">The license fingerprint is bound to a specific hostname; however, two or more machines with this hostname were found in the network. As a result, the license cannot be used.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">MissingLM</language>" +
      "<language type=\"translate\" name=\"En\">A protected application tried to log in to a Feature that supports concurrency on a Sentinel HL(Driverless configuration) key.The Sentinel LDK License Manager service is not active on the computer where the key is located.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">FeatureInsufficientExecutionCount</language>" +
      "<language type=\"translate\" name=\"En\">A protected application tried to consume multiple executions while logging in to a Feature.However, the license does not contain the number of executions that were requested.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">HaspDisabled</language>" +
      "<language type=\"translate\" name=\"En\">A Sentinel HL (Driverless configuration) key was disabled because a user attempted to tamper with the key or with the protected application.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">NoApiDylib</language>" +
      "<language type=\"translate\" name=\"En\">Unable to locate dynamic library for API.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvApiDylib</language>" +
      "<language type=\"translate\" name=\"En\">Dynamic library for API is invalid.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidObject</language>" +
      "<language type=\"translate\" name=\"En\">Object was incorrectly initialized.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InvalidParameter</language>" +
      "< language type=\"translate\" name=\"En\">Scope string is too long (maximum length is 32 KB).</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">AreadyLoggedIn</language>" +
      "<language type=\"translate\" name=\"En\">Logging in twice to same object.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">AlreadyLoggedOut</language>" +
      "<language type=\"translate\" name=\"En\">Logging out twice from same object.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">OperationFailed</language>" +
      "<language type=\"translate\" name=\"En\">Incorrect use of system or platform.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">HaspDotNetDllBroken</language>" +
      "<language type=\"translate\" name=\"En\">.NET DLL found broken.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">NotImplemented</language>" +
      "<language type=\"translate\" name=\"En\"> " +
          "Requested function was not implemented." +
          "OR" +
          "In the case of the API Dispatcher, API DLL is too old." +
      "</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">InternalError</language>" +
      "<language type=\"translate\" name=\"En\">Internal error occurred in the API.</language>" +
    "</error>" +
    "<!-- Internal Errors -->" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Invalid ProductKey or C2V</language>" +
      "<language type=\"translate\" name=\"En\">Invalid ProductKey or C2V.Please check it and try again.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error in request C2V</language>" +
      "<language type=\"translate\" name=\"En\">Error in request C2V.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Do you want to install license in New SL Key</language>" +
      "<language type=\"translate\" name=\"En\">Do you want to install license in New SL Key?</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Do you want to install license in exist Key</language>" +
      "<language type=\"translate\" name=\"En\">Do you want to install license in exist Key: {0} with Key ID = {1}? If you chouse \"No\", license will be installed in new SL key.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">No pending update</language>" +
      "<language type=\"translate\" name=\"En\">No pending update.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Response from server has error or empty</language>" +
      "<language type=\"translate\" name=\"En\">Response from server has error or empty.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error</language>" +
      "<language type=\"translate\" name=\"En\">Error</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Request</language>" +
      "<language type=\"translate\" name=\"En\">Request</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Successfully</language>" +
      "<language type=\"translate\" name=\"En\">Successfully</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Warning</language>" +
      "<language type=\"translate\" name=\"En\">Warning</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">License update successfully</language>" +
      "<language type=\"translate\" name=\"En\">License update successfully installed!</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Update didn't installed</language>" +
      "<language type=\"translate\" name=\"En\">Update didn't installed!</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Saving file error</language>" +
      "<language type=\"translate\" name=\"En\">Saving file error: {0}</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error: Accounting.exe not found in dir</language>" +
      "<language type=\"translate\" name=\"En\">Error: Accounting.exe not found in dir: {0}</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error: Stock.exe not found in dir</language>" +
      "<language type=\"translate\" name=\"En\">Error: Stock.exe not found in dir: {0}</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error: Staff.exe not found in dir</language>" +
      "<language type=\"translate\" name=\"En\">Error: Staff.exe not found in dir: {0}</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error: </language>" +
      "<language type=\"translate\" name=\"En\">Error: {0}</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Can't create dir for logs</language>" +
      "<language type=\"translate\" name=\"En\">Can't create dir for logs! Error: {0}</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Can't create log file</language>" +
      "<language type=\"translate\" name=\"En\">Can't create log file! Error: {0}</language>" + 
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Not found trial license: \"trial_license\", -  in base dir</language>" +
      "<language type=\"translate\" name=\"En\">Not found trial license: \"trial_license\", -  in base dir!</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Do you want to install trial license</language>" +
      "<language type=\"translate\" name=\"En\">Do you want to install trial license?</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Trial license can't be applied! Error</language>" +
      "<language type=\"translate\" name=\"En\">Trial license can't be applied! Error: </language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Trial license successfully installed</language>" +
        "<language type=\"translate\" name=\"En\">Trial license successfully installed!</language>" + 
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Missing or limited physical connection to network</language>" + 
      "<language type=\"translate\" name=\"En\">Missing or limited physical connection to network. Please check your connetctions settings.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error: Phisichal network unavailable</language>" +
        "<language type=\"translate\" name=\"En\">Error: Phisichal network unavailable...</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Haven't access to the internet</language>" +
      "<language type=\"translate\" name=\"En\">Haven't access to the internet. Please check your firewall or connection settings.</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error: Internet access unavailable</language>" +
        "<language type=\"translate\" name=\"En\">Error: Internet access unavailable...</language>" +
    "</error>" +
    "<error>" +
      "<language type=\"origin\" name=\"En\">Error: SentinelUp Client not found in dir:</language>" +
      "<language type=\"translate\" name=\"En\">Error: SentinelUp Client not found in dir: {0}</language>" +
    "</error>" +
  "</ErrorCodes>");*/

        public static string appSentinelUpCallData = "<upclient>" +
                                                         "<param>" +
                                                             "<key>-url</key>" +
                                                             "<value>up.sentinelcloud.com</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-k</key>" +
                                                             "<value>eafe87d22cc2a49793276f4141c0ebb0</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-pc</key>" +
                                                             "<value>1337e5b0-fd2d-4018-8a13-a921bcbfb5d2</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-v</key>" +
                                                             "<value>v1</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-update</key>" +
                                                             "<value>-update</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-check</key>" +
                                                             "<value>-check</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-st</key>" +
                                                             "<value>-st</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-l</key>" +
                                                             "<value>ru</value>" +
                                                         "</param>" +
                                                         "<param>" +
                                                             "<key>-log</key>" +
                                                             "<value>update.log</value>" +
                                                         "</param>" +
                                                     "</upclient>";

        public static string keyScope = "<haspscope>" +
                                            "<feature>" +
                                                "<name>Accounting</name>" +
                                                "<id>1</id>" +
                                            "</feature>" +
                                            "<feature>" +
                                                "<name>Stock</name>" +
                                                "<id>2</id>" +
                                            "</feature>" +
                                            "<feature>" +
                                                "<name>Staff</name>" +
                                                "<id>3</id>" +
                                            "</feature>" +
                                        "</haspscope>";

        public static string keyScopeXsd = "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
                                            "<xs:schema attributeFormDefault=\"unqualified\" elementFormDefault=\"qualified\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\">" +
                                              "<xs:element name=\"haspscope\">" +
                                                "<xs:complexType>" +
                                                  "<xs:sequence>" +
                                                    "<xs:element maxOccurs=\"unbounded\" name=\"feature\">" +
                                                      "<xs:complexType>" +
                                                        "<xs:sequence>" +
                                                          "<xs:element name=\"name\" type=\"xs:string\"/>" +
                                                          "<xs:element name=\"id\" type=\"xs:string\"/>" +
                                                        "</xs:sequence>" +
                                                      "</xs:complexType>" +
                                                    "</xs:element>" +
                                                  "</xs:sequence>" +
                                                "</xs:complexType>" +
                                              "</xs:element>" +
                                            "</xs:schema>";

        public static string keyFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                            "<haspformat root=\"hasp_info\">" +
                                                "<hasp>" +
                                                    "<element name=\"id\"/>" +
                                                    "<element name=\"type\"/>" +
                                                    "<element name=\"version\"/>" +
                                                    "<element name=\"hw_version\"/>" +
                                                    "<element name=\"key_model\"/>" +
                                                    "<element name=\"key_type\"/>" +
                                                    "<element name=\"form_factor\"/>" +
                                                    "<element name=\"hw_platform\"/>" +
                                                    "<element name=\"driverless\"/>" +
                                                    "<element name=\"fingerprint_change\"/>" +
                                                    "<element name=\"vclock_enabled\"/>" +
                                                    "<product>" +
                                                        "<element name=\"id\"/>" +
                                                        "<element name=\"name\"/>" +
                                                        "<feature>" +
                                                            "<element name=\"id\"/>" +
                                                            "<element name=\"name\"/>" +
                                                            "<element name=\"license\"/>" +
                                                            "<element name=\"locked\"/>" +
                                                        "</feature>" +
                                                    "</product>" +
                                                "</hasp>" +
                                            "</haspformat>";

        public static string emsUrl = "http://localhost:8080/ems/v78/ws";

        public static bool logIsEnabled = true;

        public static bool apiIsEnabled = true;

        public static bool newActMechanism = true;

        public static bool advancedDataIsEnabled = true;

        public static string portForTestConnection = "8080";

        public SentinelData(string pathToAlp = null)
        {
            if (!String.IsNullOrEmpty(pathToAlp) && File.Exists(pathToAlp))
            {
                XDocument tmpAlp = XDocument.Load(pathToAlp);

                errors = XDocument.Parse(tmpAlp.Root.Element("ErrorCodes").ToString());
            }
            else
            {
                errors = null; //defaultErrors;
            }
        }

        public string ErrorMessageReplacer(string locale, string originalError)
        {
            string newErrorMessage = "";

            if (errors != null) {
                foreach (var elError in errors.Root.Elements("error"))
                {
                    foreach (var elLang in elError.Elements("language"))
                    {
                        if (elLang.Attribute("type").Value.Contains("origin") && originalError.Contains(elLang.Value))
                        {
                            newErrorMessage = ((from el in elError.Elements() where ((string)el.Attribute("type").Value == "translate" && (string)el.Attribute("name").Value == locale) select el).Count() > 0) ? (from el in elError.Elements() where ((string)el.Attribute("type").Value == "translate" && (string)el.Attribute("name").Value == locale) select el).FirstOrDefault().Value : (from el in elError.Elements() where ((string)el.Attribute("type").Value == "translate" && (string)el.Attribute("name").Value == "en") select el).FirstOrDefault().Value;
                            return newErrorMessage;
                        }
                    }
                }
            }

            if (String.IsNullOrEmpty(newErrorMessage)) newErrorMessage = originalError;

            return newErrorMessage;
        }
    }
}
