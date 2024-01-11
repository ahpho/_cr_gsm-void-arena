// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;

Console.WriteLine("Hello, World!");

try
{
    WebClient MyWebClient = new WebClient();
    MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
    Byte[] pageData = MyWebClient.DownloadData("http://www.163.com"); //从指定网站下载数据
    string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句    
                                                             //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
    Console.WriteLine(pageHtml);//在控制台输入获取的内容
    using (StreamWriter sw = new StreamWriter("c:\\test\\ouput.html"))//将获取的内容写入文本
    {
        sw.Write(pageHtml);
    }
    Console.ReadLine(); //让控制台暂停,否则一闪而过了    
}
catch (WebException webEx)
{
    Console.WriteLine(webEx.Message.ToString());
}
