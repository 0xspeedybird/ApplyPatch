using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApplyPatch
{
    class PatchHelper
    {

        public const string RAW_PATCH_LINK_URL = "https://raw.githubusercontent.com/keylase/nvidia-patch/master/win";

        static public void automate(MainForm mainForm)
        {

            mainForm.setDefaultFile();

            Dictionary<string, string> sysInfo = getSystemInfo();

            string patchDownloadUrl;
            sysInfo.TryGetValue("PatchURL", out patchDownloadUrl);

            if (patchDownloadUrl != String.Empty)
            {

                Task<DownloadPageAsyncResult> downloadTask = Task.Run(() => DownloadPatchAsync(patchDownloadUrl));
                downloadTask.Wait();
                var result = downloadTask.Result.result;

                //check patch
                if (!String.IsNullOrEmpty(result))
                {
                    int returnCode = mainForm.parsePatch(result);
                    if (returnCode == Patch.NO_ERR_OK)
                    {
                        mainForm.applyPatch();
                        if (returnCode == Patch.NO_ERR_OK)
                        {
                            Environment.Exit(0);
                        }
                    }
                    Environment.Exit(returnCode);
                }
            }
            Environment.Exit(999);
        }

        static async public Task<DownloadPageAsyncResult> DownloadPatchAsync(string path)
        {
            HttpClient client = new HttpClient();
            var result = new DownloadPageAsyncResult();
            try
            {
                string downloadContent = await client.GetStringAsync(path);
                // normalize line endings
                downloadContent = Regex.Replace(downloadContent, @"\r\n|\n\r|\n|\r", Environment.NewLine);
                result.result = downloadContent;
            }
            catch (Exception ex)
            {
                // need to return ex.message for display.
                result.errorMessage = ex.Message;
            }
            return result;
        }

        static public Dictionary<string, string> getSystemInfo()
        {

            Dictionary<string, string> dict = new Dictionary<string, string>();

            // video drivers -> 4d36e968-e325-11ce-bfc1-08002be10318
            ManagementObjectSearcher objSearcher = new ManagementObjectSearcher("Select * from Win32_PnPSignedDriver WHERE Manufacturer = 'NVIDIA' and ClassGuid = '{4d36e968-e325-11ce-bfc1-08002be10318}'");
            ManagementObjectCollection objCollection = objSearcher.Get();
            foreach (ManagementObject obj in objCollection.Cast<ManagementObject>())
            {
                string m = (string)obj["Manufacturer"];
                if (m != null)
                {
                    try
                    {
                        string osVersion = (string)(from x in new ManagementObjectSearcher("SELECT Version FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                                                    select x.GetPropertyValue("Version")).FirstOrDefault();
                        string osMajorVersion = osVersion.Substring(0, osVersion.IndexOf("."));
                        string friendlyOSversion = !String.IsNullOrEmpty(osVersion) ? "Windows " + osMajorVersion : "Unknown OS";

                        //construct URL

                        //map win version to keylase paths
                        //map Win 8 and 8.1 -> Win 7
                        if (osMajorVersion.Equals("8") || osMajorVersion.Equals("8.1")) { osMajorVersion = "7"; }
                        //map Win 11 -> Win 10
                        if (osMajorVersion.Equals("11")) { osMajorVersion = "7"; }

                        //win reports NVidia driver differently than NVidia's Control Panel.  We need the latter
                        //For ex:  30.0.15.1277 vs  512.77
                        string deviceName = (string)obj["DeviceName"];
                        string winReportedNVidiaDriver = (string)obj["DriverVersion"];
                        string mappedNVidiaDriverVersion = winReportedNVidiaDriver.Replace(".", "");
                        mappedNVidiaDriverVersion = mappedNVidiaDriverVersion.Substring(4).Insert(3, ".");

                        //now let's create the link
                        string keylaseFinalUrl = String.Format("{0}/win{1}_x64/{2}/nvencodeapi64.1337", RAW_PATCH_LINK_URL, osMajorVersion,
                            (deviceName.IndexOf("Quadro") != -1 ? "quadro_" : "") + mappedNVidiaDriverVersion);


                        dict.Add("osVersion", osVersion);
                        dict.Add("osMajorVersion", osMajorVersion);
                        dict.Add("friendlyOSversion", friendlyOSversion);
                        dict.Add("deviceName", deviceName);
                        dict.Add("deviceManufacturer", (string)obj["Manufacturer"]);
                        dict.Add("winReportedNVidiaDriver", winReportedNVidiaDriver);
                        dict.Add("mappedNVidiaDriverVersion", mappedNVidiaDriverVersion);
                        dict.Add("PatchURL", keylaseFinalUrl);

                        return dict;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                }
            }
            return null;
        }

        static public string getPatchURL()
        {

            Dictionary<string, string> sysInfo = getSystemInfo();

            string patchDownloadUrl;
            sysInfo.TryGetValue("PatchURL", out patchDownloadUrl);

            return patchDownloadUrl;
        }

        static public string getSystemSummary()
        {

            Dictionary<string, string> sysInfo = getSystemInfo();

            string friendlyOSversion;
            sysInfo.TryGetValue("friendlyOSversion", out friendlyOSversion);

            string deviceName;
            sysInfo.TryGetValue("deviceName", out deviceName);

            string winReportedNVidiaDriver;
            sysInfo.TryGetValue("winReportedNVidiaDriver", out winReportedNVidiaDriver);

            string mappedNVidiaDriverVersion;
            sysInfo.TryGetValue("mappedNVidiaDriverVersion", out mappedNVidiaDriverVersion);

            string deviceManufacturer;
            sysInfo.TryGetValue("deviceManufacturer", out deviceManufacturer);

            return String.Format("{0}\n\nGPU Card: {1}\n\nWindows Reported Driver Version: {2}\n\nMapped Driver Version: {3}\n\nManufacturer:{4}",

                friendlyOSversion, deviceName, winReportedNVidiaDriver,
                mappedNVidiaDriverVersion, deviceManufacturer);;
        }
    }
}