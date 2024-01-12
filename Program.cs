// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

string[] lines = File.ReadAllLines("Input.tab", Encoding.UTF8);
StreamWriter sw = new StreamWriter("Output.tab");
//sw.WriteLine("#name\t#count\t#memory\t#chipset\t#cpu_count\t#cpu_freq\t#cpu\t#gpu\t#launch\t#price\t#os");

int TestCount = 0;
foreach (string line in lines)
{
    string outLine = line;
    string name = "error", count = "error";
    try
    {
        if (!line.StartsWith('#'))
        {
            string[] columns = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (columns.Length == 2 || (columns.Length > 3 && columns[2] == "error"))
            {
                name = columns[0];
                count = columns[1];
                DeviceInfo d = new DeviceInfo(name, count);
                string url = WebHelper.GetUrlFromGoogle(name);
                string page = WebHelper.GetPageContent(url);
                if (string.IsNullOrEmpty(page)) throw new Exception("page is empty !");
                d = Gsmarena.Parse(page);
                d.name = name;
                d.count = count;
                outLine = string.Format($"{d.name}\t{d.count}\t{d.memory}\t{d.chipset}\t{d.cpu_count}\t{d.cpu_freq}\t{d.cpu}\t{d.gpu}\t{d.launch}\t{d.price}\t{d.android}");
                Console.WriteLine(name);
                Thread.Sleep(2000);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(string.Format($"[for] Exception, device.name={name}, ex={ex}"));
    }

    sw.WriteLine(outLine);
    sw.Flush();

    if (++TestCount > 10 && false)
        break;
}

sw.Close();
Console.WriteLine("All Done!");
