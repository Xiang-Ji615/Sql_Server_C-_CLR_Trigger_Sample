using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using System.Net;

public partial class Triggers
{
    // Enter existing table or view for the target and uncomment the attribute line
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "SqlTrigger1", Target = "Name", Event = "FOR INSERT")]
    public static void SqlTrigger1()
    {
        // Replace with your own code
        SqlContext.Pipe.Send("Trigger FIRED");
        SqlTriggerContext triggContext = SqlContext.TriggerContext;
        String name = "";

        if (triggContext.TriggerAction == TriggerAction.Insert)
        {
            using (SqlConnection conn = new SqlConnection("context connection=true"))
            {
                conn.Open();
                SqlCommand sqlComm = new SqlCommand();
                SqlPipe sqlP = SqlContext.Pipe;

                sqlComm.Connection = conn;
                sqlComm.CommandText = "SELECT name from INSERTED";

                name = sqlComm.ExecuteScalar().ToString();

            }
        }


        var req = HttpWebRequest.Create("http://localhost:8080/Rest/HelloWorld?name="+name);

        req.ContentType = "application/json";
        req.Method = "GET";

        var response = req.GetResponse();
    }
}

