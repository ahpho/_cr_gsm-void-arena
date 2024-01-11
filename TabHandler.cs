using System.Collections.Generic;

public class TabHandler
{
    public static List<DeviceInfo> Read(string inFile)
    {
        List<DeviceInfo> ret = new List<DeviceInfo>();

        string[] lines = File.ReadAllLines(inFile);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;
            if (line.StartsWith('#')) continue;

            string[] columns = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (columns.Length != 2)
            {
                string err = string.Format($"Skip Input Line [{i}]: {line}");
                Console.WriteLine(err);
                continue;
            }

            ret.Add(new DeviceInfo(columns[0], columns[1]));
        }

        return ret;
    }
}
