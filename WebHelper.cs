using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class WebHelper
{
    private static WebClient _WebClient = new();

    public static string GetUrlFromGoogle(string deviceName)
    {
        deviceName = deviceName.Replace(' ', '+');
        string url = "https://www.google.com.hk/search?q=gsmarena+" + deviceName;
        string pageHtml = GetPageContent(url);
        int index1 = pageHtml.IndexOf("https://www.gsmarena.com");
        if (index1 < 0) throw new Exception("Error: Google Page doesn't contains 'https://www.gsmarena.com' !");
        int index2 = pageHtml.IndexOf(".php", index1);
        if (index1 < 0) throw new Exception("Error: Google Page doesn't contains '.php' after 'https://www.gsmarena.com' !");
        return pageHtml.Substring(index1, index2 - index1 + 4);
    }

    public static string GetPageContent(string url)
    {
        string retUrl = "";
        try
        {
            _WebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = _WebClient.DownloadData(url);//从指定网站下载数据
            retUrl = Encoding.UTF8.GetString(pageData);//如果获取网站页面采用的是GB2312，则使用Encoding.default
        }
        catch (WebException webEx)
        {
            Console.WriteLine("[GetPageContent] WebException: " + webEx.Message.ToString());
        }
        return retUrl;
    }
}
