using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace мини_CRM
{
    public class Client
    {
        public int Id { get; set; }
        public string Имя { get; set; }
        public string Фамилия { get; set; }
        public string Отчество { get; set; }
        public string Email { get; set; }
        public string Телефон { get; set; }
        public string Адрес { get; set; }
        public string Компания { get; set; }
        public string Должность { get; set; }
        public DateTime ДатаРегистрации { get; set; } = DateTime.Now;
        public string ДополнительнаяИнформация { get; set; }

        // Конструктор по умолчанию (необходим для сериализации/десериализации)
        public Client() { }

        public override string ToString()
        {
            return $"{Фамилия} {Имя} {Отчество}"; // Как будет отображаться клиент в ListBox
        }
    }
}