using System;
using System.Windows.Forms;

namespace мини_CRM
{
    public partial class OrderForm : Form
    {
        public Order Order { get; set; }  // Свойство для доступа к созданному заказу

        public OrderForm(int clientId) // Конструктор принимает ID клиента
        {
            InitializeComponent();

            // Сохраняем ClientId
            ClientId = clientId;

            // Инициализируем ComboBox статусов
            statusComboBox.Items.Add("В обработке");
            statusComboBox.Items.Add("Выполнен");
            statusComboBox.Items.Add("Отменен");
            statusComboBox.SelectedIndex = 0;// Выбираем "В обработке" по умолчанию

            Order = new Order();    // Инициализируем объект Order
            this.Text = "Создание заказа";
        }

        public int ClientId { get; set; }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Order.Описание = descriptionTextBox.Text;
            if (decimal.TryParse(amountTextBox.Text, out decimal сумма))
            {
                Order.Сумма = сумма;
            }
            else
            {
                MessageBox.Show("Неверный формат суммы.");
                return;
            }

            // Явно преобразуем индекс в значение перечисления СтатусЗаказа
            Order.Статус = (СтатусЗаказа)statusComboBox.SelectedIndex; // Изменено

            Order.ClientId = ClientId;

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