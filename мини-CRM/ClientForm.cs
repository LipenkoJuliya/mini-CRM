using System;
using System.Windows.Forms;

namespace мини_CRM
{
    public partial class ClientForm : Form
    {
        public Client Client { get; set; }

        public ClientForm()
        {
            InitializeComponent();
            Client = new Client();
        }

        public ClientForm(Client client)
        {
            InitializeComponent();
            Client = client;

            // Заполняем поля формы данными из объекта client
            nameTextBox.Text = client.Имя;
            lastNameTextBox.Text = client.Фамилия;
            middleNameTextBox.Text = client.Отчество; // <-- Убедитесь, что это есть
            emailTextBox.Text = client.Email;
            phoneTextBox.Text = client.Телефон;
            addressTextBox.Text = client.Адрес;
            companyTextBox.Text = client.Компания;
            positionTextBox.Text = client.Должность;
            additionalInfoTextBox.Text = client.ДополнительнаяИнформация;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Client.Имя = nameTextBox.Text;
            Client.Фамилия = lastNameTextBox.Text;
            Client.Отчество = middleNameTextBox.Text; // <-- Убедитесь, что это есть
            Client.Email = emailTextBox.Text;
            Client.Телефон = phoneTextBox.Text;
            Client.Адрес = addressTextBox.Text;
            Client.Компания = companyTextBox.Text;
            Client.Должность = positionTextBox.Text;
            Client.ДополнительнаяИнформация = additionalInfoTextBox.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
