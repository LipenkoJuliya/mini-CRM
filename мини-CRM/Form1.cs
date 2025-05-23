using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace мини_CRM
{
    public partial class Form1 : Form
    {
        private List<Client> clients = new List<Client>(); // Список клиентов
        private List<Order> orders = new List<Order>();   // Список заказов

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            LoadData();// Подписываемся на событие Load
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Загрузка данных (демонстрационных или из файла/БД)
            LoadSampleData();
            UpdateClientList(); // Обновляем список клиентов в ListBox
            LoadData();
        }

        private string dataFilePath = "crm_data.json"; // Имя файла для сохранения данных

        private void LoadSampleData()
        {
            // Пример создания нескольких клиентов и заказов для тестирования
            Client client1 = new Client() { Id = 1, Имя = "Иван", Фамилия = "Иванов", Отчество = "Иванович", Email = "ivan@example.com", Телефон = "123-45-67", Адрес = "ул. Ленина, 1", Компания = "Рога и копыта", Должность = "Директор", ДополнительнаяИнформация = "Любит кофе" }; // Добавлено отчество
            Client client2 = new Client() { Id = 2, Имя = "Петр", Фамилия = "Петров", Отчество = "Петрович", Email = "petr@example.com", Телефон = "765-43-21", Адрес = "ул. Пушкина, 10", Компания = "ЗАО Стройка", Должность = "Менеджер", ДополнительнаяИнформация = "Предпочитает чай" }; // Добавлено отчество

            clients.Add(client1);
            clients.Add(client2);

            Order order1 = new Order() { Id = 1, ClientId = 1, Описание = "Заказ №1", Статус = СтатусЗаказа.Принят, Сумма = 100 };
            order1.Client = client1; // Связываем заказ с клиентом
            Order order2 = new Order() { Id = 2, ClientId = 2, Описание = "Заказ №2", Статус = СтатусЗаказа.Отправлен, Сумма = 200 };
            order2.Client = client2;
            orders.Add(order1);
            orders.Add(order2);
        }

        private void UpdateClientList()
        {
            clientListBox.Items.Clear();
            foreach (var client in clients)
            {
                clientListBox.Items.Add(client); // Отображаем клиента (используется ToString())
            }
        }
        private void UpdateOrderList(int clientId)
        {
            clientOrdersListBox.Items.Clear();
            foreach (var order in orders.Where(o => o.ClientId == clientId))
            {
                clientOrdersListBox.Items.Add(order);
            }
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    string jsonData = File.ReadAllText(dataFilePath);

                    // Десериализуем данные в анонимный объект
                    var data = JsonConvert.DeserializeAnonymousType(jsonData, new { Clients = new List<Client>(), Orders = new List<Order>() });

                    // Присваиваем значения из десериализованного объекта
                    clients = data.Clients;
                    orders = data.Orders;

                    UpdateClientList(); // Обновляем список клиентов
                    if (clientListBox.SelectedItem != null)
                    {
                        Client selectedClient = (Client)clientListBox.SelectedItem;
                        UpdateOrderList(selectedClient.Id); // Обновляем список заказов для выбранного клиента
                    }
                }
                else
                {
                    // Файл не найден, создаем пустые списки
                    clients = new List<Client>();
                    orders = new List<Order>();
                    UpdateClientList();
                    UpdateOrderList(-1);  // -1 значит, заказы не привязаны ни к какому клиенту
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addClientButton_Click(object sender, EventArgs e)
        {
            ClientForm clientForm = new ClientForm(); // Создаем форму для ввода данных
            if (clientForm.ShowDialog() == DialogResult.OK) // Отображаем форму модально
            {
                Client новыйКлиент = clientForm.Client;  // Получаем клиента из формы
                // Присваиваем ID
                if (clients.Count > 0)
                {
                    новыйКлиент.Id = clients.Max(c => c.Id) + 1;
                }
                else
                {
                    новыйКлиент.Id = 1;
                }

                clients.Add(новыйКлиент); // Добавляем клиента в список
                UpdateClientList();        // Обновляем отображение списка
            }
        }

        private void editClientButton_Click(object sender, EventArgs e)
        {
            if (clientListBox.SelectedItem is Client selectedClient)
            {
                ClientForm clientForm = new ClientForm(selectedClient); // Передаем выбранного клиента
                if (clientForm.ShowDialog() == DialogResult.OK)
                {
                    //  Обновление информации о клиенте (не нужно, если используем привязку данных или свойства)
                    UpdateClientList();
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для редактирования.");
            }
        }

        private void deleteClientButton_Click(object sender, EventArgs e)
        {
            if (clientListBox.SelectedItem is Client selectedClient)
            {
                if (MessageBox.Show($"Удалить клиента {selectedClient.Имя} {selectedClient.Фамилия}?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clients.Remove(selectedClient);
                    UpdateClientList();
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления.");
            }
        }

        private void clientListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientListBox.SelectedItem is Client selectedClient)
            {
                DisplayClientDetails(selectedClient);
                DisplayClientOrders(selectedClient);
            }
        }

        private void DisplayClientDetails(Client client)
        {
            clientDetailsTextBox.Text = $"Имя: {client.Имя}\r\n" +
                                        $"Фамилия: {client.Фамилия}\r\n" +
                                        $"Отчество: {client.Отчество}\r\n" +  // <-- Добавлено отчество
                                        $"Email: {client.Email}\r\n" +
                                        $"Телефон: {client.Телефон}\r\n" +
                                        $"Компания: {client.Компания}\r\n" +
                                        $"Должность: {client.Должность}\r\n" +
                                        $"Дата регистрации: {client.ДатаРегистрации.ToShortDateString()}\r\n" +
                                        $"Дополнительная информация: {client.ДополнительнаяИнформация}";
        }

        private void DisplayClientOrders(Client client)
        {
            clientOrdersListBox.Items.Clear();
            foreach (var order in orders.Where(o => o.ClientId == client.Id))
            {
                clientOrdersListBox.Items.Add(order); // Используем ToString()
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text.ToLower();
            List<Client> filteredClients = clients.Where(c =>
                c.Имя.ToLower().Contains(searchText) ||
                c.Фамилия.ToLower().Contains(searchText) ||
                c.Email.ToLower().Contains(searchText))
                .ToList();

            clientListBox.Items.Clear();
            foreach (var client in filteredClients)
            {
                clientListBox.Items.Add(client);
            }
        }

        private void updateOrderStatusButton_Click(object sender, EventArgs e)
        {
            if (clientOrdersListBox.SelectedItem is Order selectedOrder)
            {
                // Создайте диалоговое окно для выбора статуса
                StatusChangeForm statusChangeForm = new StatusChangeForm(selectedOrder.Статус);
                if (statusChangeForm.ShowDialog() == DialogResult.OK)
                {
                    selectedOrder.Статус = statusChangeForm.SelectedStatus;
                    // Обновляем отображение статуса (или всего заказа)
                    DisplayClientOrders(selectedOrder.Client); // Перезагружаем список заказов
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для изменения статуса.");
            }
        }

        private void clientOrdersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clientOrdersListBox.SelectedItem is Order selectedOrder)
            {
                // Отображаем информацию о заказе (пример)
                clientDetailsTextBox.Text = $"Заказ №{selectedOrder.Id}\r\n" +
                                            $"Описание: {selectedOrder.Описание}\r\n" +
                                            $"Сумма: {selectedOrder.Сумма}\r\n" +
                                            $"Статус: {selectedOrder.Статус}";
            }
        }

        // Обработчик события для кнопки "Создать заказ"
        private void createOrderButton_Click(object sender, EventArgs e)
        {
            if (clientListBox.SelectedItem is Client selectedClient)
            {
                OrderForm orderForm = new OrderForm(selectedClient.Id); // Передаем ID клиента
                if (orderForm.ShowDialog() == DialogResult.OK)
                {
                    Order новыйЗаказ = orderForm.Order;
                    //Присваиваем ID заказу
                    if (orders.Count > 0)
                    {
                        новыйЗаказ.Id = orders.Max(c => c.Id) + 1;
                    }
                    else
                    {
                        новыйЗаказ.Id = 1;
                    }

                    // Связываем заказ с клиентом (важно)
                    новыйЗаказ.Client = selectedClient;
                    // Устанавливаем ClientId (важно)
                    новыйЗаказ.ClientId = selectedClient.Id;
                    orders.Add(новыйЗаказ);
                    DisplayClientOrders(selectedClient); // Обновляем список заказов
                }
            }
            else
            {
                MessageBox.Show("Выберите клиента для создания заказа.");
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем анонимный объект, содержащий списки клиентов и заказов
                var data = new
                {
                    Clients = clients,
                    Orders = orders
                };

                string jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(dataFilePath, jsonData);
                MessageBox.Show("Данные успешно сохранены.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(dataFilePath))
                {
                    string jsonData = File.ReadAllText(dataFilePath);

                    // Десериализуем данные в анонимный объект
                    var data = JsonConvert.DeserializeAnonymousType(jsonData, new { Clients = new List<Client>(), Orders = new List<Order>() });

                    // Присваиваем значения из десериализованного объекта
                    clients = data.Clients;
                    orders = data.Orders;

                    UpdateClientList(); // Обновляем список клиентов
                    if (clientListBox.SelectedItem != null)
                    {
                        Client selectedClient = (Client)clientListBox.SelectedItem;
                        UpdateOrderList(selectedClient.Id); // Обновляем список заказов для выбранного клиента
                    }


                    MessageBox.Show("Данные успешно загружены.", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Файл с данными не найден.", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.Title = "Сохранить данные как...";
            saveFileDialog.FileName = "crm_data.json"; // Имя по умолчанию

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                SaveDataToFile(filePath);
            }
        }
        private void SaveDataToFile(string filePath)
        {
            try
            {
                // Создаем анонимный объект для сериализации
                var data = new { Clients = clients, Orders = orders };

                // Сериализуем данные в JSON
                string jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

                // Сохраняем JSON в файл
                File.WriteAllText(filePath, jsonData);

                MessageBox.Show("Данные успешно сохранены.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
