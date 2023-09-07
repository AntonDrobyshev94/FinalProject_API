using FinalProject_API.AuthFinalProjectApp;
using FinalProject_API.ContextFolder;
using FinalProject_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace FinalProject_API.Data
{
    public class ApplicationData
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext context;
        DbContextOptions<DataContext> options; 
        public IConfiguration _configuration { get; }

        public ApplicationData(DataContext context, UserManager<User> userManager,
                                IConfiguration configuration, 
                                DbContextOptions<DataContext> options)
        {
            this.context = context;
            _userManager = userManager;
            _configuration = configuration;
            this.options = options;
        }

        #region Application
        /// <summary>
        /// Метод добавления нового запроса (заявки), посредством
        /// обращения к методу Add datacontext ApplicationData.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        public void AddApplications(Application application)
        {
            context.Requests.Add(application);
            context.SaveChanges();
        }

        /// <summary>
        /// Метод, возаращающий последовательность коллекции объектов, 
        /// реализующих интерфейс IApplication с помощью интерфейса 
        /// IEnumberable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IApplication> GetApplications()
        {
            return this.context.Requests;
        }

        /// <summary>
        /// Асинхронный метод удаления заявки невозвращаемого
        /// типа, который принимает в себя int значение Id 
        /// удаляемой заявки. В методе используется
        /// директива using для определения границ текущего
        /// контекста для избежания ошибки ObjectDisposedException.
        /// Происходит создание экземпляра Application, в которую
        /// записывается результат перебора таблицы Requests
        /// базы данных на предмет совпадения принимаемого id
        /// с id заявки с помощью метода FirstOrDefaultAsync.
        /// При условии, что экземпляр Application не равен нулю
        /// из таблицы Requests происходит удаление полученного
        /// экземпляра с помощью метода Remove с последующим
        /// сохранением базы данных методом SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteApplication(int id)
        {
            using (var context = new DataContext(options))
            {
                Application application = await context.Requests.FirstOrDefaultAsync(x => x.ID == id);
                if (application != null)
                {
                    context.Requests.Remove(application);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Асинхронный метод поиска заявки, принимающий int id
        /// заявки и возвращающий экземпляр заявки Application. 
        /// Метод представлен лямбда выражением, в котором 
        /// происходит перебор таблицы Requests текущей базы данных
        /// на предмет совпадения Id заявки с принимаемым Id с 
        /// помощью метода FirstOrDefaultAsync. В итоге происходит
        /// возвращение полученного экземпляра Appliacation.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Application> GetApplicationByID(int id) => await context.Requests.FirstOrDefaultAsync(x => x.ID == id);

        /// <summary>
        /// Асинхронный метод изменения заявки невозвращаемого типа,
        /// который принимает в экземпляр заявки.
        /// В методе используется директива using для определения границ 
        /// текущего контекста для избежания ошибки ObjectDisposedException.
        /// Создается экземпляр Application, в который записывается результат
        /// перебора таблицы Requests текущей базы данных на предмет
        /// совпадения Id заявки с помощью метода FirstOrDefaultAsync.
        /// Далее происходит изменение параметров полученной заявки
        /// параметрами, которые были получены в принимаемом экземпляре
        /// Application. Результат изменений сохраняется с помощью метода
        /// SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contact"></param>
        public async void ChangeStatus(Application application)
        {
            using (var context = new DataContext(options))
            {
                Application concreteApplication = await context.Requests.FirstOrDefaultAsync(x => x.ID == application.ID);
                concreteApplication.Status = application.Status;
                await context.SaveChangesAsync();
            }
        }
        #endregion
        #region Projects
        /// <summary>
        /// Метод добавления проекта, посредством
        /// обращения к методу Add datacontext ProjectModel.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public void AddProjects(ProjectModel project)
        {
            context.Projects.Add(project);
            context.SaveChanges();
        }

        /// <summary>
        /// Метод изменения проекта, принимающий модель проекта
        /// ProjectModel. В методе используется директива using 
        /// для определения границ текущего контекста для избежания 
        /// ошибки ObjectDisposedException. В директиве создаётся
        /// экземпляр ProjectModel, в который асинхронно, с помощью
        /// метода FirstOrDefaultAsync, записывается результат
        /// перебора таблицы Projects на предмет совпадения id
        /// принимаемой модели и id хранящегося в базе данных проекта.
        /// Параметры полученного экземпляра перезаписываются на 
        /// параметры принимаемой методом модели.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public async void ChangeProjects(ProjectModel project)
        {
            using (var context = new DataContext(options))
            {
                ProjectModel concreteProject = await context.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);
                concreteProject.ImageName = project.ImageName;
                concreteProject.Name = project.Name;
                concreteProject.Description = project.Description;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод, возаращающий последовательность коллекции объектов, 
        /// реализующих интерфейс IProjectModel с помощью интерфейса 
        /// IEnumberable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IProjectModel> GetProjects()
        {
            return this.context.Projects;
        }

        /// <summary>
        /// Асинхронный метод удаления проекта невозвращаемого
        /// типа, который принимает в себя int значение Id 
        /// удаляемого проекта. В методе используется
        /// директива using для определения границ текущего
        /// контекста для избежания ошибки ObjectDisposedException.
        /// Происходит создание экземпляра ProjectModel, в который
        /// записывается результат перебора таблицы Projects
        /// базы данных на предмет совпадения принимаемого id
        /// с id проекта с помощью метода FirstOrDefaultAsync.
        /// При условии, что экземпляр ProjectModel не равен нулю
        /// из таблицы Projects происходит удаление полученного
        /// экземпляра с помощью метода Remove с последующим
        /// сохранением базы данных методом SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteProject(int id)
        {
            using (var context = new DataContext(options))
            {
                ProjectModel project = await context.Projects.FirstOrDefaultAsync(x => x.Id == id);
                if (project != null)
                {
                    context.Projects.Remove(project);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Асинхронный метод поиска проекта, принимающий int id
        /// проекта и возвращающий экземпляр проекта ProjectModel. 
        /// Метод представлен лямбда выражением, в котором 
        /// происходит перебор таблицы Projects текущей базы данных
        /// на предмет совпадения Id проекта с принимаемым Id с 
        /// помощью метода FirstOrDefaultAsync. В итоге происходит
        /// возвращение полученного экземпляра ProjectModel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProjectModel> GetProjectByID(int id) => await context.Projects.FirstOrDefaultAsync(x => x.Id == id);
        #endregion
        #region Services
        /// <summary>
        /// Метод добавления услуги, посредством
        /// обращения к методу Add datacontext Service.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public void AddService(Service service)
        {
            context.Services.Add(service);
            context.SaveChanges();
        }

        /// <summary>
        /// Метод изменения услуги, принимающий модель услуги
        /// Service. В методе используется директива using 
        /// для определения границ текущего контекста для избежания 
        /// ошибки ObjectDisposedException. Внутри директивы создаётся
        /// экземпляр Service, в которой асинхронно, с помощью
        /// метода FirstOrDefaultAsync, происходит перебор таблицы Services
        /// на предмет совпадения id принимаемой модели и id хранящейся в 
        /// базе данных услуги. Результат перебора сохраняется в ранее
        /// созданный экземпляр Service. Параметры полученного экземпляра 
        /// перезаписываются на параметры принимаемой методом модели.
        /// По окончанию производится сохранение методом SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public async void ChangeService(Service service)
        {
            using (var context = new DataContext(options))
            {
                Service concreteService = await context.Services.FirstOrDefaultAsync(x => x.Id == service.Id);
                concreteService.Description = service.Description;
                concreteService.Name = service.Name;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод, возаращающий последовательность коллекции объектов, 
        /// реализующих интерфейс IService с помощью интерфейса 
        /// IEnumberable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IService> GetServices()
        {
            return this.context.Services;
        }

        /// <summary>
        /// Асинхронный метод удаления услуги невозвращаемого
        /// типа, который принимает в себя int значение Id 
        /// удаляемой услуги. В методе используется
        /// директива using для определения границ текущего
        /// контекста для избежания ошибки ObjectDisposedException.
        /// Происходит создание экземпляра Service, в который
        /// записывается результат перебора таблицы Services
        /// базы данных на предмет совпадения принимаемого id
        /// с id услуги с помощью метода FirstOrDefaultAsync.
        /// При условии, что экземпляр Service не равен нулю
        /// из таблицы Services происходит удаление полученного
        /// экземпляра с помощью метода Remove с последующим
        /// сохранением базы данных методом SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteService(int id)
        {
            using (var context = new DataContext(options))
            {
                Service service = await context.Services.FirstOrDefaultAsync(x => x.Id == id);
                if (service != null)
                {
                    context.Services.Remove(service);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Асинхронный метод поиска заявки, принимающий int id
        /// заявки и возвращающий экземпляр заявки Service. 
        /// Метод представлен лямбда выражением, в котором 
        /// происходит перебор таблицы Services текущей базы данных
        /// на предмет совпадения Id заявки с принимаемым Id с 
        /// помощью метода FirstOrDefaultAsync. В итоге происходит
        /// возвращение полученного экземпляра Service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Service> GetServiceByID(int id) => await context.Services.FirstOrDefaultAsync(x => x.Id == id);
        #endregion
        #region Blog
        /// <summary>
        /// Метод добавления блога, посредством
        /// обращения к методу Add datacontext BlogModel.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        /// <param name="blog"></param>
        public void AddBlog(BlogModel blog)
        {
            context.Blogs.Add(blog);
            context.SaveChanges();
        }

        /// <summary>
        /// Метод изменения блога, принимающий модель блога
        /// BlogModel. В методе используется директива using 
        /// для определения границ текущего контекста для избежания 
        /// ошибки ObjectDisposedException. В директиве создаётся
        /// экземпляр BlogModel, в который асинхронно, с помощью
        /// метода FirstOrDefaultAsync, записывается результат
        /// перебора таблицы Blogs на предмет совпадения id
        /// принимаемой модели и id хранящегося в базе данных блога.
        /// Параметры полученного экземпляра перезаписываются на 
        /// параметры принимаемой методом модели.
        /// По окончанию производится сохранение методом SaveChanges.
        /// </summary>
        /// <param name="blog"></param>
        public async void ChangeBlog(BlogModel blog)
        {
            using (var context = new DataContext(options))
            {
                BlogModel concreteBlog = await context.Blogs.FirstOrDefaultAsync(x => x.Id == blog.Id);
                concreteBlog.Description = blog.Description;
                concreteBlog.BlogPost = blog.BlogPost;
                concreteBlog.Name = blog.Name;
                concreteBlog.ImageName = blog.ImageName;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод, возаращающий последовательность коллекции объектов, 
        /// реализующих интерфейс IBlogModel с помощью интерфейса 
        /// IEnumberable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBlogModel> GetBlog()
        {
            return this.context.Blogs;
        }

        /// <summary>
        /// Асинхронный метод удаления блога невозвращаемого
        /// типа, который принимает в себя int значение Id 
        /// удаляемого блога. В методе используется
        /// директива using для определения границ текущего
        /// контекста для избежания ошибки ObjectDisposedException.
        /// Происходит создание экземпляра BlogModel, в который
        /// записывается результат перебора таблицы Blogs
        /// базы данных на предмет совпадения принимаемого id
        /// с id блога с помощью метода FirstOrDefaultAsync.
        /// При условии, что экземпляр blog не равен нулю
        /// из таблицы Blogs происходит удаление полученного
        /// экземпляра blog с помощью метода Remove с последующим
        /// сохранением базы данных методом SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteBlog(int id)
        {
            using (var context = new DataContext(options))
            {
                BlogModel blog = await context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
                if (blog != null)
                {
                    context.Blogs.Remove(blog);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Асинхронный метод поиска блога, принимающий int id
        /// блога и возвращающий экземпляр блога BlogModel. 
        /// Метод представлен лямбда выражением, в котором 
        /// происходит перебор таблицы Blogs текущей базы данных
        /// на предмет совпадения Id блога с принимаемым Id с 
        /// помощью метода FirstOrDefaultAsync. В итоге происходит
        /// возвращение полученного экземпляра BlogModel.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BlogModel> GetBlogByID(int id) => await context.Blogs.FirstOrDefaultAsync(x => x.Id == id);
        #endregion
        #region Contacts
        /// <summary>
        /// Метод, возаращающий экземпляр Contacts
        /// </summary>
        /// <returns></returns>
        public Contacts GetContacts()
        {
            Contacts concreteContacts = new Contacts();
            foreach (var contacts in this.context.Contacts)
            {
                concreteContacts = contacts;
            }
            return concreteContacts;
        }

        /// <summary>
        /// Метод изменения контактных данных, принимающий модель 
        /// Contacts. В методе Создаётся экземпляр Contact. Далее,
        /// в директиве using происходит перебор таблицы Contacts
        /// базы данных в цикле foreach, в котором происходит 
        /// кеширование полученного в цикле контакта в ранее
        /// созданный экземпляр Contacts (условия в цикле 
        /// отсутствуют, так как в таблице содержится только один
        /// элемент, а создание других не предусмотрено логикой
        /// программы). Параметры полученного экземпляра перезаписываются 
        /// на параметры принимаемой методом модели.
        /// По окончанию производится сохранение методом SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public async void ChangeContacts(Contacts contacts)
        {
            Contacts concreteContacts = new Contacts();
            using (var context = new DataContext(options))
            {
                foreach (var contact in context.Contacts)
                {
                    concreteContacts = contact;
                }
                concreteContacts.Address = contacts.Address;
                concreteContacts.Telephone = contacts.Telephone;
                concreteContacts.Fax = contacts.Fax;
                concreteContacts.Email = contacts.Email;
                await context.SaveChangesAsync();
            }
        }
        #endregion
        #region Links
        /// <summary>
        /// Метод добавления ссылки, посредством
        /// обращения к методу Add datacontext LinkModel.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public void AddLink(LinkModel link)
        {
            context.Links.Add(link);
            context.SaveChanges();
        }

        /// <summary>
        /// Метод изменения ссылки, принимающий модель LinkModel.
        /// В методе используется директива using для определения 
        /// границ текущего контекста для избежания ошибки 
        /// ObjectDisposedException. Внутри директивы using создается
        /// экземпляр LinkModel и с помощью метода
        /// FirstOrDefaultAsync осуществляется поиск по совпадению
        /// Id принимаемой модели и элемента LinkModel таблицы Links.
        /// Параметры полученного экземпляра перезаписываются на 
        /// параметры принимаемой методом модели.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public async void ChangeLink(LinkModel link)
        {
            using (var context = new DataContext(options))
            {
                LinkModel concreteLink = await context.Links.FirstOrDefaultAsync(x => x.Id == link.Id);
                concreteLink.ImageName = link.ImageName;
                concreteLink.Url = link.Url;
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Метод, возаращающий последовательность коллекции объектов, 
        /// реализующих интерфейс ILinkModel с помощью интерфейса 
        /// IEnumberable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ILinkModel> GetLinks()
        {
            return this.context.Links;
        }

        /// <summary>
        /// Асинхронный метод удаления ссылки невозвращаемого
        /// типа, который принимает в себя int значение Id 
        /// удаляемой ссылки. В методе используется
        /// директива using для определения границ текущего
        /// контекста для избежания ошибки ObjectDisposedException.
        /// Происходит создание экземпляра LinkModel, в который
        /// записывается результат перебора таблицы Links
        /// базы данных на предмет совпадения принимаемого id
        /// с id ссылки с помощью метода FirstOrDefaultAsync.
        /// При условии, что экземпляр link не равен нулю
        /// из таблицы Links происходит удаление полученного
        /// экземпляра link с помощью метода Remove с последующим
        /// сохранением базы данных методом SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteLink(int id)
        {
            using (var context = new DataContext(options))
            {
                LinkModel link = await context.Links.FirstOrDefaultAsync(x => x.Id == id);
                if (link != null)
                {
                    context.Links.Remove(link);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Асинхронный метод поиска ссылки, принимающий int id
        /// ссылки и возвращающий экземпляр ссылки LinkModel. 
        /// Метод представлен лямбда выражением, в котором 
        /// происходит перебор таблицы Links текущей базы данных
        /// на предмет совпадения Id ссылки с принимаемым Id с 
        /// помощью метода FirstOrDefaultAsync. В итоге происходит
        /// возвращение полученного экземпляра LinkModel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<LinkModel> GetLinkByID(int id) => await context.Links.FirstOrDefaultAsync(x => x.Id == id);
        #endregion
        #region Title
        /// <summary>
        /// Метод, возаращающий экземпляр TitleModel. Происходит
        /// создание нового экземпляра TitleModel. Далее
        /// происходит перебор таблицы Title в цикле foreach 
        /// в котором происходит кеширование полученной страницы 
        /// в ранее созданный экземпляр TitleModel (условия в цикле 
        /// отсутствуют, так как в таблице содержится только один
        /// элемент, а создание других не предусмотрено логикой
        /// программы). По окончанию происходит возврат модели
        /// ключевым словом return.
        /// </summary>
        /// <returns></returns>
        public TitleModel GetTitle()
        {
            TitleModel concreteTitle = new TitleModel();
            foreach (var title in this.context.Title)
            {
                concreteTitle = title;
            }
            return concreteTitle;
        }

        /// <summary>
        /// Метод изменения титульных данных, принимающий модель 
        /// TitleModel. В методе Создаётся экземпляр TitleModel. Далее,
        /// в директиве using происходит перебор таблицы Title
        /// базы данных в цикле foreach, в котором происходит 
        /// кеширование полученного в цикле титульного листа в ранее
        /// созданный экземпляр TitleModel (условия в цикле 
        /// отсутствуют, так как в таблице содержится только один
        /// элемент, а создание других не предусмотрено логикой
        /// программы). Параметры полученного экземпляра перезаписываются 
        /// на параметры принимаемой методом модели.
        /// По окончанию производится сохранение методом SaveChanges.
        /// </summary>
        /// <param name="title"></param>
        public async void ChangeTitle(TitleModel title)
        {
            TitleModel concreteTitle = new TitleModel();
            using (var context = new DataContext(options))
            {
                foreach (var titleModel in context.Title)
                {
                    concreteTitle = titleModel;
                }
                 
                concreteTitle.Title = title.Title;
                concreteTitle.MainTitle = title.MainTitle;
                concreteTitle.BlogTitle = title.BlogTitle;
                concreteTitle.ServicesTitle = title.ServicesTitle;
                concreteTitle.ContactsTitle = title.ContactsTitle;
                concreteTitle.ProjectsTitle = title.ProjectsTitle;
                await context.SaveChangesAsync();
            }
        }
        #endregion
        #region Tags

        /// <summary>
        /// Метод, возаращающий последовательность коллекции объектов, 
        /// реализующих интерфейс ITagModel с помощью интерфейса 
        /// IEnumberable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITagModel> GetTags()
        {
            return this.context.Tags;
        }

        /// <summary>
        /// Метод добавления тэга, посредством
        /// обращения к методу Add datacontext TagModel.
        /// По окончанию производится сохранение методом
        /// SaveChanges.
        /// </summary>
        /// <param name="contact"></param>
        public void AddTag(TagModel tag)
        {
            context.Tags.Add(tag);
            context.SaveChanges();
        }

        /// <summary>
        /// Асинхронный метод удаления тэга невозвращаемого
        /// типа, который принимает в себя int значение Id 
        /// удаляемого тэга. В методе используется
        /// директива using для определения границ текущего
        /// контекста для избежания ошибки ObjectDisposedException.
        /// Происходит создание экземпляра TagModel, в которую
        /// записывается результат перебора таблицы Tags
        /// базы данных на предмет совпадения принимаемого id
        /// с id тэга с помощью метода FirstOrDefaultAsync.
        /// При условии, что экземпляр tag не равен нулю
        /// из таблицы Tags происходит удаление полученного
        /// экземпляра тэга с помощью метода Remove с последующим
        /// сохранением базы данных методом SaveChangesAsync.
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteTag(int id)
        {
            using (var context = new DataContext(options))
            {
                TagModel tag = await context.Tags.FirstOrDefaultAsync(x => x.ID == id);
                if (tag != null)
                {
                    context.Tags.Remove(tag);
                    await context.SaveChangesAsync();
                }
            }
        }
        #endregion
        #region Authorization
        /// <summary>
        /// Асинхронный метод входа в учетную запись, который принимает
        /// UserLoginProp модель, описанную отдельным классом и возвращает
        /// TokenResponseModel, описанную отдельным классом. В данном методе
        /// происходит создание экземпляра пользователя User newUser, в 
        /// параметр UserName которого записывается UserName принимаемой модели.
        /// Далее создается экземпляр User "user" в который записывается 
        /// результат перебора таблицы Users базы данных на предмет совпадения
        /// имени пользователя с именем пользователя принимаемой модели.
        /// Если полученный экземпляр равен нулю, то возвращается null,
        /// приводящий к завершению метода.
        /// Если экземпляр не равен нулю, то создается экземпляр PasswordHasher
        /// параметаризированный строкой. Далее создается новый обобщенный
        /// экземпляр, который является результатом работы метода 
        /// VerifyHashedPassword на экземпляре passwordHasher. Данный метод
        /// принимает значение пароля из полученной модели и переводит сверяет
        /// его с паролем текущего экземпляра пользователя в виде Хэш пароля.
        /// Далее, с помощью конструкции switch происходит проверка полученного
        /// экземпляра passwordVerificationResult. При результате 
        /// PasswordVerificationResult.Failed происходит возвращение null с 
        /// помощью ключевого слова return и завершение метода.
        /// Если результат не ошибочный, то метод продолжается и происходит
        /// создание переменной строкового типа id, в которую записывается
        /// значение Id текущего экземпляра User. Далее происходит создание
        /// экземпляра коллекции List строкового типа и в цикле foreach
        /// происходит перебор таблицы UserRoles базы данных и с условием,
        /// что параметр UserId текущей таблицы равен Id пользователя происходит
        /// запись Id роли в коллекцию с помощью метода Add. Далее, создается
        /// строковая переменная roleId для записи в неё id роли. В следующем 
        /// цикле foreach происходит перебор полученной ранее коллекции List на
        /// предмет равенства id единице (т.е., означает что пользователь - 
        /// администратор). Если найден id, равный 1, то происходит завершение
        /// цикла с записью id в переменную roleId. В противном случае результат
        /// в любом случае записывается в roleId, но он уже не будет равен 1.
        /// Далее происходит создание переменной строкового типа jwt, в которую
        /// будет записан результат выполнения метода GenerateJwtToken,
        /// который принимает в себя имя текущего экземпляра пользователя и id
        /// полученной роли roleId, после чего создается модель TokenResponseModel,
        /// описанная отдельным классом, с параметрами токена и имени пользователя.
        /// В параметры созданной модели записываются полученные ранее параметры
        /// токена и имени пользователя и происходит возврат модели с помощью 
        /// метода return
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        public async Task<TokenResponseModel> Login(UserLoginProp loginData)
        {
            User newUser = new User()
            {
                UserName = loginData.UserName
            };

            User? user = await context.Users.FirstOrDefaultAsync(p => p.UserName == newUser.UserName);

            if (user is null)
            {
                return null;
            }
            else
            {
                var passwordHasher = new PasswordHasher<string>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(null, user.PasswordHash, loginData.Password);
                switch (passwordVerificationResult)
                {
                    case PasswordVerificationResult.Failed:
                        return null;
                }
            }
            string id = user.Id;
            List<string> idCol = new List<string>();
            foreach (var item in context.UserRoles)
            {
                if (item.UserId == id)
                {
                    idCol.Add(item.RoleId);
                }
            }
            string roleId = string.Empty;
            foreach (var item in idCol)
            {
                if (item == "1")
                {
                    roleId = item;
                    break;
                }
                else
                {
                    roleId = item;
                }
            }
            if (roleId != null)
            {
                string jwt = GenerateJwtToken(user.UserName, roleId);

                var response = new TokenResponseModel
                {
                    access_token = jwt,
                    username = user.UserName
                };
                return (response);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Метод генерации JWT токена, принимающий строковые переменные
        /// имени пользователя и Id роли и возвращающий строковое значение
        /// токена. При условии, что Id роли равен 1 (т.е. это id 
        /// администратора) создаётся новая коллекция List класса Claim,
        /// в которую записываются даннные о имени пользователя и роли.
        /// Далее создается Jwt токен с конфигурационными параметрами,
        /// указанными в appsettings.json и хэш кодом 256. Время действия 
        /// токена задаётся равным 10 минутам. По окончанию происходит
        /// возвращение JWT токена в виде строки с помощью ключевого слова
        /// return. Если id роли не равен 1, то создается токен обычного
        /// пользователя.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string userName, string roleId)
        {
            if (roleId == "1")
            {
                var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Role, "Admin")
                        };
                var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"]!,
                audience: _configuration["Jwt:Audience"]!,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)
                    ),
                    SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            else
            {
                var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, userName),
                            new Claim(ClaimTypes.Role, "User")
                        };
                var jwt = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"]!,
                audience: _configuration["Jwt:Audience"]!,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)
                    ),
                    SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
        }

        /// <summary>
        /// Асинхронный метод регистрации нового пользователя, принимающий
        /// модель UserRegistration, описанную отдельным классом и 
        /// возвращающий TokenResponseModel, описанный отдельным классом.
        /// В методе происходит создание экземпляра нового пользователя с
        /// именем из принимаемой модели. С помощью метода CreateAsync 
        /// происходит создание пароля для пользовательского аккаунта   
        /// экземпляра User который проходит процедуру хэш кодирования. 
        /// Результат метода содержится в переменной createResult. Далее,
        /// с помощью метода AddToRoleAsync происходит добавление роли 
        /// User для вновь созданного пользовательского аккаунта. Результат
        /// выполнения метода сохраняется в переменной addRoleResult.
        /// С помощью метода GenerateJwtToken происходит создание нового 
        /// токена для пользовательского аккаунта. Далее происходит
        /// сохранение результатов, а именно токена и имени пользователя
        /// в модель TokenResponseModel, результат который возвращается
        /// с помощью ключевого слова return, при условии, что createResult 
        /// и addRoleResult возвращают значение Succeeded. В обратном 
        /// случае возвращается null.
        /// </summary>
        /// <param name="registrData"></param>
        /// <returns></returns>
        public async Task<TokenResponseModel> Register(UserRegistration registrData)
        {
            var user = new User { UserName = registrData.LoginProp };
            var createResult = await _userManager.CreateAsync(user, registrData.Password);
            var addRoleResult = await _userManager.AddToRoleAsync(user, "User");
            var jwt = GenerateJwtToken(user.UserName, "2");

            var response = new TokenResponseModel
            {
                access_token = jwt,
                username = user.UserName
            };

            if (createResult.Succeeded && addRoleResult.Succeeded)
            {
                return response;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
