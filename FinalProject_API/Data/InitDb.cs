using FinalProject_API.AuthFinalProjectApp;
using FinalProject_API.ContextFolder;
using FinalProject_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject_API.Data
{
    public static class DbInitializer
    {
        /// <summary>
        /// Метод инициализации (происходит в процессе запуска)
        /// в результате которого происходит проверка на наличие
        /// в базе данных, таблицы Title любого поля.
        /// Если поля отсутствуют, то происходит создание
        /// нового титульного поля. 
        /// Далее, происходит открытие новой транзакции, в которой
        /// с помощью метода Add происходит добавление в базу данных 
        /// указанной модели титульной страницы. Далее, методом ExecuteSqlRawAsync
        /// происходит включение автоинкрементации, сохранение данных
        /// методом SaveChanges и отключение автоинкрементации.
        /// Методом Commit происходит отключение транзакции.
        /// Если данные в БД уже есть, то метод завершается (не
        /// выполняется).
        /// </summary>
        /// <param name="context"></param>
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();
            if (context.Title.Any()) return;

            TitleModel title = new TitleModel
            {
                Title = "Оставить заявку или задать вопрос",
                BlogTitle = "Блог",
                ProjectsTitle = "Проекты",
                ServicesTitle = "Услуги",
                MainTitle = "Главная",
                ContactsTitle = "Контакты",
            };

            using (var trans = context.Database.BeginTransaction())
            {
                context.Title.Add(title);
                context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Edits] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Edits] OFF");
                trans.Commit();
            }
        }

        /// <summary>
        /// Метод инициализации (происходит в процессе запуска)
        /// в результате которого происходит проверка на наличие
        /// в базе данных, таблицы Contacts любого поля.
        /// Если поля отсутствуют, то происходит создание
        /// нового элемента Contacts. 
        /// Далее, происходит открытие новой транзакции, в которой
        /// с помощью метода Add происходит добавление в базу данных 
        /// указанной модели контакта. Далее, методом ExecuteSqlRawAsync
        /// происходит включение автоинкрементации, сохранение данных
        /// методом SaveChanges и отключение автоинкрементации.
        /// Методом Commit происходит отключение транзакции.
        /// Если данные в БД уже есть, то метод завершается (не
        /// выполняется).
        /// </summary>
        /// <param name="context"></param>
        public static void InitializeContacts(DataContext context)
        {
            context.Database.EnsureCreated();
            if (context.Contacts.Any()) return;
            Contacts contact = new Contacts
            {
                Id = 1,
                Address = "Какой-либо адрес",
                Email = "Какой-либо Email",
                Fax = "Какой-либо факс",
                Telephone = "Какой-либо телефон"
            };

            using (var trans = context.Database.BeginTransaction())
            {
                context.Contacts.Add(contact);
                context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Contacts] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Contacts] OFF");
                trans.Commit();
            }
        }
    }
}
