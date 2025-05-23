using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace мини_CRM
{
    public enum СтатусЗаказа
    {
        Принят,
        ВОбработке,
        Собран,
        Отправлен,
        Доставлен,
        Отменен
    }

    public class Order  // <-- Изменили internal на public
    {
        public int Id { get; set; }
        public int ClientId { get; set; } // ID клиента, которому принадлежит заказ
        public DateTime ДатаРазмещения { get; set; } = DateTime.Now;
        public string Описание { get; set; }
        public СтатусЗаказа Статус { get; set; }
        public decimal Сумма { get; set; }
        public List<string> Товары { get; set; } = new List<string>();

        public Client Client { get; set; } // Ссылка на объект Client (для удобства)

        // Конструктор по умолчанию (необходим для сериализации/десериализации)
        public Order() { }

        public override string ToString()
        {
            return $"Заказ #{Id} - {Описание} - {Статус}";
        }
    }
}