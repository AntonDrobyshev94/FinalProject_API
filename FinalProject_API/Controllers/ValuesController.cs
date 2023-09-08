using FinalProject_API.AuthFinalProjectApp;
using FinalProject_API.Data;
using FinalProject_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationData repositoryData;
        public IConfiguration Configuration { get; }
        public ValuesController(ApplicationData repositoryData)
        {
            this.repositoryData = repositoryData;
        }

        #region Application
        /// <summary>
        /// GET метод, возвращающий информацию о заявках
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<IApplication> Get()
        {
            return repositoryData.GetApplications();
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации.
        /// Метод добавления новой заявки в базу данных
        /// </summary>
        /// <param name="value"></param>
        // POST api/values
        [HttpPost]
        public void AddRequest([FromBody] Application value)
        {
            repositoryData.AddApplications(value);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод удаления заявки из базы данных
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public void Delete(int id)
        {
            repositoryData.DeleteApplication(id);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод для изменения статуса заявки по указанному ID
        /// </summary>
        /// <param name="application"></param>
        [HttpPost]
        [Route("ChangeStatus")]
        [Authorize(Policy = "AdminOnly")]
        public void ChangeStatusApplication(Application application)
        {
            repositoryData.ChangeStatus(application);
        }
        #endregion
        #region Projects
        /// <summary>
        /// GET метод, возвращающий информацию о проектах
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetProjects")]
        public IEnumerable<IProjectModel> GetProjects()
        {
            return repositoryData.GetProjects();
        }

        /// <summary>
        /// Post метод с атрибутом авторизации с ролью администратора, 
        /// принимающий модель проекта и изменяющий проект
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpPost]
        [Route("ChangeProject")]
        [Authorize(Policy = "AdminOnly")]
        public void ChangeProject(ProjectModel project)
        {
            repositoryData.ChangeProjects(project);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод добавления нового проекта в базу данных
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [Route("AddProject")]
        [Authorize(Policy = "AdminOnly")]
        public void AddProject([FromBody] ProjectModel project)
        {
            repositoryData.AddProjects(project);
        }
        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод удаления проекта из базы данных
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("DeleteProject/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public void DeleteProject(int id)
        {
            repositoryData.DeleteProject(id);
        }

        /// <summary>
        /// Get Асинхронный метод, предоставляющий информацию о
        /// выбранном проекте
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet]
        [Route("ProjectDetails/{id}")]
        public async Task<ProjectModel> ProjectDetails(int id)
        {
            return await repositoryData.GetProjectByID(id);
        }
        #endregion
        #region Services
        /// <summary>
        /// GET метод, возвращающий информацию об услугах
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetServices")]
        public IEnumerable<IService> GetServices()
        {
            return repositoryData.GetServices();
        }

        /// <summary>
        /// Post метод с атрибутом авторизации с ролью администратора, 
        /// изменяющий услугу
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpPost]
        [Route("ChangeService")]
        [Authorize(Policy = "AdminOnly")]
        public void ChangeService(Service service)
        {
            repositoryData.ChangeService(service);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора.
        /// Метод добавления новой услуги в базу данных
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [Route("AddService")]
        [Authorize(Policy = "AdminOnly")]
        public void AddService([FromBody] Service service)
        {
            repositoryData.AddService(service);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод удаления услуги из базы данных
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("DeleteService/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public void DeleteService(int id)
        {
            repositoryData.DeleteService(id);
        }

        /// <summary>
        /// Get Асинхронный метод, предоставляющий информацию о
        /// выбранной услуге
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet]
        [Route("FindService/{id}")]
        public async Task<Service> FindServiceById(int id)
        {
            return await repositoryData.GetServiceByID(id);
        }
        #endregion
        #region Blog
        /// <summary>
        /// GET метод, возвращающий информацию о блоге
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetBlog")]
        public IEnumerable<IBlogModel> GetBlog()
        {
            return repositoryData.GetBlog();
        }

        /// <summary>
        /// Post метод с атрибутом авторизации с ролью администратора, 
        /// изменяющий блог
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpPost]
        [Route("ChangeBlog")]
        [Authorize(Policy = "AdminOnly")]
        public void ChangeBlog(BlogModel blog)
        {
            repositoryData.ChangeBlog(blog);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора.
        /// Метод добавления записи в блоге в базу данных
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [Route("AddBlog")]
        [Authorize(Policy = "AdminOnly")]
        public void AddBlog([FromBody] BlogModel blog)
        {
            repositoryData.AddBlog(blog);
        }
        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод удаления блога из базы данных
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("DeleteBlog/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public void DeleteBlog(int id)
        {
            repositoryData.DeleteBlog(id);
        }

        /// <summary>
        /// Get Асинхронный метод, предоставляющий информацию о
        /// выбранном блоге (возвращающий экземпляр BlogModel)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet]
        [Route("BlogDetails/{id}")]
        public async Task<BlogModel> BlogDetails(int id)
        {
            return await repositoryData.GetBlogByID(id);
        }
        #endregion
        #region Contacts
        /// <summary>
        /// GET метод, возвращающий информацию о контактах
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetContacts")]
        public Contacts GetContacts()
        {
            return repositoryData.GetContacts();
        }

        /// <summary>
        /// Post метод с атрибутом авторизации с ролью администратора, 
        /// изменяющий контакты
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpPost]
        [Route("ChangeContact")]
        [Authorize(Policy = "AdminOnly")]
        public void ChangeTitle(Contacts contacts)
        {
            repositoryData.ChangeContacts(contacts);
        }
        #endregion
        #region Links
        /// <summary>
        /// GET метод, возвращающий информацию о ссылках
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetLinks")]
        public IEnumerable<ILinkModel> GetLinks()
        {
            return repositoryData.GetLinks();
        }

        /// <summary>
        /// Post метод с атрибутом авторизации с ролью администратора, 
        /// изменяющий ссылку
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpPost]
        [Route("ChangeLink")]
        [Authorize(Policy = "AdminOnly")]
        public void ChangeLink(LinkModel link)
        {
            repositoryData.ChangeLink(link);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора.
        /// Метод добавления новой ссылки в базу данных
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [Route("AddLink")]
        [Authorize(Policy = "AdminOnly")]
        public void AddLink([FromBody] LinkModel link)
        {
            repositoryData.AddLink(link);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод удаления ссылки из базы данных
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("DeleteLink/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public void DeleteLink(int id)
        {
            repositoryData.DeleteLink(id);
        }

        /// <summary>
        /// Get Асинхронный метод с атрибутом авторизации с ролью 
        /// администратора, предоставляющий экземпляр запрошенной 
        /// ссылки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/values/5
        [HttpGet]
        [Route("LinkDetails/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<LinkModel> LinkDetails(int id)
        {
            return await repositoryData.GetLinkByID(id);
        }
        #endregion
        #region Tag
        /// <summary>
        /// GET метод, возвращающий информацию о тэгах
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetTags")]
        public IEnumerable<ITagModel> GetTagsMethod()
        {
            return repositoryData.GetTags();
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора.
        /// Метод добавления нового тэга в базу данных
        /// </summary>
        /// <param name="value"></param>
        // POST api/values
        [HttpPost]
        [Route("AddTag")]
        [Authorize(Policy = "AdminOnly")]
        public void AddTagMethod([FromBody] TagModel tag)
        {
            repositoryData.AddTag(tag);
        }

        /// <summary>
        /// POST запрос с атрибутом авторизации с ролью администратора
        /// Метод удаления тэга из базы данных
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/values/5
        [HttpDelete]
        [Route("DeleteTag/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public void DeleteTag(int id)
        {
            repositoryData.DeleteTag(id);
        }
        #endregion
        #region Title
        /// <summary>
        /// GET метод, возвращающий информацию о титульной странице
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        [Route("GetTitle")]
        public TitleModel GetTitle()
        {
            return repositoryData.GetTitle();
        }

        /// <summary>
        /// Post метод с атрибутом авторизации с ролью администратора, 
        /// изменяющий проект
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpPost]
        [Route("ChangeTitle")]
        [Authorize(Policy = "AdminOnly")]
        public void ChangeTitle(TitleModel title)
        {
            repositoryData.ChangeTitle(title);
        }
        #endregion
        #region Authorization
        /// <summary>
        /// POST запрос для регистрации нового пользователя по
        /// указанным в модели данным.
        /// </summary>
        /// <param name="regData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserRegistration regData)
        {
            TokenResponseModel loginResponse = await repositoryData.Register(regData);
            if (loginResponse == null)
            {
                return Unauthorized();
            }
            return Ok(loginResponse);
        }

        /// <summary>
        /// POST запрос на аутентификацию пользователя по указанным
        /// аутентификационным данным
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginProp loginData)
        {
            TokenResponseModel loginResponse = await repositoryData.Login(loginData);
            if (loginResponse == null)
            {
                return Unauthorized();
            }
            return Ok(loginResponse);
        }

        /// <summary>
        /// Get метод, возвращающий bool переменную с ответом
        /// о валидности токена. Метод служит для проверки
        /// токена на валидность.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("CheckToken")]
        public bool CheckTokenMethod()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
