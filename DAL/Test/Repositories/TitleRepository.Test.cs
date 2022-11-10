using DynamoDB.DAL.App.Repositories;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Moq;

namespace DynamoDB.DAL.Test.Repositories
{
    public class TitleRepositoryTest
    {
        private ITitleRepository? _titleRepository;
        private Mock<ISaveEntityRepository<Title>> _mockSaveRepository = new Mock<ISaveEntityRepository<Title>>();
        private const string TITLE_ID = "titleId";

        [SetUp]
        public void SetUp()
        {
            _mockSaveRepository = new Mock<ISaveEntityRepository<Title>>();
        }

        [TearDown]
        public void Dispose()
        {
            _titleRepository = null;
        }

        [Test]
        public async Task WhenSaveCalledWithTitle_CallsSaveEntityRepositorySaveMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                var title = new Title(TITLE_ID, "New Movie", TitleType.Movie);
                _mockSaveRepository.Setup(s => s.Save(title)).ReturnsAsync(title);
                _titleRepository = new TitleRepository(_mockSaveRepository.Object, db);

                var retTitle = await _titleRepository.Save(title);

                Assert.That(retTitle, Is.EqualTo(title));
                _mockSaveRepository.Verify(s => s.Save(title), Times.Once);
            }
        }

        [Test]
        public async Task WhenDeleteCalledWithTitle_CallsSaveEntityRepositoryDeleteMethod()
        {
            using(var db = Configuration.GetDBContext())
            {
                var title = new Title(TITLE_ID, "Deleted Movie", TitleType.Movie);
                _titleRepository = new TitleRepository(_mockSaveRepository.Object, db);

                await _titleRepository.Delete(title);

                _mockSaveRepository.Verify(s => s.Delete(title), Times.Once);
            }
        }

        [Test]
        public async Task WhenGetCalledWithValidTitleId_ReturnsCorrectTitle()
        {
            using(var db = Configuration.GetDBContext())
            {
                var title = new Title(TITLE_ID, "New Movie", TitleType.Movie);
                _titleRepository = new TitleRepository(_mockSaveRepository.Object, db);

                try
                {
                    await db.SaveAsync(title, Globals.DB_CONFIG);

                    var retTitle = await _titleRepository.Get(TITLE_ID);

                    Assert.That(retTitle.Name, Is.EqualTo(title.Name));
                }
                finally
                {
                    await db.DeleteAsync(title, Globals.DB_CONFIG);
                }
            }
        }
    }
}
