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
    DeviceInfo d = new DeviceInfo(input.name, input.count);

    try
    {
        string url = Crawler.GetUrlFromGoogle(input.name);
        string page = Crawler.GetPageContent(url);
        if (string.IsNullOrEmpty(page)) throw new Exception("page is empty !");
        d = Gsmarena.Parse(page);
    }
    catch (Exception ex)
    {
        Console.WriteLine(string.Format($"[for] Exception, device.name={input.name}, ex={ex}"));
    }

    d.name = input.name;
    d.count = input.count;
    outputs.Add(d);

    string line = string.Format($"{d.name}\t{d.count}\t{d.memory}\t{d.chipset}\t{d.cpu_count}\t{d.cpu_freq}\t{d.cpu}\t{d.gpu}\t{d.launch}\t{d.price}\t{d.android}");
    sw.WriteLine(line);
    sw.Flush();

    Console.WriteLine(d.name);
    Thread.Sleep(2000);

    if (++TestCount > 10 && false)
        break;
}

sw.Close();
Console.WriteLine("All Done!");
