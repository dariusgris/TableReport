using MailKit.Security;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TableReport.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;

namespace TableReport.Controllers
{
    internal class TableReportController
    {
        // Get outage object
        public Outage? GetOutageObject(string path)
        {
            Outage data = new();
            try
            {
                string jsonContent = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<Outage>(jsonContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            return data;
        }

        // Task 1
        public string CountNumbers(string input)
        {
            string text = input;
            string result = "";

            int mathSymbolIndex = text.IndexOfAny(new char[] { '*', '-', '+', '/' });

            if (mathSymbolIndex == -1)
            {
                string allNumbers = new string(text.Where(element => char.IsDigit(element)).ToArray());
                result = allNumbers != "" ? allNumbers : "0";
            }
            else
            {
                string firstPart = new string(text.Take(mathSymbolIndex)
                    .Where(element => char.IsDigit(element)).ToArray());
                string secondPart = new string(text.Skip(mathSymbolIndex)
                    .Where(element => char.IsDigit(element)).ToArray());

                int firstNumb = string.IsNullOrEmpty(firstPart) ? 0 : int.Parse(firstPart);
                int secondNumb = string.IsNullOrEmpty(secondPart) ? 0 : int.Parse(secondPart);

                char symbol = text[mathSymbolIndex];

                result = symbol switch
                {
                    '+' => $"{firstNumb} + {secondNumb} = {firstNumb + secondNumb}",
                    '-' => $"{firstNumb} - {secondNumb} = {firstNumb - secondNumb}",
                    '*' => $"{firstNumb} * {secondNumb} = {firstNumb * secondNumb}",
                    '/' => secondNumb != 0 ? $"{firstNumb} / {secondNumb} = {Math.Round((double)firstNumb / secondNumb, 1)}"
                    : "Can't devide by zero!",
                    _ => "Something went wrong!",
                };
            }

            return result;
        }
        // Task 2
        public void SentEmail (string email, string password, string recipient, string name, string recipientName, Outage outageObject)
        {
            string textForMessage = $"Start: {outageObject.OutageStart}," 
                + $" affected areas: {string.Join(", ", outageObject.AffectedAreas.Select(
                    area => $"{area.AreaName} - {area.AffectedCustomers}"
                + $" - {area.EstimatedRecoveryTime}"))}";
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(name, email));
                message.To.Add(new MailboxAddress(recipientName, recipient));
                message.Subject = "TableReport";

                var textPart = new TextPart("plain")
                {
                    Text = textForMessage
                };

                var multipart = new Multipart("mixed");
                multipart.Add(textPart);

                message.Body = multipart;

                int atIndex = email.IndexOf("@");
                if (atIndex != -1)
                {
                    string domain = email.Substring(atIndex + 1);
                    using (var client = new SmtpClient())
                    {
                        client.Connect($"smtp.{domain}", 587, SecureSocketOptions.StartTls);
                        client.Authenticate(email, password);
                        client.Send(message);
                        client.Disconnect(true);
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect mail!");
                    return;
                }
                MessageBox.Show("The letter has been sent!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        // Task 3
        public List<OutageTable> LoadDataIntoTable(Outage _outageObject)
        {
            List<OutageTable> outageObjForView = new List<OutageTable>();
            for (int i = 0; i < _outageObject?.AffectedAreas?.Count; i++)
            {
                var Tmp = new OutageTable
                {
                    OutageStart = _outageObject.OutageStart,
                    OutageEnd = _outageObject.OutageEnd,
                    AreaName = _outageObject.AffectedAreas[i].AreaName,
                    AffectedCustomers = _outageObject.AffectedAreas[i].AffectedCustomers,
                    Reason = _outageObject.AffectedAreas[i].Reason
                };
                outageObjForView.Add(Tmp);          
            }
            return outageObjForView;
        }
        public void SaveData(DataGrid outageDataGrid, Outage outageObject, string JsonFilePath)
        {
            List<OutageTable> outageList = (List<OutageTable>)outageDataGrid.ItemsSource;
            Outage editOutage = (Outage)outageObject.Clone();
            editOutage.OutageStart = outageList[0].OutageStart;
            editOutage.OutageEnd = outageList[0].OutageEnd;

            for (int i = 0; i < outageList.Count; i++)
            {
                if(i < editOutage?.AffectedAreas?.Count && editOutage?.AffectedAreas[i] != null) {
                    editOutage.AffectedAreas[i].AreaName = outageList[i].AreaName;
                    editOutage.AffectedAreas[i].AffectedCustomers = outageList[i].AffectedCustomers;
                    editOutage.AffectedAreas[i].Reason = outageList[i].Reason;
                }
                else {
                    var Tmp = new AffectedArea {
                        AreaId = $"area{i+1}",
                        AreaName = outageList[i].AreaName,
                        TotalCustomers = outageList[i].AffectedCustomers,
                        AffectedCustomers = outageList[i].AffectedCustomers,
                        EstimatedRecoveryTime = new DateTime(),
                        Reason = outageList[i].Reason,
                    };
                    editOutage?.AffectedAreas?.Add(Tmp); 
                }
            }
            try
            {
                string updatedJson = JsonConvert.SerializeObject(editOutage, Formatting.Indented);

                File.WriteAllText(JsonFilePath, updatedJson);
                MessageBox.Show("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
