using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using xNet;

namespace buoi1_selenium
{
    internal class Program
    {
     
       static  void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();
            int demluong = 0;
            string[] listkey = { "UK-9172b00d-1c7f-4c8b-a6de-0155cd0b0643", "UK-c1b1d6e4-c139-43f2-91f3-c838610c9891" };
            int demkey = 0;
            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(() =>
                {
                    string proxy = "";
                    #region get proxy
                    string url = $"https://wwproxy.com/api/client/proxy/available?key={listkey[demkey]}";
                    if (demkey++ >= 1)
                    {
                        demkey = 0;
                    }
          
                    try
                    {
                        HttpRequest h = new HttpRequest();
                        string response = h.Get(url).ToString();

                        // Dùng Regex để lấy giá trị "proxy": "..."
                        Match match = Regex.Match(response, @"""proxy""\s*:\s*""([^""]+)""");
                        if (match.Success)
                        {
                            proxy = match.Groups[1].Value;
                            Console.WriteLine("Proxy là: " + proxy);
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy proxy trong phản hồi.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi gửi yêu cầu: " + ex.Message);
                    }
                    #endregion
                    #region khởi tạo chrome
                    IWebDriver chrome = null; List<int> driverProcessIds = new List<int>();

                    ChromeOptions chromeOptions = new ChromeOptions();
                    ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                    chromeDriverService.HideCommandPromptWindow = true;
                    chromeOptions.AddArguments("start-maximized");
                    chromeOptions.AddArgument("--disabel-notifications");
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddArguments(new string[]
                    {
            "--disable-3d-apis",
            "--disable-background-networking",
            "--disable-bundled-ppapi-flash",
            "--disable-client-side-phishing-detection",
            "--disable-default-apps",
            "--disable-hang-monitor",
            "--disable-gpu",
            "--no-sandbox",
            "--disable-prompt-on-repost",
            "--disable-sync",
            "--use-fake-device-for-media-stream",
            "--use-fake-ui-for-media-stream",
            "--disable-webgl",
            "--enable-blink-features=ShadowDOMV0",
            "--enable-logging",
            "--disable-notifications",
            "--disable-dev-shm-usage",
            "--disable-web-security",
            "--lang=vn",
            "--disable-rtc-smoothness-algorithm",
            "--disable-webrtc-hw-decoding",
            "--disable-webrtc-hw-encoding",
            "--disable-webrtc-multiple-routes",
            "--disabel-images",
            "--disable-webrtc-hw-vp8-encoding",
            "--enforce-webrtc-ip-permission-check",
            "--force-webrtc-ip-handling-policy",
            "--ignore-certificate-errors",
            "--disable-infobars",
            "--disable-popup-blocking",
            "--enable-precise-memory-info",
            "--disable-3d-apis",
            "--start-maximized",
            "--disable-blink-features=\"BlockCredentialedSubresources\"",
            "--mute-audio",
            "--window-size=450,600",//set size chrome
            "--disable-popup-blocking" });
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.geolocation", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.notifications", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.plugins", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.popups", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.auto_select_certificate", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.mixed_script", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.media_stream", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.media_stream_mic", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.media_stream_camera", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.protocol_handlers", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.midi_sysex", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.push_messaging", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.ssl_cert_decisions", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.metro_switch_to_desktop", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.protected_media_identifier", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.site_engagement", 1);
                    chromeOptions.AddUserProfilePreference("profile.default_contextn[ii,0]t_setting_values.durable_storage", 1);
                    chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
                    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                    chromeOptions.AddArgument("--disabel-notifications");
                    chromeOptions.AddArgument("--disabel-images");
                    if (!string.IsNullOrEmpty(proxy))
                    {
                        /// gán proxy cho chrome
                        chromeOptions.AddArgument($"--proxy-server={proxy}");// fake proxy dùng laoij http haowcsj https
                        Console.WriteLine($"Đang sử dụng proxy: {proxy}");
                    }
                    chrome = new ChromeDriver(chromeDriverService, chromeOptions);
                    IJavaScriptExecutor js = (IJavaScriptExecutor)chrome;// koiwr tạo javastripcode
                    #endregion
                    chrome.Navigate().GoToUrl("https://www.facebook.com/r.php?entry_point=login");// truy cập vào đường dẫn của trang
                    /// chrome.tile get title
                    /// chrome.PageSource get html 
                    /// chrome.close();
                    //chrome.Quit(); tắt haonf toàn chrome
                    //chrome.Close();
                    //chrome.Quit();
                    
                    chrome.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div/div[1]/form/div[1]/div[1]/div[1]/div[1]/div/div[1]/input")).SendKeys("Duy");
                    chrome.FindElement(By.Name("lastname")).SendKeys("anh");
                    Random r = new Random();
                    Thread.Sleep(1000);// delay 1s
                    int ngay = r.Next(1, 30);
                    string ngay1 = $"/html/body/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div/div[1]/form/div[1]/div[2]/div[2]/span/span/select[1]/option[{ngay}]";
                    chrome.FindElement(By.XPath(ngay1)).Click(); Thread.Sleep(1000);// delay 1s
                    int thang = r.Next(1, 12);
                    string thang1 = $"/html/body/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div/div[1]/form/div[1]/div[2]/div[2]/span/span/select[2]/option[{thang}]";
                    chrome.FindElement(By.XPath(thang1)).Click(); Thread.Sleep(1000);// delay 1s
                    int nam = r.Next(26, 62);
                    string nam1 = $"/html/body/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div/div[1]/form/div[1]/div[2]/div[2]/span/span/select[3]/option[{nam}]";
                    chrome.FindElement(By.XPath(nam1)).Click(); Thread.Sleep(1000);// delay 1s
                    int gioitinh = r.Next(1, 9999);
                    if (gioitinh % 2 == 0)
                    {
                        chrome.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div/div[1]/form/div[1]/div[4]/span/span[1]/label/input")).Click();
                    }
                    else
                    {
                        chrome.FindElement(By.XPath("/html/body/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div/div[1]/form/div[1]/div[4]/span/span[2]/label/input")).Click();

                    }
                    Thread.Sleep(1000);// delay 1s
                    js.ExecuteScript("window.open();");// tạo tab mới
                    var tab = chrome.WindowHandles;// get có bn tab
                    chrome.SwitchTo().Window(tab[1]);// chuyển hướng chrome qua tab new
                    chrome.Navigate().GoToUrl("https://www.mailforspam.com/");
                    while (true)
                    {
                        try
                        {
                            chrome.FindElement(By.Name("spammail")); break;
                        }
                        catch { }
                        Thread.Sleep(1000);
                    }
                    chrome.FindElement(By.Name("spammail")).SendKeys("sdf343das" + r.Next(1, 999999999));
                    Thread.Sleep(1000);// delay 1s
                    chrome.FindElement(By.Id("button")).Click();
                    while (true)
                    {
                        try
                        {
                            chrome.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div/div[1]/h2")); break;
                        }
                        catch { }
                        Thread.Sleep(1000);
                    }
                    string email = chrome.FindElement(By.XPath("/html/body/div[1]/div[3]/div/div/div[1]/h2")).Text;
                    Thread.Sleep(1000);// delay 1s
                    chrome.SwitchTo().Window(tab[0]);
                    chrome.FindElement(By.Name("reg_email__")).SendKeys(email); Thread.Sleep(1000);// delay 1s
                    chrome.FindElement(By.Name("reg_passwd__")).SendKeys("0349dfj$@#$257845"); Thread.Sleep(1000);// delay 1s
                    chrome.FindElement(By.Name("websubmit")).Click();
                    while (true)///dùng để ktra nút đăng ký tồn tại ahy k nếu tồn tại thì nó chưa đăng ký thành công , nếu k tồn tại nó đã đăng ký thành công và bắt đầu ktra live
                    {
                        try
                        {
                            chrome.FindElement(By.Name("websubmit")); Thread.Sleep(1000);// delay 1s

                        }
                        catch { break; }
                    }
                    if (chrome.Url.Contains("confirmemail"))///ktra tồn tại Enter the confirmation code from the text message hay k
                    {
                        File.AppendAllText("acclive.txt", email+"|"+ "0349dfj$@#$257845"+Environment.NewLine);// lưu dữ liệu  vào file
                        chrome.SwitchTo().Window(tab[1]);
                        chrome.Navigate().GoToUrl($"https://www.mailforspam.com/mail/{email.Split('@')[0]}/1");
                        Thread.Sleep(3000);// delay 1s
                        var html = chrome.PageSource;
                        Match match = Regex.Match(html, @"FB-(\d+)");
                        if (match.Success)
                        {
                            string code = match.Groups[1].Value;
                            chrome.SwitchTo().Window(tab[0]);
                            chrome.FindElement(By.Id("code_in_cliff")).SendKeys(code);
                            chrome.FindElement(By.XPath("/html/body/div[1]/div[2]/div[1]/div/div/div[1]/div[2]/form/div[2]/div/button")).Click();
                            Console.WriteLine("Mã xác nhận là: " + code);
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy mã.");
                        }
                        Console.WriteLine("acc live");
                    }
                    else
                    {
                        Console.WriteLine("acc die");
                    }
                    /*chrome.Navigate().GoToUrl("https://trangchudangnhap.com/vn/vn");
                    js.ExecuteScript("document.querySelector(\"body > app-root > app-float-widget > div > div > div.cdk-drag.float-widget__draggable.cdk-drag-disabled.float-widget__draggable--ready > div > div > div\").click();");
                    try
                    {
                        var element = chrome.FindElement(By.XPath("/html/body/ng-component/div/iframe"));
                        chrome.SwitchTo().Frame(element);//chuyển iframe
                        chrome.FindElement(By.Id("name")).SendKeys("sdfsdf");
                        chrome.SwitchTo().DefaultContent();// quay về iframe ban đầu

                    }
                    catch { }*/
                    chrome.Quit();

                }); t.Start();
                threads.Add(t);
                demluong++;
                Thread.Sleep(800);// delay 1s
                if (demluong >= 2)
                {
                    foreach (var item in threads)
                    {
                        item.Join();
                    }
                    threads.Clear();
                    demluong = 0;
                }
            }
            Console.ReadKey();
        }
    }
}
