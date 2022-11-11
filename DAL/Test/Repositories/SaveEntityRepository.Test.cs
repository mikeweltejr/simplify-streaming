using DynamoDB.DAL.App.Repositories;
using DynamoDB.DAL.App.Repositories.Interfaces;

namespace DynamoDB.DAL.Test.Repositories
{
    public class SaveEntityRepositoryTest
    {
        private ISaveEntityRepository<User>? _repository;
        private IUserRepository? _userRepository;
        private const string USER_ID = "userId";
        private const string EMAIL = "email";

        [TearDown]
        public void Dispose()
        {
            _repository = null;
            _userRepository = null;
        }

        [Test]
        public async Task WhenSaveCalled_WithUser_SavesUserAsExpected()
        {
            using(var db = Configuration.GetDBContext())
            {
                var user = new User(USER_ID, EMAIL);

                try
                {
                    _repository = new SaveEntityRepository<User>(db);
                    var retUser = await _repository.Save(user);

                    Assert.That(retUser, Is.EqualTo(user));
                }
                finally
                {
                    await db.DeleteAsync(user, Globals.DB_CONFIG);
                }
            }
        }

        [Test]
        public async Task WhenDeleteCalled_WithUser_DeletesUserAsExpected()
        {
            using(var db = Configuration.GetDBContext())
            {
                var user = new User(USER_ID, EMAIL);

                await db.SaveAsync(user, Globals.DB_CONFIG);

                _repository = new SaveEntityRepository<User>(db);
                await _repository.Delete(user);

                _userRepository = new UserRepository(_repository, db);

                var retUser = await _userRepository.Get(USER_ID);

                Assert.Null(retUser);
            }
        }
    }
}
