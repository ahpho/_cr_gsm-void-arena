using System;
using System.Collections.Generic;
using System.Data.SQLite;

public class Sqlite // 暂时没用
{
    public static void Do(string inFile)
    {
        SQLiteConnection connection = new SQLiteConnection(GetConn());
    }

    private static string GetConn()
    {
        return @"data source="".\sqlite.db; version = 3;";
    }
}
