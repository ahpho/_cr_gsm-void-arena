// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

List<DeviceInfo> inputs = TabHandler.Read("Input.tab");
List<DeviceInfo> outputs = new List<DeviceInfo>();

FileStream fs = File.Create("Output.tab");
StreamWriter sw = new StreamWriter(fs);
sw.WriteLine("#name\t#count\t#memory\t#chipset\t#cpu_count\t#cpu_freq\t#cpu\t#gpu\t#launch\t#price\t#os");

int TestCount = 0;
foreach (DeviceInfo input in inputs)
{
    string url = Crawler.GetUrlFromGoogle(input.name);
    string page = Crawler.GetPageContent(url);
    
    DeviceInfo info = Gsmarena.Parse(page);
    info.name = input.name;
    info.count = input.count;
    outputs.Add(info);

    string line = string.Format($"{info.name}\t{info.count}\t{info.memory}\t{info.chipset}\t{info.cpu_count}\t{info.cpu_freq}\t{info.cpu}\t{info.gpu}\t{info.launch}\t{info.price}\t{info.android}");
    sw.WriteLine(line);
    sw.Flush();
    fs.Flush();

    Console.WriteLine(info.name);
    Thread.Sleep(2000);

    if (++TestCount > 10 && false)
        break;
}
sw.Close();
fs.Close();
Console.WriteLine("All Done!");
