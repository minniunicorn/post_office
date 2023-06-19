namespace post_office.Models
{
    public class Employee
    {
        // Свойства класса Employee
        public int ID { get; set; } // Идентификатор сотрудника
        public string Surname { get; set; } // Фамилия сотрудника
        public string Name { get; set; } // Имя сотрудника
        public string Lastname { get; set; } // Отчество сотрудника
        public string Position { get; set; } // Должность сотрудника
        public string Phone { get; set; } // Телефон сотрудника
        public string Login { get; set; } // Логин сотрудника
        public string Password { get; set; } // Пароль сотрудника
        public string Root { get; set; } // Уровень доступа сотрудника

    }
}
