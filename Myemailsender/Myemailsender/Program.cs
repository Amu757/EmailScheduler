using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Myemailsender;
using Microsoft.VisualBasic;
/*

    Author : Aman R. Waghmare
    Date   : 16-04-2023
    what it does :   this program read json file deserialize it and sends emails to receivers mentioned in json.
                     
*/
namespace Myemailsender
{
    [Serializable]  // this class is use to deserialize json file into object
    internal class DbBackupfile
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> ToSuccessful { get; set; }
        public string CCSuccessful { get; set; }
        public List<string> ToFailure { get; set; }
        public string CCFailure { get; set; }
        public List<string> Repeat { get; set; }
        public string TimeOfBackup { get; set; }
        public string OverwriteAfter { get; set; }
        public string EveryHour { get; set; }
    }

    class Autoemail   // sends email Automaticaly
    {
        public static void Main(string[] args)
        {

            DbBackupfile myobj = Autoemail.readdata();
            string[] toSuccessful = myobj.ToSuccessful[0].ToString().Split(',');
            string[] ToFailure = myobj.ToFailure[0].ToString().Split(',');

            Console.WriteLine(toSuccessful[0]);
            Console.WriteLine(toSuccessful[1]);
            Console.WriteLine(toSuccessful[2]);

            if (myobj.CCSuccessful == "yes")
            {
                sendemail(myobj, toSuccessful[0]);
                sendemail(myobj, toSuccessful[1]);
                sendemail(myobj, toSuccessful[2]);
            }
            else if (myobj.CCSuccessful == "no")
            {
                sendemail(myobj, ToFailure[0]);
                sendemail(myobj, ToFailure[1]);
                sendemail(myobj, ToFailure[2]);
            }
            else //
            {
                Console.WriteLine("ccSuccessful must be not null...");
            }

            //printing data
            printdata(myobj);
        }

        public static void sendemail(DbBackupfile myobj, string tosuccessful)
        {
            Console.WriteLine("\n&&___Email sending application___&&\n");
            Console.WriteLine(tosuccessful);

            string fromMails = "amanwaghmare311@gmail.com";
            string fromPassword = "yzfujxdnlxgtnfwz";     // application password

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMails);
            message.Subject = myobj.Subject;
            message.To.Add(new MailAddress(tosuccessful));
            message.Body = myobj.Body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMails, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);

            Console.WriteLine("____email is sent to your recepient____");
        }

        public static DbBackupfile readdata()
        {
            string filename = "E:\\DbBackupdemo.json";

            if (File.Exists(filename))
            {
                string jsonf = File.ReadAllText(filename);

                DbBackupfile dbfileobj = JsonConvert.DeserializeObject<DbBackupfile>(jsonf);


                Console.WriteLine("\n\n*******perfectly working********");
                return dbfileobj;
            }
            else
            {
                Console.WriteLine("\nfile not found ");
                return null;
            }


        }

        public static void printdata(DbBackupfile dbfileobj)
        {
            Console.WriteLine("reading data from your json file");

            Console.WriteLine($"\nsubject:{dbfileobj.Subject}");

            Console.WriteLine($"\nbody:{dbfileobj.Body}");

            Console.WriteLine("ToSuccessful:");
            foreach (string email in dbfileobj.ToSuccessful)
            {
                Console.WriteLine($"  {email}");
            }

            if (dbfileobj.CCSuccessful != null)
            {
                Console.WriteLine("\nCCSuccessful:");
                Console.WriteLine($"  {dbfileobj.CCSuccessful}");
            }

            Console.WriteLine("\nToFailure:");
            foreach (string email in dbfileobj.ToFailure)
            {
                Console.WriteLine($"  {email}");
            }

            if (dbfileobj.CCFailure != null)
            {
                Console.WriteLine("\nCCFailure:");
                Console.WriteLine($"  {dbfileobj.CCSuccessful}");
            }

            Console.WriteLine("\nRepeat:");
            if (dbfileobj.Repeat != null)
            {
                foreach (string email in dbfileobj.Repeat)
                {
                    Console.WriteLine($"  {email}");
                }
            }

            Console.WriteLine($"\ntimeofbackup: {dbfileobj.TimeOfBackup}");
            Console.WriteLine($"\noverwriteafter: {dbfileobj.OverwriteAfter}");
            Console.WriteLine($"\neveryhour: {dbfileobj.EveryHour}");

        }

    }
}
