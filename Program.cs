// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

List<DeviceInfo> inputs = TabHandler.Read("Input.tab");
List<DeviceInfo> outputs = new List<DeviceInfo>();

StreamWriter sw = new StreamWriter("Output.tab");
sw.WriteLine("#name\t#count\t#memory\t#chipset\t#cpu_count\t#cpu_freq\t#cpu\t#gpu\t#launch\t#price\t#os");

int TestCount = 0;
foreach (DeviceInfo input in inputs)
{
    DeviceInfo info = new DeviceInfo(input.name, input.count);

    try
    {
        string url = Crawler.GetUrlFromGoogle(input.name);
        string page = Crawler.GetPageContent(url);
        if (string.IsNullOrEmpty(page)) throw new Exception("page is empty !");
        info = Gsmarena.Parse(page);
    }
    catch (Exception ex)
    {
        Console.WriteLine(string.Format($"[for] Exception, device.name={input.name}, ex={ex}"));
    }

    info.name = input.name;
    info.count = input.count;
    outputs.Add(info);

    string line = string.Format($"{info.name}\t{info.count}\t{info.memory}\t{info.chipset}\t{info.cpu_count}\t{info.cpu_freq}\t{info.cpu}\t{info.gpu}\t{info.launch}\t{info.price}\t{info.android}");
    sw.WriteLine(line);
    sw.Flush();

    Console.WriteLine(info.name);
    Thread.Sleep(2000);

    if (++TestCount > 10 && false)
        break;
}

sw.Close();
Console.WriteLine("All Done!");
