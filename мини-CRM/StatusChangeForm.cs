using System;
using System.Windows.Forms;

namespace мини_CRM
{
    public partial class StatusChangeForm : Form
    {
        public СтатусЗаказа SelectedStatus { get; private set; }

        public StatusChangeForm(СтатусЗаказа currentStatus)
        {
            InitializeComponent();

            // Заполняем ComboBox значениями из перечисления СтатусЗаказа
            statusComboBox.DataSource = Enum.GetValues(typeof(СтатусЗаказа));

            // Устанавливаем выбранный статус
            statusComboBox.SelectedItem = currentStatus;
            SelectedStatus = currentStatus;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SelectedStatus = (СтатусЗаказа)statusComboBox.SelectedItem;
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
