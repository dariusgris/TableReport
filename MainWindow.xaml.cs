using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TableReport.Models;
using TableReport.Controllers;

namespace TableReport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TableReportController _controller;
        private Outage _outageObject;
        public static string JsonFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}" 
            + "..\\..\\..\\Assets\\data.json";
        public MainWindow()
        {
            InitializeComponent();

            _controller = new TableReportController();
            _outageObject = GetOutageObject();

            LoadData(_outageObject);
        }
        private Outage GetOutageObject() => _controller.GetOutageObject(JsonFilePath);
        private void LoadData(Outage _outageObject)
        {
            List<OutageTable> outageObjForView = _controller.LoadDataIntoTable(_outageObject);
            outageDataGrid.ItemsSource = outageObjForView;
        }
        private void Count_Numbers(object sender, RoutedEventArgs e)
        {
            string result = _controller.CountNumbers(inputString.Text);
           
            MessageBox.Show(result);
        }
        private void Sent_Email(object sender, RoutedEventArgs e)
        {
            string nameSender = inputName.Text;
            string nameReceiver = inputNameReceiver.Text;
            string emailSender = inputEmail.Text;
            string password = inputPassword.Text;
            string recipientEmail = inputRecipient.Text;

            _controller.SentEmail(emailSender, password, recipientEmail, nameSender, nameReceiver, _outageObject);
        }
        private void Save_Edit(object sender, RoutedEventArgs e)
        {
            DataGrid outageGrid = outageDataGrid;
            _controller.SaveData(outageGrid, _outageObject, JsonFilePath);
        }
    }
}
