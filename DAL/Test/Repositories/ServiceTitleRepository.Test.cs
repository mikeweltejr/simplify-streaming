using DynamoDB.DAL.App.Repositories;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;

namespace DynamoDB.DAL.Test.Repositories
{
    public class ServiceTitleRepositoryTest
    {
        private IServiceTitleRepository? _serviceTitleRepository;
        private Mock<ISaveEntityRepository<ServiceTitle>> _mockSaveRepository = new Mock<ISaveEntityRepository<ServiceTitle>>();
        private const string TITLE_ID = "titleId";
        private const string TITLE_NAME = "A Movie";
        private const string TITLE_ID_2 = "titleId2";
        private const string TITLE_NAME_2 = "Another Movie";

        [SetUp]
        public void SetUp()
        {
            _mockSaveRepository = new Mock<ISaveEntityRepository<ServiceTitle>>();
        }

        [TearDown]
        public void Dispose()
        {
            _serviceTitleRepository = null;
        }

        [Test]
        public async Task WhenSaveCalledWithServiceTitle_CallsSaveEntityRepositorySaveMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                var serviceTitle = new ServiceTitle(Service.Disney, TITLE_ID, TITLE_NAME);
                _serviceTitleRepository = new ServiceTitleRepository(_mockSaveRepository.Object, db);
                _mockSaveRepository.Setup(s => s.Save(serviceTitle)).ReturnsAsync(serviceTitle);

                var retServiceTitle = await _serviceTitleRepository.Save(serviceTitle);

                Assert.That(retServiceTitle, Is.EqualTo(serviceTitle));
                _mockSaveRepository.Verify(s => s.Save(serviceTitle), Times.Once);
            }
        }

        [Test]
        public async Task WhenDeleteCalledWithServiceTitle_CallsSaveEntityRepositoryDeleteMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                var serviceTitle = new ServiceTitle(Service.Disney, TITLE_ID, TITLE_NAME);
                _serviceTitleRepository = new ServiceTitleRepository(_mockSaveRepository.Object, db);

                await _serviceTitleRepository.Delete(serviceTitle);

                _mockSaveRepository.Verify(s => s.Delete(serviceTitle), Times.Once);
            }
        }

        [Test]
        public async Task WhenGetCalledWithServiceWithTitles_ReturnsCorrectListOfTitles()
        {
            using(var db = Configuration.GetDBContext())
            {
                var serviceTitle1 = new ServiceTitle(Service.Disney, TITLE_ID, TITLE_NAME);
                var serviceTitle2 = new ServiceTitle(Service.Disney, TITLE_ID_2, TITLE_NAME_2);
                var serviceTitle3 = new ServiceTitle(Service.Netflix, "BadId", "Do Not Return");

                try
                {
                    await db.SaveAsync(serviceTitle1, Globals.DB_CONFIG);
                    await db.SaveAsync(serviceTitle2, Globals.DB_CONFIG);
                    await db.SaveAsync(serviceTitle3, Globals.DB_CONFIG);
                    _serviceTitleRepository = new ServiceTitleRepository(_mockSaveRepository.Object, db);

                    var serviceTitles = await _serviceTitleRepository.GetAll(Service.Disney);

                    Assert.That(serviceTitles.Count, Is.EqualTo(2));
                }
                finally
                {
                    await db.DeleteAsync(serviceTitle1, Globals.DB_CONFIG);
                    await db.DeleteAsync(serviceTitle2, Globals.DB_CONFIG);
                    await db.DeleteAsync(serviceTitle3, Globals.DB_CONFIG);
                }
            }
        }

        [Test]
        public async Task WhenGetCalledWithServiceWithoutTitles_ReturnsEmptyList()
        {
            using(var db = Configuration.GetDBContext())
            {
                var serviceTitle1 = new ServiceTitle(Service.Disney, TITLE_ID, TITLE_NAME);
                var serviceTitle2 = new ServiceTitle(Service.Disney, TITLE_ID_2, TITLE_NAME_2);
                var serviceTitle3 = new ServiceTitle(Service.Netflix, "BadId", "Do Not Return");

                try
                {
                    await db.SaveAsync(serviceTitle1, Globals.DB_CONFIG);
                    await db.SaveAsync(serviceTitle2, Globals.DB_CONFIG);
                    await db.SaveAsync(serviceTitle3, Globals.DB_CONFIG);
                    _serviceTitleRepository = new ServiceTitleRepository(_mockSaveRepository.Object, db);

                    var serviceTitles = await _serviceTitleRepository.GetAll(Service.Apple);

                    Assert.IsEmpty(serviceTitles);
                }
                finally
                {
                    await db.DeleteAsync(serviceTitle1, Globals.DB_CONFIG);
                    await db.DeleteAsync(serviceTitle2, Globals.DB_CONFIG);
                    await db.DeleteAsync(serviceTitle3, Globals.DB_CONFIG);
                }
            }
        }

        [Test]
        public async Task WhenGetStreamingServicesForTitleCalledWithTitleId_ReturnsCorrectListOfStreamingServices()
        {
            using(var db = Configuration.GetDBContext())
            {
                var serviceTitle1 = new ServiceTitle(Service.Disney, TITLE_ID, TITLE_NAME);
                var serviceTitle2 = new ServiceTitle(Service.Netflix, TITLE_ID, TITLE_NAME);
                var serviceTitle3 = new ServiceTitle(Service.Netflix, "BadId", "Do Not Return");

                try
                {
                    await db.SaveAsync(serviceTitle1, Globals.DB_CONFIG);
                    await db.SaveAsync(serviceTitle2, Globals.DB_CONFIG);
                    await db.SaveAsync(serviceTitle3, Globals.DB_CONFIG);
                    _serviceTitleRepository = new ServiceTitleRepository(_mockSaveRepository.Object, db);

                    var serviceTitles = await _serviceTitleRepository.GetStreamingServicesForTitle(TITLE_ID);

                    Assert.That(serviceTitles.Count, Is.EqualTo(2));
                }
                finally
                {
                    await db.DeleteAsync(serviceTitle1, Globals.DB_CONFIG);
                    await db.DeleteAsync(serviceTitle2, Globals.DB_CONFIG);
                    await db.DeleteAsync(serviceTitle3, Globals.DB_CONFIG);
                }
            }
        }

        [Test]
        public async Task WhenGetCalled_WithValidServiceIdAndTitleId_ReturnsCorrectServiceTitle()
        {
            using(var db = Configuration.GetDBContext())
            {
                var serviceTitle = new ServiceTitle(Service.Disc, TITLE_ID, TITLE_NAME);

                try
                {
                    await db.SaveAsync(serviceTitle, Globals.DB_CONFIG);

                    _serviceTitleRepository = new ServiceTitleRepository(_mockSaveRepository.Object, db);

                    var retServiceTitle = await _serviceTitleRepository.Get(Service.Disc, TITLE_ID);

                    Assert.That(retServiceTitle?.ServiceId, Is.EqualTo(serviceTitle.ServiceId));
                    Assert.That(retServiceTitle?.TitleId, Is.EqualTo(serviceTitle.TitleId));
                    Assert.That(retServiceTitle?.TitleName, Is.EqualTo(serviceTitle.TitleName));
                }
                finally
                {
                    await db.DeleteAsync(serviceTitle, Globals.DB_CONFIG);
                }
            }
        }

        [Test]
        public async Task WhenGetCalled_WithInvalidServiceIdAndTitleId_ReturnsNull()
        {
            using(var db = Configuration.GetDBContext())
            {
                var serviceTitle = new ServiceTitle(Service.Disney, TITLE_ID, TITLE_NAME);

                try
                {
                    await db.SaveAsync(serviceTitle, Globals.DB_CONFIG);
                    _serviceTitleRepository = new ServiceTitleRepository(_mockSaveRepository.Object, db);

                    var retServiceTitle = await _serviceTitleRepository.Get(Service.Disney, "badTitleId");

                    Assert.Null(retServiceTitle);
                }
                finally
                {
                    await db.DeleteAsync(serviceTitle, Globals.DB_CONFIG);
                }

            }


        }
    }
}
